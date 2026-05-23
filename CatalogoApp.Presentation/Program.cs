using CatalogoApp.Application.Services;
using CatalogoApp.Domain.Interfaces;
using CatalogoApp.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies; // <-- NUEVO

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// <-- NUEVO: Configuración de Autenticación por Cookies -->
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login"; // A dónde ir si no han iniciado sesión
        options.ExpireTimeSpan = TimeSpan.FromDays(7); // Recordar sesión por 7 días
    });

// Ruta del JSON
var jsonPath = Path.Combine(
    builder.Environment.ContentRootPath,
    "data",
    "items.json"
);

// Registrar repositorio y servicio
builder.Services.AddSingleton<IItemRepository>(new JsonItemRepository(jsonPath));
builder.Services.AddScoped<ItemService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication(); // <-- NUEVO: Debe ir estrictamente ANTES de UseAuthorization
app.UseAuthorization();
app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();