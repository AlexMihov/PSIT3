using Quizio.Models;
using System;
using System.Collections.Generic;

namespace Quizio.DAO
{
    public interface IRankingDAO
    {
        List<Ranking> loadRankings();
        void updateRanking(Player fromUser, int pointsToAdd);
    }
}
