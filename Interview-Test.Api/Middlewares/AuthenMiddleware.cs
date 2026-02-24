namespace Interview_Test.Middlewares;

public class AuthenMiddleware : IMiddleware
{
    private const string hashedKey = "<your hash sha512 x-api-key>";
    public Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var apiKeyHeader = context.Request.Headers["x-api-key"];
        if (string.IsNullOrEmpty(apiKeyHeader))
        {
            context.Response.StatusCode = 401;
            return context.Response.WriteAsync("API Key is missing");
        }
        
        // Implement validate x-api-key to authenticate the user here
        throw new NotImplementedException();
    }
}