using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Instagram.Mobile.Service;
using Instagram.Mobile.View;
using System.Collections.ObjectModel;

namespace Instagram.Mobile.ViewModel
{
    public partial class AddRelationPageViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _name = "";

        public ObservableCollection<string> SelectedImages { get; } = new ObservableCollection<string>();

        [ObservableProperty]
        private IDictionary<string, string[]> _validationErrors = null;

        private readonly IRelationService _relationService;
        private readonly IAuthorizationService _authorizationService;

        public AddRelationPageViewModel(IRelationService relationService, IAuthorizationService authorizationService)
        {
            _relationService = relationService;
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
        private void RemoveImage(string image)
        {
            SelectedImages.Remove(image);
        }

        [RelayCommand]
        private async Task AddAsync()
        {
            try
            {
                await _relationService.AddAsync(_authorizationService.UserData.UserId, Name, SelectedImages);

                await Shell.Current.GoToAsync(nameof(ProfilePage));
            }
            catch(InvalidRequestException exception)
            {
                ValidationErrors = exception.Response.ValidationErrors;
            }
        }
    }
}
