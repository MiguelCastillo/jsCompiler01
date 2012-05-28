using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SoftGPL.vs10.UI
{
    /// <summary>
    /// Interaction logic for Options.xaml
    /// </summary>
    public partial class Options : UserControl
    {
        public Options(ViewModel.MainViewModel viewModel)
        {
            this.DataContext = viewModel;
            InitializeComponent();
        }

        private void TextBox_IntOnly_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            int result;
            e.Handled = int.TryParse(e.Text, out result) == false;
        }
    }
}