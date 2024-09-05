using MercadoPago.Config;
using MercadoPagoAPI.Infraestructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddHttpClient();

builder.Services
    .AddServiceConfiguration()
    .AddRepositoryConfiguration();

MercadoPagoConfig.AccessToken = builder.Configuration["MercadoPago:AccessToken"];

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();