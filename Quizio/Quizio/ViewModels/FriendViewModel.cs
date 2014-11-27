using Microsoft.Practices.Prism.Mvvm;
using Quizio.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizio.ViewModels
{
    public class FriendViewModel : BindableBase
    {
        public ModelAggregator Aggregator { get; set; }

        public FriendViewModel(ModelAggregator aggregator)
        {
            this.Aggregator = aggregator;
        }
    }
}
