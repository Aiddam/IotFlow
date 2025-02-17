using Autofac;
using Autofac.Extensions.DependencyInjection;
using IotFlow.DataAccess;
using IotFlow.Models.DI;
using IotFlow.Middlewares;
using IotFlow.Transformers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Builder;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    IotFlow.DependencyInjector.Load(containerBuilder);
});

builder.Services.AddCors(o =>
{
    o.AddPolicy("DevCorsPolicy", config =>
    {
        config.AllowAnyHeader().AllowAnyMethod().AllowCredentials();

        if (builder.Environment.IsDevelopment())
        {
            config.WithOrigins(builder.Configuration["Origins:Test"]!);
        }
        else
        {
            config.WithOrigins(builder.Configuration["ORIGIN_TEST"]!);
        }
    });
});

// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.Conventions.Add(
        new RouteTokenTransformerConvention(new SlugifyParameterTransformer()));
}).AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Auth
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["JWT:AccessSecretCode"]!))
        };
    });
builder.Services.AddAuthorization();

builder.Services.AddDbContext<ServerContext>(options =>
{
    if (builder.Environment.IsDevelopment())
    {
        options.UseNpgsql(builder.Configuration.GetConnectionString("ConnectionString"));
    }
    else
    {
        options.UseNpgsql(builder.Configuration["SERVER_CONNECTION_STRING"]);
    }
});


builder.Services.Configure<JwtConfiguration>(jwtConfigurations =>
{
    if (builder.Environment.IsDevelopment())
    {
        jwtConfigurations.Audience = builder.Configuration["JWT:Audience"]!;
        jwtConfigurations.Issuer = builder.Configuration["JWT:Issuer"]!;
        jwtConfigurations.AccessLifetime = int.Parse(builder.Configuration["JWT:AccessLifetime"]!);
        jwtConfigurations.RefreshLifetime = int.Parse(builder.Configuration["JWT:RefreshLifetime"]!);
        jwtConfigurations.AccessSecretCode = builder.Configuration["JWT:AccessSecretCode"]!;
        jwtConfigurations.RefreshSecretCode = builder.Configuration["JWT:RefreshSecretCode"]!;
    }
    else
    {
        jwtConfigurations.Audience = builder.Configuration["JWT_AUDIENCE"]!;
        jwtConfigurations.Issuer = builder.Configuration["JWT_ISSUER"]!;
        jwtConfigurations.AccessLifetime = int.Parse(builder.Configuration["JWT_ACCESS_LIFETIME"]!);
        jwtConfigurations.RefreshLifetime = int.Parse(builder.Configuration["JWT_REFRESH_LIFETIME"]!);
        jwtConfigurations.AccessSecretCode = builder.Configuration["JWT_ACCESS_SECRET_CODE"]!;
        jwtConfigurations.RefreshSecretCode = builder.Configuration["JWT_REFRESH_SECRET_CODE"]!;
    }
});
builder.WebHost.UseUrls("http://0.0.0.0:5000");
var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseMiddleware<ErrorHandlerMiddleware>();

app.UseCors("DevCorsPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
