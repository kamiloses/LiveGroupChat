using LiveGroupChat.Middlewares;
using LiveGroupChat.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;

var builder = WebApplication.CreateBuilder(args);

// Usługi dla pamięci podręcznej i sesji
builder.Services.AddDistributedMemoryCache();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSession(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddTransient<HomeService>();
// Usługi RazorPages i kontrolerów
builder.Services.AddRazorPages();
builder.Services.AddControllers();

// Usługa SignalR
builder.Services.AddSignalR();

// Konfiguracja DbContext z połączeniem do SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Ustawienia sesji
app.UseSession();

// Middleware do generowania UserId
//app.UseMiddleware<UserIdMiddleware>();

// Usługi statyczne
app.UseStaticFiles();
app.UseRouting();

// Mapowanie Razor Pages i kontrolerów
app.MapRazorPages();
app.MapControllers();

// Endpoint SignalR
app.MapHub<ChatHub>("/chatHub");

app.Run();