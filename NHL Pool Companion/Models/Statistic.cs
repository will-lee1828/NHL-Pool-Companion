
using System.ComponentModel.DataAnnotations;
namespace NHL_Pool_Companion.Models
{
    public class Statistic
    {
        [Key]
        public int StatisticsID { get; set; }
        public int GamesPlayed { get; set; }
        public int Goals { get; set; }
        public int Assits { get; set; }
        public int Points { get; set; }
        public int PlusMinus { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
        public int OverTimeLosses { get; set; }
        public int ShutOut { get; set; }

        public int PlayerID { get; set; }
        public virtual Player Player { get; set; }
    }
}