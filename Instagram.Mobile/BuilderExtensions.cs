using Instagram.Mobile.Service;
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
            builder.Services.AddTransient<AppShell>();
            builder.Services.AddTransient<ShellViewModel>();
            builder.Services.AddTransient<PostPage>();
            builder.Services.AddTransient<PostPageViewModel>();
            builder.Services.AddTransient<SearchPage>();
            builder.Services.AddTransient<SearchPageViewModel>();
            builder.Services.AddTransient<AddPostPage>();
            builder.Services.AddTransient<AddPostPageViewModel>();
            builder.Services.AddTransient<UpdateProfilePage>();
            builder.Services.AddTransient<UpdateProfilePageViewModel>();
            builder.Services.AddTransient<AddStoryPage>();
            builder.Services.AddTransient<AddStoryPageViewModel>();
            builder.Services.AddTransient<AddRelationPage>();
            builder.Services.AddTransient<AddRelationPageViewModel>();
            builder.Services.AddTransient<RelationManagementPage>();
            builder.Services.AddTransient<RelationManagementPageViewModel>();
            builder.Services.AddTransient<App>();

            builder.Services.AddSingleton<IAuthorizationService, AuthorizationService>();
            builder.Services.AddSingleton<IHttpClientFactory, HttpClientFactory>();

            builder.Services.AddSingleton<IUserService, UserService>();
            builder.Services.AddSingleton<IUserFollowService, UserFollowService>();
            builder.Services.AddSingleton<IPostService, PostService>();
            builder.Services.AddSingleton<IPostCommentService, PostCommentService>();
            builder.Services.AddSingleton<IPostLikeService, PostLikeService>();
            builder.Services.AddSingleton<IUserStoryService, UserStoryService>();
            builder.Services.AddSingleton<IRelationService, RelationService>();
            builder.Services.AddSingleton<IRelationImageService, RelationImageService>();
        }
    }
}
