using EcommerceAPI.Extensions;
using EcommerceAPI.Helpers;
using EcommerceAPI.Middleware;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var DefaultConnection = builder.Configuration.GetConnectionString("DefaultConnection");

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddAutoMapper(typeof(MappingProfiles));

builder.Services.AddApplicationServices();
builder.Services.AddSwaggerServices();

// Learn more about configuring Swagger/OpenAPI at 
// https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<StoreContext>(x => {
    x.UseSqlServer(DefaultConnection);
});

var app = builder.Build();

using (var scope = app.Services.CreateScope()) {
    var services = scope.ServiceProvider;
    var loggerFactory = services.GetRequiredService<ILoggerFactory>();
    try {
        var context = services.GetRequiredService<StoreContext>();
        await context.Database.MigrateAsync();
        await StoreSeedContext.SeedAsync(context, loggerFactory);
    } catch (Exception ex) {
        var logger = loggerFactory.CreateLogger<Program>();
        logger.LogError(ex, "An error occured during migration");
    }
}

app.UseMiddleware<ExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerDocumention();
}

app.UseStatusCodePagesWithReExecute("/errors/{0}");

app.UseHttpsRedirection();

app.UseStaticFiles(); // for wwwroot/images but we dont need it!

app.UseAuthorization();

app.MapControllers();

app.Run();