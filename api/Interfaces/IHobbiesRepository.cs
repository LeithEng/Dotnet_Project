using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using api.Models;

namespace api.Interfaces
{
    public interface IHobbiesRepository : IBaseRepository<Hobby>
    {
        Task<IEnumerable<Hobby>> GetFirstLevelAsync();
        Task<IEnumerable<Hobby>> GetChildren(string id);

    }
}