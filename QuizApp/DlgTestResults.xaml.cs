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

namespace QuizApp
{
    /// <summary>
    /// Interaction logic for DlgTestResults.xaml
    /// </summary>
    public partial class DlgTestResults : Window
    {
        public DlgTestResults(int correctAnswers, int numberOfQuestions)
        {
            InitializeComponent();
            TxblFinalScore.Text = $"Your final score is:\n {correctAnswers} Out of {numberOfQuestions} questions!";

        }

        
    }
}
