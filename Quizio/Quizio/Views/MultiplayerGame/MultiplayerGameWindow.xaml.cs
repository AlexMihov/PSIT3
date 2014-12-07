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

namespace Quizio.Views.MultiplayerGame
{
    /// <summary>
    /// Interaction logic for MultiplayerGameWindow.xaml
    /// </summary>
    public partial class MultiplayerGameWindow : ModernWindow
    {
        public MultiplayerGameWindow(SoloGameViewModel vm)
        {
            InitializeComponent();
            this.DataContext = vm;
            this.Closing += vm.OnWindowClosing;
        }
    }
}
