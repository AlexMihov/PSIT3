using Microsoft.Practices.Prism.Mvvm;
using Quizio.DAO;
using Quizio.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizio.Aggregators
{
    public class GameResultAggregator : BindableBase
    {
        private Challenge _challenge;
        public Challenge Challenge
        {
            get { return this._challenge; }
            set { SetProperty(ref this._challenge, value);}
        }

        #region DAO Interfaces
        private IMultiplayerGameDAO gameDao;
        #endregion

        public GameResultAggregator()
        {
            this.gameDao = new MultiplayerGameDAO();
        }

        public GameResultAggregator(IMultiplayerGameDAO gameDao)
        {
            this.gameDao = gameDao;
        }

        public void loadSelectedChallenge()
        {
            Challenge = gameDao.getChallenge(Challenge.Id);
        }

        public List<Round> getCorrectRounds(List<Round> rounds)
        {
            List<Round> result = new List<Round>();
            var it = rounds.GetEnumerator();
            while (it.MoveNext())
            {
                var round = it.Current;
                if (round.isCorrect())
                {
                    result.Add(round);
                }
            }
            return result;
        }

        public List<Round> getFalseRounds(List<Round> rounds)
        {
            List<Round> result = new List<Round>();
            var it = rounds.GetEnumerator();
            while (it.MoveNext())
            {
                var round = it.Current;
                if (!round.isCorrect() && !round.isTimedOut())
                {
                    result.Add(round);
                }
            }
            return result;
        }

        public List<Round> getTimedOutRounds(List<Round> rounds)
        {
            List<Round> result = new List<Round>();
            var it = rounds.GetEnumerator();
            while (it.MoveNext())
            {
                var round = it.Current;
                if (round.isTimedOut())
                {
                    result.Add(round);
                }
            }
            return result;
        }
    }
}
