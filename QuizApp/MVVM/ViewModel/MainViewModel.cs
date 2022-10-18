using QuizApp.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizApp.MVVM.ViewModel
{
    class MainViewModel : ObservableObject
    {
        public HomeQuizViewModel HomeQuizVM { get; set; }

        private object _currentView;

        public object CurrentView
        {
            get { return _currentView; }
            set { 
                _currentView = value; 
                OnPropertyChanged();
            }
        }


        public MainViewModel()
        {
            HomeQuizVM = new HomeQuizViewModel();
            CurrentView = HomeQuizVM;      
        }
    }
}
