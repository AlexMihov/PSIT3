﻿using FirstFloor.ModernUI.Windows.Controls;
<<<<<<< Updated upstream
using Quizio.Pages.Dialogs;
using Quizio.ViewModels;
=======
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism;
using Quizio.Views.Dialogs;
>>>>>>> Stashed changes
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
<<<<<<< Updated upstream

        public MainWindow(MainViewModel mv)
        {
            InitializeComponent();
            this.DataContext = mv;
=======
        public MainWindow(MainViewModel mvm)
        {
            InitializeComponent();
            this.DataContext = mvm;
>>>>>>> Stashed changes
        }
    }
}
