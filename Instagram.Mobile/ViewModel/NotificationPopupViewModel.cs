using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Instagram.Mobile.Service;
using Instagram.Models.Notification;
using Instagram.Models.Notification.Request;
using System.Collections.ObjectModel;

namespace Instagram.Mobile.ViewModel
{
    public partial class NotificationViewModel : ObservableObject
    {
        [ObservableProperty]
        private NotificationModel _notification;

        [ObservableProperty]
        private bool _isRead = false;

        public string Created => DateTimeOffset.FromUnixTimeMilliseconds(Notification.Created).ToString("dd.MM.yyyy HH:mm");

        public NotificationViewModel(NotificationModel notification)
        {
            _notification = notification;
            _isRead = notification.IsRead;
        }
    }

    public partial class NotificationPopupViewModel : ObservableObject
    {
        public ObservableCollection<NotificationViewModel> Notifications { get; set; }
            = new ObservableCollection<NotificationViewModel>();

        private readonly INotificationService _notificationService;

        public NotificationPopupViewModel(IEnumerable<NotificationModel> notifications, INotificationService notificationService)
        {
            _notificationService = notificationService;
            
            foreach(var notification in notifications)
            {
                Notifications.Add(new NotificationViewModel(notification));
            }
        }

        [RelayCommand]
        private async Task ReadNotificationAsync(NotificationViewModel notification)
        {
            notification.IsRead = true;

            await _notificationService.UpdateAsync(new UpdateNotificationRequest
            {
                Id = notification.Notification.Id,
                IsRead = true
            });
        }

        [RelayCommand]
        private async Task DeleteNotificationAsync(NotificationViewModel notification)
        {
            Notifications.Remove(notification);

            await _notificationService.DeleteByIdAsync(notification.Notification.Id);
        }

        [RelayCommand]
        private async Task CloseAsync()
        {
            await PopupService.CloseAsync();
        }
    }
}
