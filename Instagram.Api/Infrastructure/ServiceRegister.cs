﻿using FluentMigrator.Runner;
using Instagram.Application;
using Instagram.Application.Abstraction;
using Instagram.Application.Command;
using Instagram.Database.Factory;
using Instagram.Database.Migrations;
using Instagram.Database.Repository;
using Instagram.Database.Sql;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Instagram.Api.Infrastructure
{
    public static class ServiceRegister
    {
        public static void RegisterDatabase(this IServiceCollection services, IConfiguration config)
        {
            services.AddScoped<IMigrationManager, MigrationManager>();
            services.AddScoped<IConnectionFactory, ConnectionFactory>();
            services.AddScoped<ISqlQueryBuilder, SqlQueryBuilder>();

            services.AddScoped<IUserRepository, UserRepository>();

            services.AddFluentMigratorCore()
                .ConfigureRunner(c =>
                {
                    c.AddSqlServer2016();
                    c.WithGlobalConnectionString(config["ConnectionStrings:SqlConnection"]);
                    c.ScanIn(typeof(MigrationManager).Assembly).For.Migrations();
                });
        }

        public static void RegisterApplication(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();
            
            services.AddScoped<IUserFactory, UserFactory>();
            services.AddScoped<IResponseFactory, ResponseFactory>();

            services.AddMediatR(opt =>
            {
                opt.RegisterServicesFromAssemblyContaining<LoginCommand>();
            });
        }

        public static void ConfigureAuthorization(this WebApplicationBuilder builder)
        {
            builder.Services.AddAuthentication(config =>
            {
                config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(config =>
            {
                var bytes = Encoding.UTF8.GetBytes(builder.Configuration["Auth:SecretKey"]);
                var key = new SymmetricSecurityKey(bytes);

                config.SaveToken = true;
                config.RequireHttpsMetadata = false;
                config.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = key,
                    ValidIssuer = builder.Configuration["Auth:Issuer"],
                    ValidAudience = builder.Configuration["Auth:Audience"],
                };
            })
            .AddJwtBearer("Websocket", config =>
            {
                var bytes = Encoding.UTF8.GetBytes(builder.Configuration["Auth:SecretKey"]);
                var key = new SymmetricSecurityKey(bytes);

                config.SaveToken = true;
                config.RequireHttpsMetadata = false;
                config.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = key,
                    ValidIssuer = builder.Configuration["Auth:Issuer"],
                    ValidAudience = builder.Configuration["Auth:Audience"],
                };
                config.Events = new JwtBearerEvents
                {
                    OnMessageReceived = (context) =>
                    {
                        var path = context.HttpContext.Request.Path;
                        if (path.StartsWithSegments("/ws"))
                        {
                            var token = context.Request.Query["access_token"];

                            if (!string.IsNullOrWhiteSpace(token))
                            {
                                context.Token = token;
                            }
                        }

                        return Task.CompletedTask;
                    }
                };
            });
        }
    }
}
