using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NHL_Pool_Companion.Models
{   
    public class Player
    {
        public static readonly Dictionary<string, string> Teams = new Dictionary<string, string>()
        {
            { "ANA","Anaheim Ducks"},
            { "ARI","Arizona Coyotes"},
            { "BOS","Boston Bruins"},
            { "BUF","Buffalo Sabres"},
            { "CGY","Calgary Flames"},
            { "CAR","Carolina Hurricanes"},
            { "CHI","Chicago Blackhawks"},
            { "COL","Colorado Avalanche"},
            { "CLB","Columbus Blue Jackets"},
            { "DAL","Dallas Stars"},
            { "DET","Detroit Red Wings"},
            { "EDM","Edmonton Oilers"},
            { "FLA","Florida Panthers"},
            { "LAK","Los Angeles Kings"},
            { "MIN","Minnesota Wild"},
            { "MTL","Montreal Canadiens"},
            { "NAS","Nashville Predators"},
            { "NJD","New Jersey Devils"},
            { "NYI","New York Islanders"},
            { "NYR","New York Rangers"},
            { "OTT","Ottawa Senators"},
            { "PHI","Philadelphia Flyers"},
            { "PIT","Pittsburgh Penguins"},
            { "SJS","San Jose Sharks"},
            { "STL","St. Louis Blues"},
            { "TBL","Tampa Bay Lightning"},
            { "TOR","Toronto Maple Leafs"},
            { "VAN","Vancouver Canucks"},
            { "WAS","Washington Capitals"},
            { "WPG","Winnipeg Jets"}
        };

        [Key]
        public int PlayerID { get; set; }

        public Position Position { get; set; }

        public string PlayerName { get; set; }

        public string Team { get; set; }

        public int Salary { get; set; }

        public bool Picked { get; set; }

        public virtual  ICollection<Statistic> Statistics { get; set; }

    }
    public enum Position
    {
        Forward, Defence, Goalie
    }
}
