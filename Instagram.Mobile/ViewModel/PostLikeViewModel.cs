using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Instagram.Mobile.View;
using Instagram.Models.PostLike;
using Mopups.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instagram.Mobile.ViewModel
{
    public partial class PostLikeViewModel : ObservableObject
    {
        [ObservableProperty]
        private PostLikeModel _like;

        public string UserImageUrl => $"{Configuration.ImageUrl}profile/{_like.UserId}";

        public PostLikeViewModel(PostLikeModel like)
        {
            _like = like;
        }

        [RelayCommand]
        private async Task GoToProfileAsync()
        {
            await Shell.Current.GoToAsync(nameof(ProfilePage), new Dictionary<string, object>
            {
                {"UserId", Like.UserId }
            });

            await MopupService.Instance.PopAllAsync();
        }
         
    }
}
