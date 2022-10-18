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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace QuizApp.MVVM.View
{
    /// <summary>
    /// Interaction logic for HomeQuizView.xaml
    /// </summary>
    public partial class HomeQuizView : UserControl
    {
        public HomeQuizView()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DlgTakeTest inputDialog = new DlgTakeTest();
            if (inputDialog.ShowDialog() == true) ;
            //BtnCreateDialog.Content = "Success!";
        }
    }
}
