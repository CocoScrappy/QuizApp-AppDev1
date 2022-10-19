using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace QuizApp
{
    /// <summary>
    /// Interaction logic for DlgCreateTest.xaml
    /// </summary>
    public partial class DlgCreateTest : Window
    {
        public DlgCreateTest()
        {
            InitializeComponent();
        }
         private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            List<Test> tests = Globals.DbContextAutoGen.Tests.ToList();
            List<TestQuestion> testQuestions = Globals.DbContextAutoGen.TestQuestions.ToList();
            List<Category> categories = Globals.DbContextAutoGen.Categories.ToList();
            ComboCategory.ItemsSource = categories;
            ComboCategory.SelectedIndex = 24;
           // ComboCategory.SelectedItem == null ComboCategory.Text = "Any Category";
        }

        private void BtnCreateTest_Click(object sender, RoutedEventArgs e)
        {
            CreateQuery();
            this.DialogResult = true;
        }

        // Create Query String from Form inputs
        private async void CreateQuery()
        {
            Test test = new Test();
            //Validate amount of questions
            if (TbxAmount.Text == "")
            {
                MessageBox.Show(this, "Amount must be greater than 0", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Error);
            } 
            int.TryParse(TbxAmount.Text, out int amount);
            if (amount <= 0)
            {
                MessageBox.Show(this, "Amount must be greater than 0", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            string strAmount = $"amount={amount}";
            test.Amount = amount;
            //Validate category
            string strCategory = "";
            int category = 0;
            if (ComboCategory.SelectedItem != null && ComboCategory.SelectedIndex != 24)
            {
                category = ComboCategory.SelectedIndex + 9;
                strCategory = $"&category={category}";
            }
            test.CategoryId = ComboCategory.SelectedIndex + 1;
            //Validate Difficulty
            if (ComboDifficulty.SelectedItem == null)
            {
                MessageBox.Show(this, "Difficulty cannot be null", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            string difficulty = "";
            switch (ComboDifficulty.Text)
            {
                case "Easy":
                    difficulty = "&difficulty=easy";
                    break;
                case "Medium":
                    difficulty = "&difficulty=medium";
                    break;
                case "Hard":
                    difficulty = "&difficulty=hard";
                    break;
            }
            test.Difficulty = ComboDifficulty.Text;
            //Validate Type
            string type = "";
            if (ComboType.SelectedItem == null) return;
            if (ComboType.Text == "Multiple Choice") type = "&type=multiple";
            if (ComboType.Text == "True / False") type = "&type=boolean";
            test.Type = ComboType.Text;
            string query = $"https://opentdb.com/api.php?" + strAmount + strCategory + difficulty + type;
            Console.WriteLine(query);
            int testId = 0;
            test.OwnerId = Globals.CurrentUser.Id;
            try
            {
                //SaveTest returns the new Test Id so we can use it in TestQuestions
               testId =  SaveTest(test);
                Console.WriteLine(testId);
                await RequestTest(query, testId);
            }
            catch (SystemException ex)
            {
                MessageBox.Show(this, "An error occured saving to the database:\n" + ex.Message+"\n" + ex.InnerException , "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }

        private static int SaveTest(Test test)
        {


            try
            {
                //Add(test) returns the completed test object with it's new Id
                Globals.DbContextAutoGen.Tests.Add(test);
                Globals.DbContextAutoGen.SaveChanges();
                return test.Id;
            }
            catch (SystemException)
            {

                throw;
            }
        }

        private static async Task RequestTest(string query, int testId)
        {
            //testId 0 == no test id sent back from Db
            if (testId == 0)
            {
                MessageBox.Show("An error occured saving Test to database", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

                HttpClient client = Globals.Client;

            string response = await client.GetStringAsync(
                $"{query}");
            //parse string to Json Object
            JObject results = JObject.Parse(response);
            //Extract category json tokens from Json response object
            IList<JToken> questions = results["results"].Children().ToList();

                foreach (JToken q in questions)
                {

                 
                    TempTestQuestion question = new TempTestQuestion();
                    //deserialize Json to Category Entity Objects
                    question = q.ToObject<TempTestQuestion>();

                    string wrong = "";
                    for (int i = 0; i < question.Incorrect_Answers.Length; i++)
                    {
                        if (wrong == "") wrong = question.Incorrect_Answers[i];
                        else wrong += $";{question.Incorrect_Answers[i]}";
                    }
                    //Transfer Temp info to DB entities

                    //Get Test Id back for Test Questions fields
                    TestQuestion dbQuestion = new TestQuestion();
                    dbQuestion.TestId = testId;
                    dbQuestion.Question = HttpUtility.HtmlDecode(question.Question);
                    dbQuestion.CorrectAnswer = HttpUtility.HtmlDecode(question.Correct_Answer);
                    dbQuestion.WrongAnswers = HttpUtility.HtmlDecode(wrong);
                    
                try { 
                    //Add and Save to database
                    Globals.DbContextAutoGen.TestQuestions.Add(dbQuestion);
                    Globals.DbContextAutoGen.SaveChanges();
                }
                catch(SystemException)
                {
                        throw;
                }
            }
        }

        private class TempTestQuestion
        {
            public string Category { get; set; }
            public string Type { get; set; }
            public string Difficulty { get; set; }
            public string Question { get; set; }
            public string Correct_Answer { get; set; }
            public string[] Incorrect_Answers { get; set; }
        }
    }
}
