using Microsoft.Practices.Prism.Mvvm;
using Quizio.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizio.ViewModels
{
    public class HomeViewModel : BindableBase
    {
        public ModelAggregator Aggregator { get; set; }

        public HomeViewModel(ModelAggregator aggregator)
        {
            this.Aggregator = aggregator;
        }
    }
}
