using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Instagram.Mobile.Service;
using Instagram.Models.PostComment;

namespace Instagram.Mobile.ViewModel
{
    public partial class PostCommentViewModel : ObservableObject
    {
        [ObservableProperty]
        private PostCommentModel _comment;

        public PostCommentViewModel(PostCommentModel comment)
        {
            _comment = comment;
        }

        [RelayCommand]
        private async Task GoToProfilePageAsync()
            => await NavigationService.GoToProfilePageAsync(Comment.UserId);
    }
}
