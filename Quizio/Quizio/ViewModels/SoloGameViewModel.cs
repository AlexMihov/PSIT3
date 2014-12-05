using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Prism.Commands;
using Quizio.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using System.Windows;
using Quizio.Views.SoloGame;
using System.Windows.Threading;
using FirstFloor.ModernUI.Windows.Controls;

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
        public GameAggregator Game { get; set; }

        public List<UserInput> CorrectUserInputs { get; set; }
        public List<UserInput> FalseUserInputs { get; set; }
        public List<UserInput> TimedOutUserInputs { get; set; }

        private List<int> timeNeeded;
        public int TimeNeededSum { get; set; }

        public ICommand NextQuestion { get; private set; }
        public ICommand CloseAndSave { get; private set; }

        private BackgroundWorker bw;
        private Window gameWindow;
        private DispatcherTimer myTimer;
        private static int ANSWERTIME = 10;
        #endregion

        public SoloGameViewModel(GameAggregator game)
        {
            this.Game = game;
            this.gameWindow = null;
            
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

            bw = new BackgroundWorker();
            bw.DoWork += bw_DoWork;
            bw.RunWorkerCompleted += bw_RunWorkerCompleted;
            
            getRandomQuestion();

            SwitchView("Play");
            myTimer.Start();
        }

        #region worker functions
        private void bw_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                int pointsToAdd = CorrectUserInputs.Count * ((QuestionsRemaining * ANSWERTIME) - TimeNeededSum);
                Game.updateRanking(pointsToAdd);
            }
            catch (Exception ex)
            {
                e.Result = ex.Message; // abusing e.Result as exception messanger
            }
        }

        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Result != null)
            {
                ModernDialog.ShowMessage("Deine Punkte konnten leider nicht hochgeladen werden.\n" + 
                    e.Result + "\n\nVersuche es bitte erneut oder schliesse das Fenster manuell.",
                    "Fehler", MessageBoxButton.OK);
            }
            else if(gameWindow != null)
            {
                gameWindow.Close();
            }
            else
            {
                ModernDialog.ShowMessage("Interner Fehler, bitte schliesse das Spiel manuell. Deine Punkte wurden aber hochgeladen!", "Fehler", MessageBoxButton.OK);
            }
        }
        #endregion

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
                this.FalseUserInputs.Add(new UserInput(CurrentQuestion,
                    CurrentQuestion.GetAnswerByText(answerText).Id));
            }
            else
            {
                Answer ans = CurrentQuestion.GetCorrectAnswer();
                this.CorrectUserInputs.Add(new UserInput(CurrentQuestion, ans.Id));
            }

            myTimer.Stop();
            timeNeeded.Add(TimerTickCount);

            this.QuestionsDone++;
            getRandomQuestion();
        }

        private void SaveAndClose(object parameter)
        {
            if (parameter is System.Windows.Window)
            {
                this.gameWindow = parameter as System.Windows.Window;
            }
            if (!bw.IsBusy)
            {
                bw.RunWorkerAsync();
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
                UserInput timedOutInput = new UserInput(CurrentQuestion, timedOutAnswer.Id);
                TimedOutUserInputs.Add(timedOutInput);

                this.QuestionsDone++;
                getRandomQuestion();
            }
            TimerTickCountDown = ANSWERTIME - TimerTickCount;
        }
        #endregion
    }
}