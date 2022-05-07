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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PrimalEditor.GameProject
{
    /// <summary>
    /// Interaction logic for OpenProjectVieew.xaml
    /// </summary>
    public partial class OpenProjectView : UserControl
    {
        public OpenProjectView()
        {
            InitializeComponent();
        }

        private void OnOpen_Button_Click(object sender, RoutedEventArgs e)
        {
            OpenSelectedProject();
        }

        private void OnListBoxItem_Mouse_DoubleClick(object sender, RoutedEventArgs e)
        {
            OpenSelectedProject();
        }
        private void OpenSelectedProject()
        {

            var project = OpenProject.Open(ProjectsListBox.SelectedItem as ProjectData);
            bool bDialogResult = false;
            var win = Window.GetWindow(this);
            if (project != null)
            {
                bDialogResult = true;
            }
            win.DialogResult = bDialogResult;
            win.Close();
        }
    }
}
