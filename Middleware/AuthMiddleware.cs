namespace UserManagementAPI.Middleware {
    public class AuthMiddleware {
        private readonly RequestDelegate _next;
        public AuthMiddleware(RequestDelegate next) => _next = next;

        public async Task Invoke(HttpContext context) {
        // ✅ Allow Swagger access without token
        if (context.Request.Path.StartsWithSegments("/swagger")) {
            await _next(context);
            return;
        }

        // 🔐 Check for token
        var token = context.Request.Headers["Authorization"].FirstOrDefault();
        if (token != "Bearer valid-token") {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("error: Unauthorized");
            return;
        }
        
        await _next(context);
        }
    }
}