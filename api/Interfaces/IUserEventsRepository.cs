using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using api.Models;

namespace api.Interfaces
{
    public interface IUserEventsRepository : IBaseRepository<UserEvent>
    {
        Task<IEnumerable<UserEvent>> getUsers(string eventId, bool includes = false);
        
    }
}