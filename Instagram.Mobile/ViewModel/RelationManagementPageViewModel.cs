using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Instagram.Mobile.Service;
using Instagram.Mobile.View;
using Instagram.Models.Relation.Request;
using System.Collections.ObjectModel;

namespace Instagram.Mobile.ViewModel
{
    public class RelationImageViewModel
    {
        public string Url => $"{Configuration.ImageUrl}relation/{Id}";
        public long Id { get; set; }

        public RelationImageViewModel(long id)
        {
            Id = id;
        }
    }

    public partial class RelationManagementPageViewModel : ObservableObject
    {
        public ObservableCollection<RelationViewModel> Relations { get; } = new ObservableCollection<RelationViewModel>();

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(HasSelectedRelation))]
        [NotifyPropertyChangedFor(nameof(SelectedRelationImages))]
        private RelationViewModel _selectedRelation = null;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(HasSelectedImage))]
        private string _selectedImage = null;

        public IEnumerable<RelationImageViewModel> SelectedRelationImages => SelectedRelation?.Relation.ImageIds
            .Select(id => new RelationImageViewModel(id));

        public bool HasSelectedRelation => SelectedRelation is not null;
        public bool HasSelectedImage => SelectedImage is not null;

        private readonly IRelationService _relationService;
        private readonly IAuthorizationService _authorizationService;
        private readonly IRelationImageService _relationImageService;

        public RelationManagementPageViewModel(IRelationService relationService, IAuthorizationService authorizationService,
            IRelationImageService relationImageService)
        {
            _relationService = relationService;
            _authorizationService = authorizationService;
            _relationImageService = relationImageService;
        }

        [RelayCommand]
        private async Task LoadRelationAsync()
        {
            Relations.Clear();

            var relations = await _relationService.GetAsync(new GetRelationRequest { UserId = _authorizationService.UserData.UserId });

            foreach(var relation in relations) 
            {
                Relations.Add(new RelationViewModel(relation));
            }
        }

        [RelayCommand]
        private void SelectRelation(RelationViewModel relationViewModel)
        {
            SelectedRelation = relationViewModel;
        }

        [RelayCommand]
        private async Task DeleteImageAsync(long imageId)
        {
            await _relationImageService.DeleteAsync(imageId);
        }

        [RelayCommand]
        private async Task SelectImageAsync()
        {
            var file = await MediaPicker.Default.PickPhotoAsync();

            if(file is not null)
            {
                SelectedImage = file.FullPath;
            }
        }

        [RelayCommand]
        private async Task AddImageAsync()
        {
            await _relationImageService.AddAsync(SelectedRelation.Relation.Id, SelectedImage);

            SelectedImage = null;

            await Toast.Make("Image added!").Show();
        }

        [RelayCommand]
        private async Task GoToAddRelationPageAsync()
        {
            await Shell.Current.GoToAsync(nameof(AddRelationPage));
        }

        [RelayCommand]
        private async Task DeleteRelationAsync(RelationViewModel relation)
        {
            await _relationService.DeleteByIdAsync(relation.Relation.Id);

            Relations.Remove(relation);

            if(relation == SelectedRelation)
            {
                SelectedRelation = null;
                SelectedImage = null;
            }
        }
    }
}
