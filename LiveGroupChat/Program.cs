using System.Net.WebSockets;
using System.Text;
using LiveGroupChat.Middlewares;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDistributedMemoryCache(); // Użycie pamięci na potrzeby sesji
builder.Services.AddSession(options =>
{
    options.Cookie.HttpOnly = true;  // Bezpieczne przechowywanie ciasteczek
    options.Cookie.IsEssential = true; // Sesja jest niezbędna
});



builder.Services.AddRazorPages();
builder.Services.AddControllers(); // lub AddControllersWithViews()

var app = builder.Build();



app.UseSession();

// Rejestrujemy nasze middleware do generowania UserId
app.UseMiddleware<UserIdMiddleware>();


// Konfiguracja WebSocketów
var wsOptions = new WebSocketOptions
{
    KeepAliveInterval = TimeSpan.FromMinutes(2)
};
app.UseWebSockets(wsOptions);

// Middleware do WebSocketów
app.Use(async (context, next) =>
{
    if (context.Request.Path == "/ws")
    {
        if (context.WebSockets.IsWebSocketRequest)
        {
            using var webSocket = await context.WebSockets.AcceptWebSocketAsync();
            await Echo(webSocket);
        }
        else
        {
            context.Response.StatusCode = 400;
        }
    }
    else
    {
        await next();
    }
});

app.UseStaticFiles();
app.UseRouting();
app.MapRazorPages();
app.MapControllers(); 
app.Run();

static async Task Echo(WebSocket socket)
{
    var buffer = new byte[1024 * 4];
    var result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

    while (!result.CloseStatus.HasValue)
    {
        await socket.SendAsync(new ArraySegment<byte>(buffer, 0, result.Count), result.MessageType, result.EndOfMessage, CancellationToken.None);
        result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
    }

    await socket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
}