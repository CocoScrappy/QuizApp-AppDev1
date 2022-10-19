using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xaml;

namespace QuizApp
{
    /// <summary>
    /// Interaction logic for DlgTakeTest.xaml
    /// </summary>
    public partial class DlgTakeTest : Window
    {
        private static Test Test { get; set; }
        private static List<TestQuestion> TestQuestions { get; set; }
        private static Attempt Attempt { get; set; }
        private static List<AttemptRespons> PlayerResponses { get; set; }
        private static int CurrentIndex { get; set; }
        private static int ListLenght { get; set; }
        private static int AnsweredCorrectly { get; set; }
        public static string QuestionNumber
        {
            get
            {
                return $"{CurrentIndex + 1}/{ListLenght}";
            }
        }
        public DlgTakeTest()
        {
            InitializeComponent();
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;

            


        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
            //find test entity
            ////FIXME:: Find for dynamic testId
            Test = Globals.DbContextAutoGen.Tests.Where(t => t.Id == 14).OfType<Test>().FirstOrDefault();
            if (Test == null)
            {
                MessageBox.Show(this, "Test could not be found", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            // populate questions list
            TestQuestions = Globals.DbContextAutoGen.TestQuestions.Where(q => q.TestId == 14).OfType<TestQuestion>().ToList();
            if (TestQuestions == null || TestQuestions.Count() == 0)
            {
                MessageBox.Show(this, "No questions were found for this Test", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            CurrentIndex = 0;
            AnsweredCorrectly = 0;
            ListLenght = TestQuestions.Count();
            PaintCurrentQuestion();
            PlayerResponses = new List<AttemptRespons>();
            // FOR TESTING REMOVE ME 
            Globals.CurrentUser = Globals.DbContextAutoGen.Users.Where(u => u.Id == 2).FirstOrDefault();
        }

        private void PaintCurrentQuestion()
        {
            //update question number
            LblQuestionNumber.Content = QuestionNumber;

            //Find out Question parameters
            TxblQuestion.Text = HttpUtility.HtmlDecode(TestQuestions[CurrentIndex].Question);
            //create answer list with Correct answer at index 0
            List<string> answers = new List<string> { TestQuestions[CurrentIndex].CorrectAnswer };

            //Split and add wrong answers to answers list.
            string[] wrongAnswers = TestQuestions[CurrentIndex].WrongAnswers.Split(';');
            foreach (string wrongAnswer in wrongAnswers)
            {
                answers.Add(wrongAnswer);
            }
            //reset button styles
            BeforeAnsweredControls();
            if(wrongAnswers.Length == 1)
            {// setting up for True / False Questions
                BtnAnswer1.Content = "True";
                BtnAnswer2.Content = "False";
                BtnAnswer3.Visibility = Visibility.Hidden;
                BtnAnswer4.Visibility = Visibility.Hidden;
            }
            else
            {
            //shuffle answer positions for multiple choice
            List<int> positions = new List<int> { 1, 2, 3, 0 };
            Random rand = new Random();
            List<int> shuffledPositions = positions.OrderBy(_ => rand.Next()).ToList();

            BtnAnswer1.Content = answers[shuffledPositions[0]];
            BtnAnswer2.Content = answers[shuffledPositions[1]];
            BtnAnswer3.Content = answers[shuffledPositions[2]];
            BtnAnswer4.Content = answers[shuffledPositions[3]];
            }

            
        }
        //Style when going to next question
        private void BeforeAnsweredControls()
        {
            BtnNext.IsEnabled = false;
            BtnNext.Visibility = Visibility.Hidden;

            BtnAnswer1.Background = default;
            BtnAnswer2.Background = default;
            BtnAnswer3.Background = default;
            BtnAnswer4.Background = default;

            BtnAnswer1.IsEnabled = true;
            BtnAnswer2.IsEnabled = true;
            BtnAnswer3.IsEnabled = true;
            BtnAnswer4.IsEnabled = true;

            BtnAnswer3.Visibility = Visibility.Visible;
            BtnAnswer4.Visibility = Visibility.Visible;
        }
        //style after answering question and waiting to go to next
        private void AfterAnsweredControls()
        {
            BtnAnswer1.IsEnabled = false;
            BtnAnswer2.IsEnabled = false;
            BtnAnswer3.IsEnabled = false;
            BtnAnswer4.IsEnabled = false;
            BtnNext.IsEnabled = true;
            BtnNext.Visibility = Visibility.Visible;
        }

        private void BtnAnswer1_Click(object sender, RoutedEventArgs e)
        {
            string answer = BtnAnswer1.Content.ToString();
            Boolean isCorrect = false;
            int testQuestionId = TestQuestions[CurrentIndex].Id;

            if (CheckAnswer(answer))
            {
                BtnAnswer1.Background = Brushes.Green;
                AnsweredCorrectly++;
                isCorrect = true;
            }
            else
            {
                BtnAnswer1.Background = Brushes.Red;
            }
            PlayerResponses.Add(new AttemptRespons() { Response = answer, TestQuestionId = testQuestionId, IsCorrect = Convert.ToInt16(isCorrect) });
            AfterAnsweredControls();

        }

        private void BtnAnswer2_Click(object sender, RoutedEventArgs e)
        {
            string answer = BtnAnswer2.Content.ToString();
            Boolean isCorrect = false;
            int testQuestionId = TestQuestions[CurrentIndex].Id;

            if (CheckAnswer(answer))
            {
                BtnAnswer2.Background = Brushes.Green;
                AnsweredCorrectly++;
                isCorrect = true;
            }
            else
            {
                BtnAnswer2.Background = Brushes.Red;
            }
            PlayerResponses.Add(new AttemptRespons() { Response = answer, TestQuestionId = testQuestionId, IsCorrect = Convert.ToInt16(isCorrect) });
            AfterAnsweredControls();
        }

        private void BtnAnswer3_Click(object sender, RoutedEventArgs e)
        {
            string answer = BtnAnswer3.Content.ToString();
            Boolean isCorrect = false;
            int testQuestionId = TestQuestions[CurrentIndex].Id;

            if (CheckAnswer(answer))
            {
                BtnAnswer3.Background = Brushes.Green;
                AnsweredCorrectly++;
                isCorrect = true;
            }
            else
            {
                BtnAnswer3.Background = Brushes.Red;
            }
            PlayerResponses.Add(new AttemptRespons() { Response = answer, TestQuestionId = testQuestionId, IsCorrect = Convert.ToInt16(isCorrect) });
            AfterAnsweredControls();
        }

        private void BtnAnswer4_Click(object sender, RoutedEventArgs e)
        {
            string answer = BtnAnswer4.Content.ToString();
            Boolean isCorrect = false;
            int testQuestionId = TestQuestions[CurrentIndex].Id;

            if (CheckAnswer(answer))
            {
                BtnAnswer4.Background = Brushes.Green;
                AnsweredCorrectly++;
                isCorrect = true;
            }
            else
            {
                BtnAnswer4.Background = Brushes.Red;
                isCorrect = false;
            }
            PlayerResponses.Add(new AttemptRespons() { Response = answer, TestQuestionId = testQuestionId, IsCorrect = Convert.ToInt16(isCorrect) });
            AfterAnsweredControls();
        }

        private Boolean CheckAnswer(string answer)
        {
            if(answer == TestQuestions[CurrentIndex].CorrectAnswer)
            {

                return true;
            }
            return false;
        }

        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            if(CurrentIndex < ListLenght-1)
            {
                CurrentIndex++;
                PaintCurrentQuestion();
            }
            else
            {
                SaveAttempt();
                BtnNext.Content = "Show Results";
                BtnNext.Width += 45;
                DlgTestResults testResults = new DlgTestResults(AnsweredCorrectly, ListLenght);
                if (testResults.ShowDialog() == false)
                {
                    BtnDone.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
                }

                
            }
        }

        private void SaveAttempt()
        {
            Attempt Attempt = new Attempt();

            Attempt.TestId = Test.Id;
            Attempt.PlayerId = Globals.CurrentUser.Id;
            Attempt.DateTaken = DateTime.Now;
            Attempt.Archived = 0;
            double result = ((double)AnsweredCorrectly/(double)ListLenght) * 100;
            Attempt.Result = (int)result;

            try
            {
                Globals.DbContextAutoGen.Attempts.Add(Attempt);
                Globals.DbContextAutoGen.SaveChanges();
                SavePlayerResponses(Attempt.Id);
                //increase score based on test difficulty
                UpdateUserScore();
            }
            catch (SystemException ex)
            {
                MessageBox.Show(this, "An error occured saving to the database:\n" + ex.Message + "\n" + ex.InnerException, "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SavePlayerResponses(int attemptId)
        {
            try
            {//Add attemptId to responses and save to Db
                foreach (AttemptRespons a in PlayerResponses)
                {
                    a.AttemptId = attemptId;
                    Globals.DbContextAutoGen.AttemptResponses.Add(a);
                    Globals.DbContextAutoGen.SaveChanges();
                }
            }
            catch (SystemException)
            {
                throw;
            }
        }

        private void UpdateUserScore()
        {
            if(Globals.CurrentUser.Score == null) Globals.CurrentUser.Score = 0;
            int increase = 0;
            switch (Test.Difficulty)
            {
                case "Easy":
                    increase = 1;
                    break;
                case "Medium":
                    increase = 2;
                    break;
                case "Hard":
                    increase = 3;
                    break;
            }
            Globals.CurrentUser.Score += increase;
            Globals.DbContextAutoGen.SaveChanges();
        }


    }
}
