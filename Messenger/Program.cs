using MessengerData;
using MessengerData.Providers;
using MessengerData.Repository;
using Microsoft.OpenApi.Models;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Messenger.Hubs;
using Messenger.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>();
builder.Services.AddScoped<UserProvider>();
builder.Services.AddScoped<MessageProvider>();
builder.Services.AddScoped<AuthenticateProvider>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Insert token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
        new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
            }
        },
        new List<string>(){ }
        }
    });
});

builder.Services.AddAuthentication("OAuth")
    .AddJwtBearer("OAuth", config =>
    {
        byte[] secretBytes = Encoding.UTF8.GetBytes(builder.Configuration["AuthJwtKey"]!);

        var key = new SymmetricSecurityKey(secretBytes);

        config.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidIssuer = builder.Configuration["Issuer"],
            ValidAudience = builder.Configuration["Audience"],
            IssuerSigningKey = key
        };

        config.Events = new JwtBearerEvents
        {
            OnMessageReceived = (context) =>
            {
                var access_token = context.Request.Query["access_token"];

                if (!string.IsNullOrEmpty(access_token))
                {
                    context.Token = access_token;
                }

                return Task.CompletedTask;

            }

        };
    });

builder.Services.AddCors(option => {
    option.AddDefaultPolicy(builder =>
        builder
        //.AllowAnyOrigin()
        .WithOrigins(new string[] { "http://localhost:4200", "https://localhost:4200" })
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials()
         );
});

builder.Services.AddSignalR(config => {});

builder.Logging.AddConsole();
builder.Logging.AddAzureWebAppDiagnostics();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

//app.MapControllers();

//app.UseEndpoints(endpoints =>
//{
//    //endpoints.MapDefaultControllerRoute();
//    //endpoints.MapHub<MessengerHub>("/messenger");

//});

app.MapDefaultControllerRoute();
app.MapHub<MessengerHub>("/messenger");

app.MigrateDatabase();
app.Run();
