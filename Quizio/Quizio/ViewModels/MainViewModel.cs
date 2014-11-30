using Microsoft.Practices.Prism.Mvvm;
using Quizio.Models;

namespace Quizio.ViewModels
{
    public class MainViewModel : BindableBase
    {
        #region hosted ViewModels for integrated Views
        public RegularGameViewModel RegularGameViewModel { get; set; }
        public ProfileViewModel ProfileViewModel { get; set; }
        public RankingViewModel RankingViewModel { get; set; }
        public HomeViewModel HomeViewModel { get; set; }
        public FriendViewModel FriendViewModel { get; set; }
        #endregion

        public MainViewModel(ModelAggregator aggregator)
        {
            this.RegularGameViewModel = new RegularGameViewModel(aggregator);
            this.ProfileViewModel = new ProfileViewModel(aggregator);
            this.RankingViewModel = new RankingViewModel(aggregator);
            this.HomeViewModel = new HomeViewModel(aggregator);
            this.FriendViewModel = new FriendViewModel(aggregator);
        }
    }
}