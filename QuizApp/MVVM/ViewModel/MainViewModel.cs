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
        public RelayCommand HomeQuizViewCommand { get; set; }
        public RelayCommand StatsViewCommand { get; set; }
        public RelayCommand NewQuizViewCommand { get; set; }

        public HomeQuizViewModel HomeQuizVM { get; set; }
        public StatsViewModel StatsVM { get; set; }
        public NewQuizViewModel NewQuizVM { get; set; }

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
            StatsVM = new StatsViewModel();
            CurrentView = HomeQuizVM;
            HomeQuizViewCommand = new RelayCommand(o =>
            {
                CurrentView = HomeQuizVM;

            });
            StatsViewCommand = new RelayCommand(o =>
            {
                CurrentView = StatsVM;

            });
            NewQuizViewCommand = new RelayCommand(o =>
            {
                CurrentView = NewQuizVM;

            });
        }
    }
}
