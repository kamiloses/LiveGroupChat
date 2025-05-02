namespace LiveGroupChat.Middlewares;

public class UserIdMiddleware
{
    private readonly RequestDelegate _next;

    public UserIdMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Sprawdzamy, czy ID użytkownika już istnieje w sesji
        if (string.IsNullOrEmpty(context.Session.GetString("UserId")))
        {
            // Jeżeli nie, generujemy nowe ID i zapisujemy w sesji
            var userId = Guid.NewGuid().ToString();
            context.Session.SetString("UserId", userId);
        }

        // Przekazujemy kontrolę do następnego middleware
        await _next(context);
    }
}
