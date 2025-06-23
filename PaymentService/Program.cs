
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.Json;
using RabbitMQ.Stream.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using RabbitMQ.Stream.Client.Reliable;
var builder = WebApplication.CreateBuilder(args);
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenLocalhost(5001, listenOptions =>
    {
        listenOptions.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http1AndHttp2;
    });
});
var configuration = builder.Configuration;

builder.Services.AddGrpc();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<IEventPublisher, EventPublisher>();
builder.Services.AddSingleton<Task<StreamSystem>>(StreamSystem.Create(new StreamSystemConfig()));
builder.Services.AddDbContext<PaymentDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddDbContext<PaymentDbContext>(options =>
    options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<IPaymentService, PaymentServices>();
builder.Services.AddHostedService<WalletStreamConsumer>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var key = Encoding.ASCII.GetBytes(builder.Configuration["Jwt:SecretKey"]!);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateLifetime = true
    };
});
builder.Services.AddAuthorization();
var app = builder.Build();
app.MapGrpcService<PaymentGrpcService>();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    // Configure Swagger UI at the root URL
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Villa Backend API V1");
    c.RoutePrefix = string.Empty;
});

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();




app.Run();