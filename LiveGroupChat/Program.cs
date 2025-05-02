

using LiveGroupChat.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddRazorPages();
builder.Services.AddControllers();

// Dodaj SignalR do kontenera
builder.Services.AddSignalR();

var app = builder.Build();

app.UseSession();

// Middleware np. generujący UserId
app.UseMiddleware<UserIdMiddleware>();

app.UseStaticFiles();
app.UseRouting();

app.MapRazorPages();
app.MapControllers();

// ⬇️ Dodaj endpoint dla SignalR
app.MapHub<ChatHub>("/chatHub");

app.Run();