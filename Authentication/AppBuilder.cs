using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using System.Text;
using Authentication.Core.Interfaces;
using Authentication.Core.Services;
using Microsoft.Extensions.Logging;
using Authentication.Controllers;

namespace Authentication
{
    public static class AppBuilder
    {
        public static void Configure(WebApplicationBuilder builder)
        {
            builder.Configuration.AddEnvironmentVariables(); // 👈 Asegura que se lean las env vars

            // Configura el binding de la sección MongoDBSettings con Environment Override
            // 1. Configurar las opciones de MongoDB desde appsettings.json

            _ = builder.Services.Configure<MongoDBSettings>(options =>
            {
                options.ConnectionString = Environment.GetEnvironmentVariable("MONGO_CONNECTION_STRING")
                    ?? builder.Configuration.GetSection("MongoDBSettings:ConnectionString").Value;

                options.DatabaseName = Environment.GetEnvironmentVariable("MONGO_DB_NAME")
                    ?? builder.Configuration.GetSection("MongoDBSettings:DatabaseName").Value;
            });


            // 2. Registrar el cliente de MongoDB como Singleton (recomendado)
            builder.Services.AddSingleton<IMongoClient>(sp =>
            {
                // Logging dentro del factory del singleton
                var logger = sp.GetRequiredService<ILogger<Program>>();
                var settings = sp.GetRequiredService<IOptions<MongoDBSettings>>().Value;
                logger.LogInformation("MongoDB connecting to database: {DatabaseName}", settings.DatabaseName);
                return new MongoClient(settings.ConnectionString);
            });

            // 3. Opcional pero útil: Registrar IMongoDatabase
            builder.Services.AddScoped<IMongoDatabase>(sp =>
            {
                var client = sp.GetRequiredService<IMongoClient>();
                var settings = sp.GetRequiredService<IOptions<MongoDBSettings>>().Value;
                return client.GetDatabase(settings.DatabaseName);
            });
            // 4. (Recomendado) Registrar tu propio servicio/repositorio
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IAuthService, AuthService>();


            builder.Services.AddControllers();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend", policy =>
                {
                    policy.WithOrigins("http://localhost:3000")
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes("a1f4e3c9d5b60789c3e2f1a0b4d6c8e7f9a1b2c3d4e5f6a7b8c9d0e1f2a3b4c5")),
                    ValidateLifetime = true,
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ClockSkew = TimeSpan.Zero
                };
            });
        }

        public static void ConfigureApp(WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("AllowFrontend");
            //app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
        }

        public class MongoDBSettings
        {
            public string ConnectionString { get; set; } = null!;
            public string DatabaseName { get; set; } = null!;
        }
    }
}
