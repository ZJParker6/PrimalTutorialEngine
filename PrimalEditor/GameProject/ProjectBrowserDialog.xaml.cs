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
            Loaded += OnProjectBrowserDialogLoaded;
        }

        private void OnProjectBrowserDialogLoaded(object sender, RoutedEventArgs e)
        {
            Loaded -= OnProjectBrowserDialogLoaded;
            if(!OpenProject.Projects.Any())
            {
                openProjectButton.IsEnabled = false;
                openProjectView.Visibility = Visibility.Hidden;
                OnToggleButton_Click(createProjectButton, new RoutedEventArgs());
            }
        }

        private void OnToggleButton_Click(object sender, RoutedEventArgs e)
        {
            if(sender == openProjectButton)
            {
                if(createProjectButton.IsChecked==true) // so we were in create mode
                {
                    createProjectButton.IsChecked = false;
                    BrowserContent.Margin = new Thickness(0);
                }
                openProjectButton.IsChecked = true;//nothing should happen we should say in mode
            }
            else
            {
                if (openProjectButton.IsChecked == true) // so we were in create mode
                {
                    openProjectButton.IsChecked = false;
                    BrowserContent.Margin = new Thickness(-800,0,0,0);
                }
                createProjectButton.IsChecked = true;//nothing should happen we should say in mode
            }
        }
    }
}
