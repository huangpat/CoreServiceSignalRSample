using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using CoreServiceSignalRSample.service;

namespace CoreServiceSignalRSample.hub
{

    public class SignalRHub : Hub
    {
        private const string CF_REFRESH = "refreshData";
        private const string CF_CLOSE = "closeConnection";


        private IBGService realtimeGame;

        public SignalRHub(IBGService realtimeGame)
        {
            this.realtimeGame = realtimeGame;
            Console.Out.WriteLine("RealtimeHub is initialized.");
            Console.Out.WriteLine($"realtimeGame is assigned");
        }



        /// <summary>
        /// Server invoke client functions
        /// </summary>
        /// <param name="gameRoom"></param>
        /// <param name="clientData"></param>
        /// <returns></returns>
        public async Task<Boolean> answerGame(string gameName)
        {
            string game = this.realtimeGame.getGame(gameName);
            if (game != "")
            {
                return true;
            }
            else
                return false;
        }







        /// <summary>
        /// client invoke hub methos
        /// </summary>
        /// <param name="gameRoom"></param>
        /// <param name="client"></param>
        /// <returns></returns>
        // client request
        //
        public async Task startGame(string gameRoom, string client)
        {
            Console.WriteLine("startGame", gameRoom);
            while (true)
            {
                await Clients.Group(gameRoom).SendAsync(CF_REFRESH, getRealtimeGameByRoomJson(gameRoom));
                await Task.Delay(1000 *2);
            }
        }
        // this only register in signalr hub
        public async Task joinGame(string gameRoom, string client)
        {
            Console.WriteLine("joinGame", gameRoom);
            string clientId = Context.ConnectionId;
            await Groups.AddToGroupAsync(clientId, gameRoom);

            await Clients.User(clientId).SendAsync(CF_REFRESH, getRealtimeGameByRoomJson(gameRoom));
        }

        // this will join the game servcie and register in signalr hub in one step
        public async Task joinGameService(string gameRoom)
        {
            Console.WriteLine("joinGame2", gameRoom);
            string clientId = Context.ConnectionId;

            this.realtimeGame.realtimeJoin(gameRoom);

            await Groups.AddToGroupAsync(clientId, gameRoom);

            await Clients.User(clientId).SendAsync(CF_REFRESH, getRealtimeGameByRoomJson(gameRoom));
        }

        public async Task leaveGame(string gameRoom)
        {
            this.realtimeGame.realtimeLeave(gameRoom);
        }


        //interal method
        private string getRealtimeGameByRoomJson(string gameRoom)
        {
            return JsonConvert.SerializeObject(this.realtimeGame.getGame(gameRoom), Formatting.Indented);
        }

        //for two way communication
        // calling "broadcast" function in client and send clientData
        private async Task broadcastDataToClient(string gameRoom, string newClientData)
        {
            await Clients.Group(gameRoom).SendAsync("broadcastData", newClientData);

        }

        // detect when client is disconnected
        public override async System.Threading.Tasks.Task OnDisconnectedAsync(Exception exception)
        {
            var b = Context.ConnectionId;

            // store the user id so it can be detected


            await base.OnDisconnectedAsync(exception);

        }

    }
}
