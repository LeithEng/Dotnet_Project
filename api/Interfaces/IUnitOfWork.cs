using api.Interfaces;
using api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace api.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IBaseRepository<Post> posts { get; }
        IBaseRepository<Comment> comments { get; }
        IBaseRepository<Reaction> reactions { get; }
        IBaseRepository<Event> events { get;}
        IUserEventsRepository userEvents { get; }
        IHobbiesRepository hobbies { get; }
        IFavoriteHobbiesRepository favoriteHobbies { get; }
        int Complete();
    }
}