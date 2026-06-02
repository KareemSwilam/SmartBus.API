
using Mapster;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using QuestPDF.Infrastructure;
using SmartBus.Application.DependencyInjection;
using SmartBus.Application.IServices;
using SmartBus.Application.Services;
using SmartBus.Infrasturcture.BackgroundServices;
using SmartBus.Infrasturcture.DependencyInjection;
using SmartBus.Infrasturcture.Hubs;
using SmartBus.Infrasturcture.Identity;
using SmartBus.Infrasturcture.Persistence;
using Swashbuckle.AspNetCore;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Text;

namespace SmartBus.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            // Add services to the container.
            builder.Services.AddDbContext<ApplicationContext>(option =>
             option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(option =>
            {
                option.SignIn.RequireConfirmedAccount = false;
                option.Password.RequireLowercase = false;
                option.Password.RequireUppercase = false;
                option.Password.RequireNonAlphanumeric = false;

            }).AddEntityFrameworkStores<ApplicationContext>()
           .AddDefaultTokenProviders();
            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            //builder.Services.AddOpenApi();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "AuthTest API",
                    Version = "v1"
                });

                // ?? Add JWT Authentication to Swagger
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter 'Bearer {your token}' in the text input below.\n\nExample: 'Bearer abc123xyz'"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                          Array.Empty<string>()
                     }
                });
                options.EnableAnnotations();
            });
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(jwt =>
            {
                var key = Encoding.ASCII.GetBytes(builder.Configuration["JWTConfig:Secert"]!);

                jwt.SaveToken = true;
                jwt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                };
            });
            builder.Services.AddSignalR();
            builder.Services.AddInfrastructure(builder.Configuration);
            builder.Services.AddApplicationMapping();
            builder.Services.AddAplicationServices();
            builder.Services.AddHostedService<TicketBackgroundServies>();
            QuestPDF.Settings.License = LicenseType.Community;



            builder.Services.AddHttpClient("GooglePlaces", client =>
            {
                client.BaseAddress = new Uri("https://nominatim.openstreetmap.org/search?");
            });
           
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();

            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();
            app.MapHub<NotificationHub>("/hubs/notification");
            app.Run();
        }
    }
}
