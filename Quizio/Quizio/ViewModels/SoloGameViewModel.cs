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
using System.Windows;
using Quizio.Views.SoloGame;

namespace Quizio.ViewModels
{
    public class SoloGameViewModel : BindableBase
    {
        private FrameworkElement _contentControlView;
        public FrameworkElement ContentControlView
        {
            get { return _contentControlView; }
            set
            {
                SetProperty(ref this._contentControlView, value);
            }
        }

        public ICommand NextQuestion { get; private set; }

        public Quiz Quiz { get; private set; }

        private Question _currentQuestion;
        public Question CurrentQuestion
        {
            get { return this._currentQuestion; }
            set
            {
                SetProperty(ref this._currentQuestion, value);
            }
        }

        private int _questionsRemaining;
        public int QuestionsRemaining
        {
            get { return this._questionsRemaining; }
            set
            {
                SetProperty(ref this._questionsRemaining, value);
            }
        }

        private int _questionsDone;
        public int QuestionsDone
        {
            get { return this._questionsDone; }
            set
            {
                SetProperty(ref this._questionsDone, value);
            }
        }

        public int CorrectAnswers { get; private set; }
        public List<Question> WrongQuestions { get; private set; }
        public List<string> UserInput { get; private set; }

        public SoloGameViewModel(Quiz quiz)
        {
            this.Quiz = quiz;
            QuestionDAO dao = new QuestionDAO();
            this.Quiz.Questions = dao.loadQuestionsOfQuiz(quiz.Id);
            this.QuestionsDone = 1;
            this.WrongQuestions = new List<Question>();
            this.UserInput = new List<string>();
            this.CorrectAnswers = 0;

            this.NextQuestion = new DelegateCommand<object>(this.GetNextQuestion);

            QuestionsRemaining = Quiz.Questions.Count;

            getRandomQuestion();

            SwitchView("Play");
        }

        private void getRandomQuestion()
        {
            if (this.QuestionsDone == this.QuestionsRemaining)
            {
                SwitchView("Result");
            }
            else
            {
                this.CurrentQuestion = Quiz.getRandomQuestion();
                Quiz.Questions.Remove(this.CurrentQuestion);
            }
        }

        private void GetNextQuestion(object parameter)
        {
            string answerText = (string)parameter;

            if (!CurrentQuestion.checkAnswer(answerText))
            {
                this.WrongQuestions.Add(CurrentQuestion);
                this.UserInput.Add(answerText);
            }
            else
            {
                this.CorrectAnswers++;
            }
            this.QuestionsDone++;
            getRandomQuestion();
        }

        public void SwitchView(string viewName)
        {
            switch (viewName)
            {
                case "Play":
                    ContentControlView = new SoloGamePlay();
                    break;

                case "Result":
                    ContentControlView = new SoloGameResult();
                    break;
            }
        }

        internal void OnWindowClosing(object sender, CancelEventArgs e)
        {
            App.Current.MainWindow.Show();
        }
    }
}
