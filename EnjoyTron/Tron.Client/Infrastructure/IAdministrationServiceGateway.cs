using System.Collections.Generic;
using System.Threading.Tasks;
using Tron.AdminClient.Models;

namespace Tron.AdminClient.Infrastructure
{
    public interface IAdministrationServiceGateway
    {
        Task LoginAsync();
        Task<IEnumerable<Player>> ListPlayersAsync();
        Task<IEnumerable<Game>> ListGamesAsync();
        Task<Game> CreateGameAsync();
        Task<Game> GetGameAsync(int gameId);
        Task StartGameAsync(int gameId);
        Task AddPlayerAsync(int gameId, int playerId);
        Task RemovePlayerAsync(int gameId, int playerId);
        Task SetMapAsync(int gameId, Map map);
        Task DeleteGameAsync(int gameId);
        Task ResumeGameAsync(int gameId);
        Task PauseGameAsync(int gameId);
        Task<Match> GetMatchAsync(int gameId);
        Task<Turn> GetNextTurnAsync(int gameId);
        Task DropPlayer(int gameId, int playerId);
        Task<string> GetLiveInfo(int gameId);

        ConnectionData ConnectionData { get; set; }
        long LastCallTime { get; set; }
    }
}