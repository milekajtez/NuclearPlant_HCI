﻿using System;
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
using System.Windows.Shapes;

namespace PZ3_NetworkService.Views
{
    /// <summary>
    /// Interaction logic for NetworkView.xaml
    /// </summary>
    public partial class NetworkView : UserControl
    {
        public NetworkView()
        {
            this.DataContext = new PZ3_NetworkService.ViewModel.NetworkViewModel();
            InitializeComponent();
            
        }
    }
}
