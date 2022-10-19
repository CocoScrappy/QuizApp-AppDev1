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
    /// Interaction logic for NewQuizView.xaml
    /// </summary>
    public partial class NewQuizView : UserControl
    {
        public NewQuizView()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DlgCreateTest createDialog = new DlgCreateTest();
            if (createDialog.ShowDialog() == true) ;
        }
    }
}
