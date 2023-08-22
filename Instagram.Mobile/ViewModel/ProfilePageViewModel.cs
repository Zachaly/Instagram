using CommunityToolkit.Mvvm.ComponentModel;
using Instagram.Models.User;

namespace Instagram.Mobile.ViewModel
{
    [QueryProperty(nameof(UserId), "UserId")]
    public partial class ProfilePageViewModel : ObservableObject
    {
        [ObservableProperty]
        private long _userId;

        [ObservableProperty]
        private UserModel _userModel = new UserModel 
        { 
            Id = 1,
            Bio = "bio",
            Name = "Name",
            Nickname = "NickName",
            Verified = false,
        };

        [ObservableProperty]
        private int _followersCount = 1;

        [ObservableProperty]
        private int _followingCount = 2;

        [ObservableProperty]
        private int _postCount = 3;

        public string ImageUrl => $"{Configuration.ApiUrl}image/profile/{UserModel.Id}";
    }
}
