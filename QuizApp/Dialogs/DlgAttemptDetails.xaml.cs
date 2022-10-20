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
using System.Windows.Shapes;

namespace QuizApp.Dialogs
{
    /// <summary>
    /// Interaction logic for DlgAttemptDetails.xaml
    /// </summary>
    public partial class DlgAttemptDetails : Window
    {
        //Details displayed in the List View
        private class AttemptDetails
        {
            public int Id { get; set; }
            public string Question { get; set; }
            public string CorrectAnswer { get; set; }
            public string IncorrectAnswers { get; set; }
            public string PlayerAnswer { get; set; }
            public bool IsCorrect {get; set; }

        }
        public DlgAttemptDetails(Attempt attempt)
        {
            InitializeComponent();
            //WindowStartupLocation = WindowStartupLocation.CenterScreen;

            if (attempt == null)
            {
                MessageBox.Show(this, "An error occured when retrieving this attempt details", "Missing Record", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                //Extract relevant information from Selected Attempt Entity
                List<AttemptDetails> attemptDetails = new List<AttemptDetails>();
                for(int i = 0; i < attempt.Test.Amount; i++)
                {
                    AttemptDetails attemptDetail = new AttemptDetails();
                    attemptDetail.Id = i + 1;
                    attemptDetail.Question = attempt.Test.TestQuestions.ElementAt(i).Question;
                    attemptDetail.CorrectAnswer = attempt.Test.TestQuestions.ElementAt(i).CorrectAnswer;
                    attemptDetail.IncorrectAnswers = attempt.Test.TestQuestions.ElementAt(i).WrongAnswers.Replace(";", ", ");
                    attemptDetail.PlayerAnswer = attempt.AttemptResponses.ElementAt(i).Response;
                    attemptDetail.IsCorrect = Convert.ToBoolean(attempt.AttemptResponses.ElementAt(i).IsCorrect);
                    attemptDetails.Add(attemptDetail);
                }
                LvAttemptDetails.ItemsSource = attemptDetails;
            }
        }
    }
}
