using BookingHotel.API.Extensions;
using BookingHotel.Application;
using BookingHotel.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddWebServices(builder.Configuration);
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

var app = builder.Build();

app.UseMiddleware<BookingHotel.API.Middlewares.ExceptionMiddleware>();
app.UseCors("AllowAll");
app.ConfigurePipeline();

app.Run();
