﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Instagram.Mobile.View;
using Mopups.Services;
using System.Collections.ObjectModel;

namespace Instagram.Mobile.ViewModel
{
    public partial class UserListPopupViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<UserListItem> _users;

        public UserListPopupViewModel(IEnumerable<UserListItem> users)
        {
            _users = new ObservableCollection<UserListItem>(users);
        }

        public class UserListItem
        {
            public string UserName { get; set; }
            public long Id { get; set; }
            public string ImageUrl => $"{Configuration.ImageUrl}profile/{Id}";
        }

        [RelayCommand]
        private async Task GoToProfileAsync(UserListItem user)
        {
            await Shell.Current.GoToAsync(nameof(ProfilePage), new Dictionary<string, object>
            {
                { "UserId", user.Id }
            });

            await MopupService.Instance.PopAllAsync();
        }
    }
}
