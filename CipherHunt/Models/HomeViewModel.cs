using Repository.Common;
using System.Collections.Generic;

namespace CipherHunt.Models
{
    public class HomeViewModel
    {
        public List<TblProduct> ComboMeals { get; set; }
        public List<TblProduct> HomeMenu { get; set; }
    }
    public class ScoreBoardViewModel
    {
        public List<ScoreBoard> Scores { get; set; }
        public List<ScoreBoard> TeamScores { get; set; }
    }
}