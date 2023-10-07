using Instagram.Mobile.View;
using Instagram.Mobile.ViewModel;
using Instagram.Models.Notification;
using Instagram.Models.User;
using Mopups.Services;

namespace Instagram.Mobile.Service
{
    public static class PopupService
    {
        public static async Task ShowUserListPopupAsync(IEnumerable<UserListPopupViewModel.UserListItem> users)
            => await MopupService.Instance.PushAsync(new UserListPopup(new UserListPopupViewModel(users)));
        

        public static async Task ShowUserListPopupAsync(IEnumerable<UserModel> users)
            => await ShowUserListPopupAsync(users.Select(u => new UserListPopupViewModel.UserListItem
            {
                Id = u.Id,
                UserName = u.Nickname
            }));

        public static async Task ShowUserListPopupAsync(IEnumerable<UserViewModel> users)
            => await ShowUserListPopupAsync(users.Select(u => u.User));

        public static async Task ShowUserStoriesPopup(IEnumerable<UserStoryViewModel> stories, int startingIndex)
            => await MopupService.Instance.PushAsync(new UserStoryPopup(new UserStoryPopupViewModel(stories, startingIndex)));

        public static async Task ShowRelationsPopup(IEnumerable<RelationViewModel> relations, int startingIndex)
            => await MopupService.Instance.PushAsync(new RelationPopup(new RelationPopupViewModel(relations, startingIndex)));

        public static async Task ShowNotificationPopupAsync(IEnumerable<NotificationModel> notifications, INotificationService notificationService)
            => await MopupService.Instance.PushAsync(new NotificationPopup(new NotificationPopupViewModel(notifications, notificationService)));

        public static async Task CloseAsync()
            => await MopupService.Instance.PopAllAsync();
    }
}
