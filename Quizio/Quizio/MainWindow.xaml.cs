using FirstFloor.ModernUI.Windows.Controls;
<<<<<<< HEAD
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism;
using Quizio.Views.Dialogs;
=======
<<<<<<< Updated upstream
using Quizio.Pages.Dialogs;
using Quizio.ViewModels;
=======
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism;
using Quizio.Views.Dialogs;
>>>>>>> Stashed changes
>>>>>>> 925372401c3fb132665a76daad52bb52bc22e7b1
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
using Quizio.ViewModels;

namespace Quizio
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ModernWindow, IView
    {
<<<<<<< HEAD
=======
<<<<<<< Updated upstream

        public MainWindow(MainViewModel mv)
        {
            InitializeComponent();
            this.DataContext = mv;
=======
>>>>>>> 925372401c3fb132665a76daad52bb52bc22e7b1
        public MainWindow(MainViewModel mvm)
        {
            InitializeComponent();
            this.DataContext = mvm;
<<<<<<< HEAD
=======
>>>>>>> Stashed changes
>>>>>>> 925372401c3fb132665a76daad52bb52bc22e7b1
        }
    }
}
