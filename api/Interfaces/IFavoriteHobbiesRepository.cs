using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using api.Models;

namespace api.Interfaces
{
    public interface IFavoriteHobbiesRepository : IBaseRepository<FavoriteHobby>
    {
        Task<IEnumerable<FavoriteHobby>> getUsers(string hobbyId, bool includes = false);
    }
}