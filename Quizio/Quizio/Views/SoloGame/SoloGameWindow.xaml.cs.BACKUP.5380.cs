using FirstFloor.ModernUI.Windows.Controls;
using Quizio.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Quizio.Views.SoloGame
{
    /// <summary>
    /// Interaction logic for SoloGameWindow.xaml
    /// </summary>
    public partial class SoloGameWindow : ModernWindow
    {
        public SoloGameWindow(SoloGameViewModel vm)
        {
            InitializeComponent();
            this.DataContext = vm;
            this.Closing += vm.OnWindowClosing;
        }
    }
}
