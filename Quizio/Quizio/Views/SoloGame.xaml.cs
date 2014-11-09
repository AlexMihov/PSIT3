﻿using Quizio.Models;
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

namespace Quizio.Views
{
    /// <summary>
    /// Interaction logic for SoloGame.xaml
    /// </summary>
    public partial class SoloGame : UserControl
    {
        public string Difficulty {get; set;}
        public string Category {get; set;}

        public SoloGame(string category, string difficulty)
        {
            InitializeComponent();
            this.Category = "dini mum";
            this.Difficulty = difficulty;
        }
    }
}