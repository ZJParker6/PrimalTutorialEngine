﻿using PrimalEditor.GameProject;
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

namespace PrimalEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += OnMainWindowLoaded;
        }

        private void OnMainWindowLoaded(object sender, RoutedEventArgs e)
        {
            Loaded -= OnMainWindowLoaded; // window only loaded once, this should not happen multiple times
            OpenProjectBrowserDialog();
        }

        private void OpenProjectBrowserDialog()
        {
            var ProjectBrowser = new ProjectBrowserDialog();
            if(ProjectBrowser.ShowDialog()==false)
            {
                Application.Current.Shutdown();
            }
            else
            {

            }
        }
    }
}
