namespace LiveGroupChat.Middlewares
{
    public class UserIdMiddleware
    {
        private readonly RequestDelegate _next;
        private static readonly Random _random = new Random();

        public UserIdMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var userIdString = context.Session.GetString("UserId");

            if (string.IsNullOrEmpty(userIdString))
            {
                int userId = _random.Next(1, 1001);
                context.Session.SetString("UserId", userId.ToString());
            }

            await _next(context);
        }
    }
}