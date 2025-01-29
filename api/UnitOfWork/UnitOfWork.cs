using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;
using api.Data;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace api.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;  


        public IBaseRepository<Post> posts { get; private set; }
        public IBaseRepository<Comment> comments { get; private set; }
        public IBaseRepository<Reaction> reactions { get; private set; }
        public IBaseRepository<Event> events { get; private set; }
        public IUserEventsRepository userEvents { get; private set; }
        public IHobbiesRepository hobbies { get; private set; }
        public IFavoriteHobbiesRepository favoriteHobbies { get; private set; }
        public UserManager<User> UserManager => _userManager;



        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            // Initializing repositories
            posts = new BaseRepository<Post>(_context);
            comments = new BaseRepository<Comment>(_context);
            reactions = new BaseRepository<Reaction>(_context);
            events = new BaseRepository<Event>(_context);
            userEvents = new UserEventsRepository(_context);
            hobbies = new HobbiesRepository(_context);
            favoriteHobbies = new FavoriteHobbiesRepository(_context);
        }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}