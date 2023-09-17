using CommunityToolkit.Maui.Alerts;

namespace Instagram.Mobile.Service
{
    public static class ToastService
    {
        public static Task MakeToast(string message)
            => Toast.Make(message).Show();
    }
}
