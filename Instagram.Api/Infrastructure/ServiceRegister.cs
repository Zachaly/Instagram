﻿using FluentMigrator.Runner;
using FluentValidation;
using Instagram.Api.Authorization;
using Instagram.Api.Infrastructure.NotificationCommands;
using Instagram.Api.Infrastructure.Service;
using Instagram.Api.Infrastructure.ServiceProxy;
using Instagram.Application;
using Instagram.Application.Abstraction;
using Instagram.Application.Auth.Abstraction;
using Instagram.Application.Command;
using Instagram.Application.UserStory;
using Instagram.Application.Validation;
using Instagram.Database.Factory;
using Instagram.Database.Migrations;
using Instagram.Database.Repository;
using Instagram.Database.Sql;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
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
            services.AddScoped<IPostTagRepository, PostTagRepository>();
            services.AddScoped<IUserClaimRepository, UserClaimRepository>();
            services.AddScoped<IPostReportRepository, PostReportRepository>();
            services.AddScoped<IUserBanRepository, UserBanRepository>();
            services.AddScoped<IRelationRepository, RelationRepository>();
            services.AddScoped<IRelationImageRepository, RelationImageRepository>();
            services.AddScoped<IDirectMessageRepository, DirectMessageRepository>();
            services.AddScoped<IAccountVerificationRepository, AccountVerificationRepository>();
            services.AddScoped<INotificationRepository, NotificationRepository>();
            services.AddScoped<IUserStoryImageRepository, UserStoryImageRepository>();
            services.AddScoped<IUserBlockRepository, UserBlockRepository>();

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
            services.AddScoped<IPostTagService, PostTagService>();
            services.AddScoped<IUserClaimService, UserClaimService>();
            services.AddScoped<IPostReportService, PostReportService>();
            services.AddScoped<IUserBanService, UserBanService>();
            services.AddScoped<IUserDataService, UserDataService>();
            services.AddScoped<IRelationService, RelationService>();
            services.AddScoped<IDirectMessageService, DirectMessageService>();
            services.AddScoped<IAccountVerificationService, AccountVerificationService>();
            services.AddScoped<INotificationFactory, NotificationFactory>();
            services.AddScoped<IUserStoryService, UserStoryService>();
            services.AddScoped<IUserBlockService, UserBlockService>();

            services.AddScoped<IUserFactory, UserFactory>();
            services.AddScoped<IResponseFactory, ResponseFactory>();
            services.AddScoped<IPostFactory, PostFactory>();
            services.AddScoped<IUserFollowFactory, UserFollowFactory>();
            services.AddScoped<IPostLikeFactory, PostLikeFactory>();
            services.AddScoped<IPostCommentFactory, PostCommentFactory>();
            services.AddScoped<IPostTagFactory, PostTagFactory>();
            services.AddScoped<IUserClaimFactory, UserClaimFactory>();
            services.AddScoped<IPostReportFactory, PostReportFactory>();
            services.AddScoped<IUserBanFactory, UserBanFactory>();
            services.AddScoped<IRelationFactory, RelationFactory>();
            services.AddScoped<IDirectMessageFactory, DirectMessageFactory>();
            services.AddScoped<IAccountVerificationFactory, AccountVerificationFactory>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IUserStoryFactory, UserStoryFactory>();
            services.AddScoped<IUserBlockFactory, UserBlockFactory>();

            services.AddValidatorsFromAssemblyContaining<RegisterCommandValidator>();           

            services.AddMediatR(opt =>
            {
                opt.RegisterServicesFromAssemblyContaining<LoginCommand>();
                opt.RegisterServicesFromAssemblyContaining<SendFollowNotificationCommand>();
            });
        }
        
        public static void RegisterProxies(this IServiceCollection services)
        {
            services.AddScoped<IPostServiceProxy, PostServiceProxy>();
            services.AddScoped<IPostCommentServiceProxy, PostCommentServiceProxy>();
            services.AddScoped<IPostLikeServiceProxy, PostLikeServiceProxy>();
            services.AddScoped<IUserServiceProxy, UserServiceProxy>();
            services.AddScoped<IUserFollowServiceProxy, UserFollowServiceProxy>();
            services.AddScoped<IPostTagServiceProxy, PostTagServiceProxy>();
            services.AddScoped<IUserClaimServiceProxy, UserClaimServiceProxy>();
            services.AddScoped<IPostReportServiceProxy, PostReportServiceProxy>();
            services.AddScoped<IUserBanServiceProxy, UserBanServiceProxy>();
            services.AddScoped<IRelationServiceProxy, RelationServiceProxy>();
            services.AddScoped<IDirectMessageServiceProxy, DirectMessageServiceProxy>();
            services.AddScoped<IAccountVerificationServiceProxy, AccountVerificationServiceProxy>();
            services.AddScoped<INotificationServiceProxy, NotificationServiceProxy>();
            services.AddScoped<IUserStoryServiceProxy, UserStoryServiceProxy>();
            services.AddScoped<IUserBlockServiceProxy, UserBlockServiceProxy>();

            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingPipeline<,>));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationPipeline<,>));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoginCommandPipeline<,>));
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
            }).AddJwtBearer(AuthPolicyNames.WebSocketScheme, config =>
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

            builder.Services.AddAuthorization(config =>
            {
                config.AddPolicy(AuthPolicyNames.Admin, c => c.RequireClaim("Role", UserClaimValues.Admin));
                config.AddPolicy(AuthPolicyNames.Moderator, c => c.RequireClaim("Role", UserClaimValues.Moderator, UserClaimValues.Admin));
                config.AddPolicy(AuthPolicyNames.NotBanned, c => c.AddRequirements(new NotBannedRequirement()));
            });

            builder.Services.AddSingleton<IAuthorizationHandler, NotBannedHandler>();
        }
    }
}
