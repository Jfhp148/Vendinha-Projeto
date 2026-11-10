using Microsoft.EntityFrameworkCore;
using Vendinha.Api.Data;
using Vendinha.Api.Middlewares;
using Vendinha.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<VendinhaContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("Padrao")));

builder.Services.AddScoped<ClienteService>();
builder.Services.AddScoped<DividaService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<VendinhaContext>();
    context.Database.EnsureCreated();
}

app.UseMiddleware<ErrosMiddleware>();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();
