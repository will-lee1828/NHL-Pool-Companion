using System.Net;
using System.Web.Mvc;
using CsQuery;
using System.Collections.Concurrent;
using NHL_Pool_Companion.Models;
using System.Threading.Tasks;
using System.Linq;
using Newtonsoft.Json;
using System.Data.Entity;
using EntityFramework.Extensions;

namespace NHL_Pool_Companion.Controllers
{
    public class HomeController : Controller
    {
        private readonly int CurrentYear = System.DateTime.Now.Year;
        public ActionResult Index()
        {
            using (var nhldc = new NHLPoolDbContext())
            {
                var players = nhldc.Players.Include(l => l.Statistics).ToList();
                return View(players);
            }
                
        }

        public ActionResult ImportPlayers()
        {        
            var client = new WebClient();
            CQ domplayers = client.DownloadString("http://stats.nhlnumbers.com/player_stats/position/skater/year/" + CurrentYear);
            ProcessStats(domplayers);
            CQ domgoalies = client.DownloadString("http://stats.nhlnumbers.com/player_stats/position/goaltender/year/" + CurrentYear);
            ProcessStats(domgoalies);            

            return View();
        }

        private Player ProcessPlayer(string url)
        {
            var client = new WebClient();
            
            CQ playerDom = client.DownloadString("http://stats.nhlnumbers.com" + url);

            var detailsTable = playerDom["div.info-content"].Children("table[cellspacing][cellpadding]").First();

            var team = detailsTable["td:nth-child(2) a"].Attr("href").Replace("/teams/", "");

            var position = detailsTable["tr:nth-child(2) td:nth-child(2)"].First().Text();
            var salary = 0;
            var name = playerDom["span.blue"].First().Text();
            var salaryRow = playerDom["table[id='salary-data'] tbody tr"].First();
            var year = salaryRow.Children("td:first-child").Text();
            if(year == (CurrentYear + 1).ToString())
            {
                var tempSalary = salaryRow.Children("td:nth-child(6)").Text();
                var tempDecimal = 0.0;
                double.TryParse(tempSalary, out tempDecimal);
                salary =(int) (tempDecimal * 1000000.0);
            }
            var player = new Player() {
                Team = team,
                PlayerName = name,
                Salary = salary,
                Position = GetPlayerPosition(position)
            };

            

            return player;
        }

        private Statistic ProcessPlayerStatistics(CQ dom, Position position)
        {
            var statistics = new Statistic();
            if(position == Position.Forward || position == Position.Defence)
            {
                statistics.GamesPlayed = int.Parse(dom["td:nth-child(3)"].Text());
                statistics.Goals = int.Parse(dom["td:nth-child(4)"].Text());
                statistics.Assits = int.Parse(dom["td:nth-child(5)"].Text());
                statistics.PlusMinus = int.Parse(dom["td:nth-child(6)"].Text());
            }
            else
            {
                statistics.GamesPlayed = int.Parse(dom["td:nth-child(3)"].Text());
                statistics.Wins = int.Parse(dom["td:nth-child(6)"].Text());
                statistics.Losses = int.Parse(dom["td:nth-child(7)"].Text());
                statistics.OverTimeLosses = int.Parse(dom["td:nth-child(8)"].Text());
                statistics.ShutOut = int.Parse(dom["td:nth-child(15)"].Text());
            }
               

            return statistics;
        }

        private Position GetPlayerPosition(string position)
        {
            switch (position)
            {
                case "Left":
                case "Right":
                case "Center":
                    return Position.Forward;
                case "Defence":
                    return Position.Defence;
                case "Goaltender":
                    return Position.Goalie;
            }
            return Position.Forward;
        }

        private void ProcessStats(CQ dom)
        {
            var playerRows = dom["table[id='skaters-data'] tbody tr"];

            var parallelOptions = new ParallelOptions() { MaxDegreeOfParallelism = -1 };

            Parallel.ForEach(playerRows.ToList(), parallelOptions, playerRow =>
            {
                using (var nhldc = new NHLPoolDbContext())
                {
                    CQ innerDom = playerRow.InnerHTML.ToString();

                    var playerURL = innerDom["td:first-child a"].Attr("href");
                    var player = ProcessPlayer(playerURL);


                    nhldc.Players.Add(player);
                    nhldc.SaveChanges();

                    var stats = ProcessPlayerStatistics(innerDom, player.Position);
                    stats.Player = player;
                    nhldc.Statistics.Add(stats);
                    nhldc.SaveChanges();
                }

            });
        }

        public JsonResult PickPlayer(int PlayerID)
        {
            using (var nhldc = new NHLPoolDbContext())
            {
                var player = nhldc.Players.Find(PlayerID);
                if(player != null)
                {
                    player.Picked = !player.Picked;
                    nhldc.SaveChanges();
                }
                else
                {
                    return Json(new { success = false, PlayerID = PlayerID, ErrorMessage = "Player Not Found" });
                }
            }
            return Json(new { success = true, PlayerID = PlayerID, ErrorMessage = ""});
        }
    }
}