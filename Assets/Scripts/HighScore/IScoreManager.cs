using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.HighScore
{
    interface IScoreManager
    {
        void SaveHighScore(Enum minigameType, int totalScore, int playerId);
        //void GetHighScore();
    }
}
