using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Instagram.Mobile.Service;
using Instagram.Mobile.View;
using Instagram.Models.User.Request;
using System.Collections.ObjectModel;

namespace Instagram.Mobile.ViewModel
{
    public partial class SearchPageViewModel : ObservableObject
    {
        public ObservableCollection<UserViewModel> Users { get; set; } = new ObservableCollection<UserViewModel>();

        [ObservableProperty]
        private string _userName;

        private readonly IUserService _userService;

        public SearchPageViewModel(IUserService userService)
        {
            _userService = userService;
        }

        [RelayCommand]
        private async Task SearchUsersAsync()
        {
            Users.Clear();
            if(UserName.Length < 3)
            {
                return;
            }

            var users = await _userService.GetAsync(new GetUserRequest
            {
                SearchNickname = UserName
            });

            foreach(var user in users) 
            { 
                Users.Add(new UserViewModel(user));
            }
        }

        [RelayCommand]
        private async Task GoToProfilePageAsync(long id)
            => await Shell.Current.GoToAsync(nameof(ProfilePage), new Dictionary<string, object>
            {
                { "UserId", id }
            });
    }
}
