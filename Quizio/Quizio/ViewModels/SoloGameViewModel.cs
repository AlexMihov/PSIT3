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
using FirstFloor.ModernUI.Windows.Controls;
using System.Windows.Threading;

namespace Quizio.ViewModels
{
    public class SoloGameViewModel : BindableBase
    {
        #region Datafields with Raising Events
        private FrameworkElement _contentControlView;
        public FrameworkElement ContentControlView
        {
            get { return _contentControlView; }
            set
            {
                SetProperty(ref this._contentControlView, value);
            }
        }

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

        private int _timerTickCount;
        public int TimerTickCount
        {
            get { return this._timerTickCount; }
            set
            {
                SetProperty(ref this._timerTickCount, value);
            }
        }

        private int _timerTickCountDown;
        public int TimerTickCountDown
        {
            get { return this._timerTickCountDown; }
            set
            {
                SetProperty(ref this._timerTickCountDown, value);
            }
        }
        #endregion

        #region Shared Datafields without raising events
        public Game Game { get; set; }

        public List<UserInput> CorrectUserInputs { get; set; }
        public List<UserInput> FalseUserInputs { get; set; }
        public List<UserInput> TimedOutUserInputs { get; set; }

        private List<int> timeNeeded;
        public int TimeNeededSum { get; set; }

        public ICommand NextQuestion { get; private set; }
        public ICommand CloseAndSave { get; private set; }

        private DispatcherTimer myTimer;
        private static int ANSWERTIME = 10;
        #endregion

        public SoloGameViewModel(Game game)
        {
            this.Game = game;
            
            QuestionsDone = 1;
            QuestionsRemaining = Game.Quiz.Questions.Count;

            CorrectUserInputs = new List<UserInput>();
            FalseUserInputs = new List<UserInput>();
            TimedOutUserInputs = new List<UserInput>();
            timeNeeded = new List<int>();

            NextQuestion = new DelegateCommand<object>(this.GetNextQuestion);
            CloseAndSave = new DelegateCommand<object>(this.SaveAndClose);

            TimerTickCount = 0;
            TimerTickCountDown = ANSWERTIME;
            myTimer = new DispatcherTimer();
            myTimer.Interval = new TimeSpan(0, 0, 1);
            myTimer.Tick += new EventHandler(Timer_Tick);
            
            getRandomQuestion();

            SwitchView("Play");
            myTimer.Start();
        }

        #region VM private functions
        private void getRandomQuestion()
        {
            if (this.QuestionsDone == this.QuestionsRemaining+1)
            {
                TimeNeededSum = timeNeeded.Sum();
                SwitchView("Result");
            }
            else
            {
                this.CurrentQuestion = Game.Quiz.getRandomQuestion();
                Game.Quiz.Questions.Remove(this.CurrentQuestion);
                TimerTickCount = 0;
                TimerTickCountDown = ANSWERTIME;
                myTimer.Interval = new TimeSpan(0, 0, 1);
                myTimer.Start();
            }
        }

        private void SwitchView(string viewName)
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
        #endregion

        #region Command-Functions for Buttons
        private void GetNextQuestion(object parameter)
        {
            string answerText = (string)parameter;

            if (!CurrentQuestion.checkAnswer(answerText))
            {
                this.FalseUserInputs.Add(new UserInput(CurrentQuestion.QuestionString,
                    CurrentQuestion.GetAnswerByText(answerText), CurrentQuestion.GetCorrectAnswer()));
            }
            else
            {
                Answer ans = CurrentQuestion.GetCorrectAnswer();
                this.CorrectUserInputs.Add(new UserInput(CurrentQuestion.QuestionString, ans, ans));
            }

            myTimer.Stop();
            timeNeeded.Add(TimerTickCount);

            this.QuestionsDone++;
            getRandomQuestion();
        }

        private void SaveAndClose(object parameter)
        {
            int pointsToAdd = CorrectUserInputs.Count * ((QuestionsRemaining * ANSWERTIME) - TimeNeededSum);

            Game.updateRanking(pointsToAdd);

            if (parameter is System.Windows.Window)
            {
                (parameter as System.Windows.Window).Close();
            }
        }
        #endregion

        #region EventHandlers for VM
        internal void OnWindowClosing(object sender, CancelEventArgs e)
        {
            App.Current.MainWindow.Show();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            DispatcherTimer timer = (DispatcherTimer)sender;
            if (++TimerTickCount == ANSWERTIME)
            {
                timer.Stop();

                timeNeeded.Add(TimerTickCount);

                Answer timedOutAnswer = new Answer("Timeout", false);
                Answer ans = CurrentQuestion.GetCorrectAnswer();
                UserInput timedOutInput = new UserInput(CurrentQuestion.QuestionString, timedOutAnswer, ans);
                TimedOutUserInputs.Add(timedOutInput);

                this.QuestionsDone++;
                getRandomQuestion();
            }
            TimerTickCountDown = ANSWERTIME - TimerTickCount;
        }
        #endregion
    }
}