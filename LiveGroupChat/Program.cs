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

app.UseMiddleware<WebSocketMiddleware>();

app.UseStaticFiles();
app.UseRouting();
app.MapRazorPages();
app.MapControllers(); 
app.Run();
