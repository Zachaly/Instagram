﻿using FluentMigrator.Runner;
using Instagram.Api.Infrastructure.ServiceProxy;
using Instagram.Application;
using Instagram.Application.Abstraction;
using Instagram.Application.Command;
using Instagram.Database.Factory;
using Instagram.Database.Migrations;
using Instagram.Database.Repository;
using Instagram.Database.Sql;
using MediatR;
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
            services.AddScoped<IPostRepository, PostRepository>();
            services.AddScoped<IPostImageRepository, PostImageRepository>();
            services.AddScoped<IUserFollowRepository, UserFollowRepository>();
            services.AddScoped<IPostLikeRepository, PostLikeRepository>();
            services.AddScoped<IPostCommentRepository, PostCommentRepository>();

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
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IPostService, PostService>();
            services.AddScoped<IUserFollowService, UserFollowService>();
            services.AddScoped<IPostLikeService, PostLikeService>();
            services.AddScoped<IPostCommentService, PostCommentService>();

            services.AddScoped<IUserFactory, UserFactory>();
            services.AddScoped<IResponseFactory, ResponseFactory>();
            services.AddScoped<IPostFactory, PostFactory>();
            services.AddScoped<IUserFollowFactory, UserFollowFactory>();
            services.AddScoped<IPostLikeFactory, PostLikeFactory>();
            services.AddScoped<IPostCommentFactory, PostCommentFactory>();

            services.AddMediatR(opt =>
            {
                opt.RegisterServicesFromAssemblyContaining<LoginCommand>();
            });
        }
        
        public static void RegisterProxies(this IServiceCollection services)
        {
            services.AddScoped<IPostServiceProxy, PostServiceProxy>();
            services.AddScoped<IPostCommentServiceProxy, PostCommentServiceProxy>();
            services.AddScoped<IPostLikeServiceProxy, PostLikeServiceProxy>();
            services.AddScoped<IUserServiceProxy, UserServiceProxy>();
            services.AddScoped<IUserFollowServiceProxy, UserFollowServiceProxy>();

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingPipeline<,>));
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
            });            
        }
    }
}
