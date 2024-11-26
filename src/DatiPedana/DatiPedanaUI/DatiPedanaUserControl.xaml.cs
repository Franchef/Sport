using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
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

namespace DatiPedanaUI
{
    /// <summary>
    /// Interaction logic for DatiPedanaUserControl.xaml
    /// </summary>
    public partial class DatiPedanaUserControl : UserControl
    {
        public DatiPedanaUserControl(DatiPedanaViewModel? viewModel = null)
        {
            viewModel = viewModel ?? new DatiPedanaViewModel();
            InitializeComponent();
        }

        public DatiPedanaViewModel ViewModel { get; init; } = new DatiPedanaViewModel();
    }
}
