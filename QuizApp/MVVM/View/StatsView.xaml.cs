using QuizApp.Dialogs;
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

namespace QuizApp.MVVM.View
{
    /// <summary>
    /// Interaction logic for StatsView.xaml
    /// </summary>
    public partial class StatsView : UserControl
    {
        private static List<Attempt> UserAttemps { get; set; }

        public StatsView()
        {
            InitializeComponent();
            //Display all attempts for current user                         FIX MY ID
            UserAttemps = Globals.DbContextAutoGen.Attempts.Where(a => a.PlayerId == 2).ToList();
            LvAttempts.ItemsSource = UserAttemps = UserAttemps.ToList();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void MiDetails_Click(object sender, RoutedEventArgs e)
        {
            //Feed selected Attempt to the Attempt Details Dialog Constructor
            Attempt selectedAttempt = LvAttempts.SelectedItem as Attempt;
            if (selectedAttempt == null) return;
            DlgAttemptDetails dlgAttemptDetails = new DlgAttemptDetails(selectedAttempt);
            if (dlgAttemptDetails.ShowDialog() == true) ;
        }
    }
}
