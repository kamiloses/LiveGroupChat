using System.Net.WebSockets;
using LiveGroupChat.Middlewares;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
var app = builder.Build();
app.UseWebSockets();

app.UseMiddleware<WebSocketMiddleware>();
app.UseStaticFiles();
app.UseRouting();

app.MapControllers();
//app.UseMvcWithDefaultRoute();

app.Run();