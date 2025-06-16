using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using restaurante.Models;
using restaurante.dbContext;
using restaurante.Interfaces;
using restaurante.Services;


var builder = WebApplication.CreateBuilder(args);

 //Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IPasswordHasher<Empleado>, PasswordHasher<Empleado>>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(option =>
    {
        option.LoginPath = "/Login/Login";
        option.ExpireTimeSpan = TimeSpan.FromMinutes(20);
        option.AccessDeniedPath = "/Home/Index";
    });


var connectionString = builder.Configuration.GetConnectionString("dbrestaurante") ?? throw new Exception("No hay Conexion A la base de datos");

builder.Services.AddDbContext<DbrestauranteContext>(conexion => conexion.UseMySql(connectionString,ServerVersion.AutoDetect(connectionString)));

builder.Services.AddScoped<IProducto, ProductoService>();
builder.Services.AddScoped<IComplemento,ComplementoService>();
builder.Services.AddScoped<IEmpleado, EmpleadoService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Login}/{id?}");       
app.Run();
