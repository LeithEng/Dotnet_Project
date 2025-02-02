using System.Collections.Generic;
using System.Threading.Tasks;
using api.Models;

namespace api.Interfaces
{
    public interface IFavoriteHobbiesRepository : IBaseRepository<FavoriteHobby>
    {
        Task<IEnumerable<FavoriteHobby>> GetByUserIdAsync(string userId);
        Task<IEnumerable<FavoriteHobby>> GetUsersByHobbyAsync(string hobbyId, bool includes = false);
        Task RemoveByHobbyIdAsync(string hobbyId);
        Task<FavoriteHobby> GetByUserAndHobbyIdAsync(string userId, string hobbyId);
    }
}
