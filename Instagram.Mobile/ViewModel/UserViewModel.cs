using CommunityToolkit.Mvvm.ComponentModel;
using Instagram.Models.User;

namespace Instagram.Mobile.ViewModel
{
    public partial class UserViewModel : ObservableObject
    {
        [ObservableProperty]
        private UserModel _user;

        public string ImageUrl => $"{Configuration.ImageUrl}profile/{User.Id}";
        
        public UserViewModel(UserModel user)
        {
            _user = user;
        }
    }
}
