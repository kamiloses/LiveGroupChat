namespace LiveGroupChat.Middlewares
{
    public class UserIdMiddleware
    {
        private readonly RequestDelegate _next;
        private static readonly Random _random = new Random(); // statyczny, aby uniknąć problemu z seedem

        public UserIdMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Sprawdzamy, czy ID użytkownika już istnieje w sesji
            var userIdString = context.Session.GetString("UserId");

            if (string.IsNullOrEmpty(userIdString))
            {
                // Generujemy losowe ID i zapisujemy w sesji
                int userId = _random.Next(1, 1001); // zakres 1–1000 włącznie
                context.Session.SetString("UserId", userId.ToString());
            }

            // Przekazujemy kontrolę do następnego middleware
            await _next(context);
        }
    }
}