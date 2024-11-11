namespace WebAPI
{
    public class GlobalExceptionHandlerMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.StackTrace);
                context.Response.StatusCode = 400;
                await context.Response.WriteAsJsonAsync(new {Error = ex.Message});
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine(ex.StackTrace);
                context.Response.StatusCode = 404;
                await context.Response.WriteAsJsonAsync(new {Error = ex.Message});
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                context.Response.StatusCode = 500;
                await context.Response.WriteAsJsonAsync(new { Error = ex.Message});
            }
        }
    }
}