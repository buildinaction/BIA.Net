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
using Microsoft.VisualStudio.PlatformUI;

namespace BIA.CRUDScaffolder.UI
{
    /// <summary>
    /// Interaction logic for SelectModelWindow.xaml
    /// </summary>
    public partial class SelectModelWindow : DialogWindow
    {
        public SelectModelWindow(CustomViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;
            this.Topmost = true;
            this.HasMaximizeButton = false;
            this.HasMinimizeButton = false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void GenerateListAjax_Click(object sender, RoutedEventArgs e)
        {
            if (chkGenerateListAjax.IsChecked == true)
                chkGenerateListFullAjax.IsChecked = false;
        }

        private void GenerateListFullAjax_Click(object sender, RoutedEventArgs e)
        {
            if (chkGenerateListFullAjax.IsChecked == true)
                chkGenerateListAjax.IsChecked = false;
        }
    }
}
