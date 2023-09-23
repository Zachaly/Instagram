using Instagram.Mobile.View;

namespace Instagram.Mobile.Service
{
    public static class NavigationService
    {
        public static Task GoToProfilePageAsync(long userId)
            => Shell.Current.GoToAsync(nameof(ProfilePage), new Dictionary<string, object>
            {
                { "UserId", userId }
            });

        public static Task GoToProfilePageAsync()
            => Shell.Current.GoToAsync(nameof(ProfilePage));

        public static Task GoToPageAsync<TPage>()
            => Shell.Current.GoToAsync(nameof(TPage));

        public static Task GoToPostPageAsync(long postId)
            => Shell.Current.GoToAsync(nameof(PostPage), new Dictionary<string, object>
            {
                { "PostId", postId }
            });

        public static Task GoToChatPageAsync(long userId)
            => Shell.Current.GoToAsync(nameof(ChatPage), new Dictionary<string, object>
            {
                { "UserId", userId }
            });

        public static Task GoBackAsync()
            => Shell.Current.GoToAsync("..");
    }
}
