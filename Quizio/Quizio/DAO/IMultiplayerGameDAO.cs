using Quizio.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizio.DAO
{
    public interface IMultiplayerGameDAO
    {
        void saveChallengeGame(Game challengeGame, Player challengedFriend, string challengeText);
        void saveChallengeResponse(Challenge challenge);
    }
}
