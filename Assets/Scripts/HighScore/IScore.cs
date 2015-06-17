using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.HighScore
{
    interface IScore
    {
        int AnswerScoreWeigth { get; set; }
        int TimeScoreWeigth { get; set; }
        int TotalAskedQuestions { get; set; }
        int TotalCorrectQuestions { get; set; }
        int CalculateScore();
        void SaveScore(int totalScore, int totalTimeSeconds);
    }
}
