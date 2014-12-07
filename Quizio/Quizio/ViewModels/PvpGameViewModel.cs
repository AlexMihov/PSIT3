using Quizio.Models;
using Quizio.Views.SoloGame;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizio.ViewModels
{
    public class PvpGameViewModel : RegularGameViewModel
    {
        private Friend _friendToChallange;
        public Friend FriendToChallange
        {
            get { return this._friendToChallange; }
            set { SetProperty(ref this._friendToChallange, value); }
        }

        private string _challangeText;
        public string ChallangeText
        {
            get { return this._challangeText; }
            set { SetProperty(ref this._challangeText, value); }
        }

        public PvpGameViewModel(ModelAggregator aggregator) : base(aggregator)
        {
        }
    }
}
