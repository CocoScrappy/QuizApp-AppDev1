// using Newtonsoft.Json;
// using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
using System.Xml.Linq;

namespace QuizApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        HttpClient client = new HttpClient();

        public MainWindow()
        {
            InitializeComponent();
        }
        /*private void Button_Click(object sender, RoutedEventArgs e)
        {
            DlgCreateTest inputDialog = new DlgCreateTest();
            if (inputDialog.ShowDialog() == true)
                BtnCreateDialog.Content = "Success!";
        }*/
    }

}
