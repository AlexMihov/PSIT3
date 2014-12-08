using FirstFloor.ModernUI.Windows.Controls;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Quizio.Aggregators;
using Quizio.Models;
using Quizio.Views.HomeViews.ResultView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Quizio.ViewModels
{
    public class GameResultViewModel : BindableBase
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

        public int ChallengerPoints { get; set; }
        public int ResponsePlayerPoints { get; set; }

        public List<Round> CorrectChallengerInputs { get; set; }
        public List<Round> FalseChallengerInputs { get; set; }
        public List<Round> TimedOutChallengerInputs { get; set; }
        public List<Round> CorrectResponsePlayerInputs { get; set; }
        public List<Round> FalseResponsePlayerInputs { get; set; }
        public List<Round> TimedOutResponsePlayerInputs { get; set; }

        public GameResultAggregator ResultDataAggregator { get; set; }

        public ICommand CloseWindow { get; set; }

        private static int ANSWERTIME = 10;

        public GameResultViewModel(GameResultAggregator resultDataAggregator)
        {
            ResultDataAggregator = resultDataAggregator;
            ContentControlView = new GameResultTabControl();
            prepareViewDataFormat();
            calculatePoints();
            this.CloseWindow = new DelegateCommand<object>(closeResultViewWindow);
        }

        private void closeResultViewWindow(object param)
        {
            ModernWindow wnd = param as ModernWindow;
            wnd.Close();
        }

        private void prepareViewDataFormat()
        {
            List<Round> challangerRounds = ResultDataAggregator.Challenge.ChallengeGame.Rounds;
            List<Round> responsePlayerRounds = ResultDataAggregator.Challenge.ResponseGame.Rounds;

            CorrectChallengerInputs = ResultDataAggregator.getCorrectRounds(challangerRounds);
            CorrectResponsePlayerInputs = ResultDataAggregator.getCorrectRounds(responsePlayerRounds);

            FalseChallengerInputs = ResultDataAggregator.getFalseRounds(challangerRounds);
            FalseResponsePlayerInputs = ResultDataAggregator.getFalseRounds(responsePlayerRounds);

            TimedOutChallengerInputs = ResultDataAggregator.getTimedOutRounds(challangerRounds);
            TimedOutResponsePlayerInputs = ResultDataAggregator.getTimedOutRounds(responsePlayerRounds);
        }

        private void calculatePoints()
        {
            ResponsePlayerPoints = CorrectResponsePlayerInputs.Count * ((ResultDataAggregator.Challenge.ChallengeGame.Rounds.Count * ANSWERTIME) - ResultDataAggregator.Challenge.ResponseGame.Time);
            ChallengerPoints = CorrectChallengerInputs.Count * ((ResultDataAggregator.Challenge.ChallengeGame.Rounds.Count * ANSWERTIME) - ResultDataAggregator.Challenge.ChallengeGame.Time);
        }
    }
}
