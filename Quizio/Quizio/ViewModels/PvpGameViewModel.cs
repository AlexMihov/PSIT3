using Quizio.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizio.ViewModels
{
    public class PvpGameViewModel
    {
        public ModelAggregator Aggregator { get; set; }

        public PvpGameViewModel(ModelAggregator aggregator)
        {
            this.Aggregator = aggregator;
        }
    }
}
