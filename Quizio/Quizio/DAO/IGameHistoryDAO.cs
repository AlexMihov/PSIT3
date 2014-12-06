using Quizio.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizio.DAO
{
    public interface IGameHistoryDAO
    {
        List<Challenge> loadChallenges();
    }
}
