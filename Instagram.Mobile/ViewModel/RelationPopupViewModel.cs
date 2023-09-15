using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mopups.Services;

namespace Instagram.Mobile.ViewModel
{
    public partial class RelationPopupViewModel : ObservableObject
    {
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(CurrentRelation))]
        [NotifyPropertyChangedFor(nameof(CurrentImageUrl))]
        private int _currentRelationIndex = 0;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(CurrentImageUrl))]
        private int _currentImageIndex = 0;

        private IEnumerable<RelationViewModel> _relations;

        public RelationViewModel CurrentRelation => _relations.ElementAt(CurrentRelationIndex);

        public string CurrentImageUrl 
            => $"{Configuration.ImageUrl}relation/{CurrentRelation.Relation.ImageIds.ElementAt(CurrentImageIndex)}";

        public RelationPopupViewModel(IEnumerable<RelationViewModel> relations, int startIndex)
        {
            _relations = relations;
            CurrentRelationIndex = startIndex;
        }

        [RelayCommand]
        private async Task ChangeImage(int count)
        {
            if(CurrentImageIndex + count < 0)
            {
                if(CurrentRelationIndex - 1 < 0)
                {
                    CurrentRelationIndex--;
                    CurrentImageIndex = 0;
                }

                return;
            }

            if(CurrentImageIndex + count >= CurrentRelation.Relation.ImageIds.Count())
            {
                if(CurrentRelationIndex + 1 < _relations.Count())
                {
                    CurrentImageIndex = 0;
                    CurrentRelationIndex++;
                } 
                else
                {
                    await MopupService.Instance.PopAllAsync();
                }

                return;
            }

            CurrentImageIndex++;
        }
    }
}
