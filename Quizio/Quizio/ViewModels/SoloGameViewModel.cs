using FirstFloor.ModernUI.Presentation;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Commands;
using Quizio.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Quizio.Utilities;

namespace Quizio.ViewModels
{
    public class SoloGameViewModel : BindableBase, IPageViewModel
    {
        private ICommand _changePageCommand;
        private IPageViewModel _currentPageViewModel;
        private List<IPageViewModel> _pageViewModels;

        private Question currentQuestion;

        public int QuestionsRemaining { get; set; }

        public List<Answer> CurrentAnswers { get; set; }

        public string CurrentQuestion { get; set; }

        public Quiz Quiz { get; set; }

        public List<Question> Questions { get; set; }

        public SoloGameViewModel(Quiz quiz)
        {
            this.Quiz = quiz;
            QuestionDAO dao = new QuestionDAO();
            Questions = dao.loadQuestionsOfQuiz(Quiz.Id);



            this.QuestionsRemaining = Quiz.Questions.Count;

            // Add available pages
            PageViewModels.Add(new SoloGamePlayViewModel());
            PageViewModels.Add(new SoloGameResultViewModel());

            // Set starting page
            CurrentPageViewModel = PageViewModels[0];
        }

        internal void OnWindowClosing(object sender, CancelEventArgs e)
        {
            App.Current.MainWindow.Show();
        }

        private void getRandomQuestion()
        {
            this.currentQuestion = Quiz.getRandomQuestion();
            Quiz.Questions.Remove(this.currentQuestion);
            this.QuestionsRemaining = Quiz.Questions.Count;
            if (this.QuestionsRemaining == 0)
            {
                CurrentPageViewModel = PageViewModels[1];
            }
        }

        private void getAnswersForQuestion()
        {

        }
 
        #region Properties / Commands
 
        public ICommand ChangePageCommand
        {
            get
            {
                if (_changePageCommand == null)
                {
                    _changePageCommand = new RelayCommand(
                        p => ChangeViewModel((IPageViewModel)p),
                        p => p is IPageViewModel);
                }
 
                return _changePageCommand;
            }
        }
 
        public List<IPageViewModel> PageViewModels
        {
            get
            {
                if (_pageViewModels == null)
                    _pageViewModels = new List<IPageViewModel>();
 
                return _pageViewModels;
            }
        }
 
        public IPageViewModel CurrentPageViewModel
        {
            get
            {
                return _currentPageViewModel;
            }
            set
            {
                SetProperty(ref _currentPageViewModel, value);
            }
        }
 
        #endregion
 
        #region Methods
 
        private void ChangeViewModel(IPageViewModel viewModel)
        {
            if (!PageViewModels.Contains(viewModel))
                PageViewModels.Add(viewModel);
 
            CurrentPageViewModel = PageViewModels
                .FirstOrDefault(vm => vm == viewModel);
        }
 
        #endregion

        }
    }
