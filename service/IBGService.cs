using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreServiceSignalRSample.service
{
    public interface IBGService
    {
        Task<bool> addGame(string fGame);
        string[] getGameNames();
        int getNumberOfGames();
        ICollection<string> realtimeGetGames(bool includeAll = false);
        string realtimeJoin(string gameName);
        void realtimeLeave(string gameName);

        string getGame(string gameName);
    }
}