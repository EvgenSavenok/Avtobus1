using Avtobus1.Application.Interfaces;
using Avtobus1.Application.Services;
using Avtobus1.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

// For this project server side rendering would be best solution
// Because frontend frameworks like Angular may be too redundant
builder.Services.AddControllersWithViews();
builder.Services.ConfigureSwagger();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.ConfigureSqlContext(builder.Configuration);
builder.Services.ConfigureRepositoryManager();
builder.Services.AddValidators();
builder.Services.AddScoped<IUrlService, UrlService>();

var app = builder.Build();

app.UseExceptionHandler("/Home/Error");

app.ApplyMigrations();

app.UseHttpsRedirection();
app.UseStaticFiles(); 

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();