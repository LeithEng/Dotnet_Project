using Microsoft.EntityFrameworkCore;
using api.Consts;
using api.Interfaces;
using api.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using api.Models;

namespace Repositories
{
    public class UserEventsRepository : BaseRepository<UserEvent>  ,IUserEventsRepository
    {
        protected ApplicationDbContext _context;

        public UserEventsRepository(ApplicationDbContext context) : base(context)
        {
        }
        public Task<UserEvent> GetByIdAsync(string userId)
        {
            return FindAsync(b => b.UserId == userId);
        } 
        public Task<IEnumerable<UserEvent>> getUsers(string eventId, bool includes = false){
            if(includes){
                return FindAllAsync(b => b.EventId == eventId, new string[] { "User" });
            }
            return FindAllAsync(b => b.EventId == eventId);
        }
    }
}



