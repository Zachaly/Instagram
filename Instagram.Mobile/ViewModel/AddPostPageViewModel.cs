using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Instagram.Mobile.Service;
using System.Collections.ObjectModel;

namespace Instagram.Mobile.ViewModel
{
    public partial class AddPostPageViewModel : ObservableObject
    {
        private readonly IPostService _postService;
        private readonly IAuthorizationService _authorizationService;

        public ObservableCollection<string> SelectedImages { get; set; } = 
            new ObservableCollection<string>();

        private List<string> _tags = new List<string>();

        public ObservableCollection<string> AddedTags { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<string> PhotoGallery { get; set; } = new ObservableCollection<string>();

        [ObservableProperty]
        private string _content = "";

        [ObservableProperty]
        private string _newTag = "";

        [ObservableProperty]
        private IDictionary<string, string[]>? _validationErrors = null;

        public AddPostPageViewModel(IPostService postService, IAuthorizationService authorizationService)
        {
            _postService = postService;
            _authorizationService = authorizationService;
        }

        [RelayCommand]
        private async Task SelectImageAsync()
        {
            var file = await MediaPicker.Default.PickPhotoAsync();

            if(file is null)
            {
                return;
            }

            SelectedImages.Add(file.FullPath);
        }

        [RelayCommand]
        private async Task AddTagAsync()
        {
            if (string.IsNullOrEmpty(NewTag))
            {
                await Toast.Make("Tag cannot be empty").Show();
                return;
            }

            if (_tags.Contains(NewTag))
            {
                await Toast.Make("Tag already added").Show();
                return;
            }

            _tags.Add(NewTag);

            AddedTags.Add($"#{NewTag}");

            NewTag = "";
        }

        [RelayCommand]
        private void DeleteTag(string tag)
        {
            _tags.Remove(tag.Replace("#", ""));
            AddedTags.Remove(tag);
        }

        [RelayCommand]
        private void DeleteImage(string image)
        {
            SelectedImages.Remove(image);
        }

        [RelayCommand]
        private async Task AddPostAsync()
        {
            if (!SelectedImages.Any())
            {
                await Toast.Make("You must add images!").Show();
                return;
            }

            try
            {
                await _postService.AddAsync(_authorizationService.UserData.UserId,
                    Content,
                    SelectedImages,
                    _tags);
            }
            catch(InvalidRequestException exception)
            {
                ValidationErrors = exception.Response.ValidationErrors;
                return;
            }
            
            await Toast.Make("Post added").Show();

            Content = "";
            SelectedImages.Clear();
            AddedTags.Clear();
            _tags.Clear();
            ValidationErrors = null;
        }
    }
}
