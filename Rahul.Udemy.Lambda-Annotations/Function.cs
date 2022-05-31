using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Lambda.Annotations;
using Amazon.Lambda.Core;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace Rahul.Udemy.Lambda_Annotations;

public class Function
{
    private readonly DynamoDBContext _dynamoDbContext;

    public Function()
    {
        _dynamoDbContext = new DynamoDBContext(new AmazonDynamoDBClient());
    }

    /// <summary>
    /// A simple function that takes a string and does a ToUpper
    /// </summary>
    /// <param name="input"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    [LambdaFunction]
    [HttpApi(LambdaHttpMethod.Get, "users/{userId}")]
    public async Task<User> FunctionHandler(string userId, ILambdaContext context)
    {
        Guid.TryParse(userId, out var id);
        return await _dynamoDbContext.LoadAsync<User>(id);
    }

    [LambdaFunction]
    [HttpApi(LambdaHttpMethod.Post, "users")]
    public async Task PostFunctionHandler([FromBody] User user, ILambdaContext context)
    {
        await _dynamoDbContext.SaveAsync(user);
    }
}

public class User
{
    public Guid Id { get; set; }
    public string Name { get; set; }
}