using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Instagram.Mobile.Service;
using Instagram.Mobile.View;
using Instagram.Models.Notification.Request;

namespace Instagram.Mobile.ViewModel
{
    public partial class ShellViewModel : ObservableObject
    {
        public bool IsAuthorized => _authorizationService.IsAuthorized;

        [ObservableProperty]
        private int _notificationCount = 0;

        private readonly IAuthorizationService _authorizationService;
        private readonly INotificationService _notificationService;

        public ShellViewModel(IAuthorizationService authorizationService, INotificationService notificationService) 
        {
            _authorizationService = authorizationService;
            _notificationService = notificationService;
        }

        [RelayCommand]
        private async Task LogoutAsync()
        {
            await _authorizationService.LogoutAsync();
            await NavigationService.GoToPageAsync<LoginPage>();
        }

        [RelayCommand]
        private void UpdateAuthorizedStatus() => OnPropertyChanged(nameof(IsAuthorized));

        [RelayCommand]
        private async Task GetNotificationCountAsync()
        {
            if(!_authorizationService.IsAuthorized)
            {
                return;
            }

            NotificationCount = await _notificationService.GetCountAsync(new GetNotificationRequest
            {
                UserId = _authorizationService.UserData.UserId,
                IsRead = false
            });
        }

        [RelayCommand]
        private async Task ShowNotificationsAsync()
        {
            NotificationCount = 0;

            var notifications = await _notificationService.GetAsync(new GetNotificationRequest
            {
                UserId = _authorizationService.UserData.UserId,
            });

            await PopupService.ShowNotificationPopupAsync(notifications, _notificationService);
        }
    }
}
