using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PrimalEditor.GameProject
{
    /// <summary>
    /// Interaction logic for ProjectBrowserDialog.xaml
    /// </summary>
    public partial class ProjectBrowserDialog : Window
    {
        public ProjectBrowserDialog()
        {
            InitializeComponent();
        }

        private void OnToggleButton_Click(object sender, RoutedEventArgs e)
        {
            if(sender == OpenProjectButton)
            {
                if(CreateProjectButton.IsChecked==true) // so we were in create mode
                {
                    CreateProjectButton.IsChecked = false;
                    BrowserContent.Margin = new Thickness(0);
                }
                OpenProjectButton.IsChecked = true;//nothing should happen we should say in mode
            }
            else
            {
                if (OpenProjectButton.IsChecked == true) // so we were in create mode
                {
                    OpenProjectButton.IsChecked = false;
                    BrowserContent.Margin = new Thickness(-800,0,0,0);
                }
                CreateProjectButton.IsChecked = true;//nothing should happen we should say in mode
            }
        }
    }
}
