﻿using System;
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
    /// Interaction logic for NewProjectView.xaml
    /// </summary>
    public partial class NewProjectView : UserControl
    {
        public NewProjectView()
        {
            InitializeComponent();
        }

        private void OnCreate_Button_Click(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as NewProject;
            var projectPath = vm.CreateProject(TemplateListBox.SelectedItem as ProjectTemplate);
            bool bDialogResult = false;
            var win = Window.GetWindow(this);
            if(!string.IsNullOrEmpty(projectPath))
            {
                bDialogResult = true;
            }
            win.DialogResult = bDialogResult;
            win.Close();
        }
    }
}
