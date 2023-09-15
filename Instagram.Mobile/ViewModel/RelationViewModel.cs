using CommunityToolkit.Mvvm.ComponentModel;
using Instagram.Models.Relation;

namespace Instagram.Mobile.ViewModel
{
    public partial class RelationViewModel : ObservableObject
    {
        [ObservableProperty]
        private RelationModel _relation;

        public string FirstImageUrl => $"{Configuration.ImageUrl}relation/{Relation.ImageIds.First()}";

        public RelationViewModel(RelationModel relation)
        {
            _relation = relation;
        }
    }
}
