using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;


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
            Test test = ((Button)sender).Tag as Test;
            Console.WriteLine(test.Category.Image.Id.ToString());
           DlgTakeTest inputDialog = new DlgTakeTest(test);
           if (inputDialog.ShowDialog() == true) ;
            //BtnCreateDialog.Content = "Success!";
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (Globals.CurrentUser != null) { 
                List<Test> userTests = Globals.DbContextAutoGen.Tests.Where(x => x.OwnerId == Globals.CurrentUser.Id).ToList();
                IcUserTests.ItemsSource = userTests;
                foreach(Test test in userTests)
                {
                    Console.WriteLine(test.Category.Name);
                }
                List<Test> communityTests = Globals.DbContextAutoGen.Tests.Where(t => t.OwnerId != Globals.CurrentUser.Id).ToList();
                IcCommunityTests.ItemsSource = communityTests;
            }
            List<Test> userTests = Globals.DbContextAutoGen.Tests.Where(x => x.OwnerId == Globals.CurrentUser.Id).OrderBy(t => t.Id).ThenByDescending(t => t.Id).ToList();
            IcUserTests.ItemsSource = userTests;

            List<Test> communityTests = Globals.DbContextAutoGen.Tests.Where(t => t.OwnerId != Globals.CurrentUser.Id).OrderBy(t => t.Id).ThenByDescending(t => t.Id).ToList();
            IcCommunityTests.ItemsSource = communityTests;
            
        }
    }
}
