﻿using Instagram.Mobile.Service;
using Instagram.Mobile.View;
using Instagram.Mobile.ViewModel;

namespace Instagram.Mobile
{
    public static class BuilderExtensions
    {
        public static void AddServices(this MauiAppBuilder builder)
        {
            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<RegisterPage>();
            builder.Services.AddTransient<LoginPageViewModel>();
            builder.Services.AddTransient<RegisterPageViewModel>();
            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<MainPageViewModel>();
            builder.Services.AddTransient<ProfilePage>();
            builder.Services.AddTransient<ProfilePageViewModel>();

            builder.Services.AddSingleton<IAuthorizationService, AuthorizationService>();
            builder.Services.AddSingleton<IHttpClientFactory, HttpClientFactory>();

            builder.Services.AddSingleton<IUserService, UserService>();
        }
    }
}