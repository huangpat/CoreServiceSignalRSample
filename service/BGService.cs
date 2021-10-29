using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CoreServiceSignalRSample.service
{
    public class BGService : BackgroundService, IBGService
    {
        private List<string> games;

        public BGService()
        {
            this.games = new List<string>();
            //this.addGame("Default Game");
        }

        public async Task<bool> addGame(string fGame)
        {
            if (this.games.Count <= 20)
            {
                this.games.Add(fGame.ToUpper() + "(" + Convert.ToString(this.games.Count) + ")");
                Console.WriteLine("service addGame:games count is " + Convert.ToString(this.games.Count));
                return true;
            }
            else
            {
                return false;
            }
        }

        public int getNumberOfGames()
        {
            return this.games.Count;
        }

        public string[] getGameNames()
        {
            return this.games.ToArray();
        }



        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            int i = 0;
            while (true)
            {
                i++;
                if (games.Count == 0)
                {
                    this.addGame("Default Game");
                }
                JsonSerializer js = new JsonSerializer();
                
                Console.WriteLine("ExecuteAsync {1} :games count is {0}:{2}", this.games.Count, i, JsonConvert.SerializeObject(this.games, Formatting.Indented));

                await Task.Delay(1000 * 2 * 5);
            }
            //throw new NotImplementedException();

            // use thread pool to start each game in collection
        }

        public ICollection<string> realtimeGetGames(bool includeAll = false)
        {
            return this.games;
        }
        public string realtimeJoin(string gameName)
        {
            this.games.Add(gameName);

            return gameName;
        }
        public void realtimeLeave(string gameName)
        {
            this.games.Remove(gameName);
        }


        public string getGame(string gameName)
        {
            string foundGame = "";
            foreach(var g in this.games)
            {
                if (g.ToUpper() == gameName.ToUpper())
                {
                    foundGame = g;
                }
            }
            return foundGame;
        }
    }
}
