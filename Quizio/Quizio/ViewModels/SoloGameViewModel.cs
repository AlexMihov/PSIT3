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

namespace Quizio.ViewModels
{
    public class SoloGameViewModel : BindableBase, IPageViewModel
    {
        private IPageViewModel _currentPageViewModel;

        public IPageViewModel CurrentPageViewModel
        {
            get { return this._currentPageViewModel; }
            set { SetProperty(ref this._currentPageViewModel, value); }
        }

        private Question currentQuestion;

        public int QuestionsRemaining { get; set; }

        public List<Answer> CurrentAnswers { get; set; }

        public string CurrentQuestion { get; set; }

        public Quiz Quiz { get; set; }

        public SoloGameViewModel(Quiz quiz)
        {
            this.Quiz = quiz;
            this.QuestionsRemaining = Quiz.Questions.Count;

            // Set starting page
            CurrentPageViewModel = new SoloGamePlayViewModel();
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
                CurrentPageViewModel = new SoloGameResultViewModel();
            }
        }

        private void getAnswersForQuestion()
        {

        }

        }
    }
