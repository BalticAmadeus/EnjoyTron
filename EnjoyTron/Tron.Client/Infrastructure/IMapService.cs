using System.Threading.Tasks;
using Tron.AdminClient.Models;

namespace Tron.AdminClient.Infrastructure
{
    public interface IMapService
    {
        Task<Map> LoadMapAsync(string path);
    }
}
