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
    public class FavoriteHobbiesRepository : BaseRepository<FavoriteHobby>  ,IFavoriteHobbiesRepository
    {
        protected ApplicationDbContext _context;

        public FavoriteHobbiesRepository(ApplicationDbContext context) : base(context)
        {
        }
        public Task<FavoriteHobby> GetByIdAsync(string userId)
        {
            return FindAsync(b => b.UserId == userId);
        } 
        public Task<IEnumerable<FavoriteHobby>> getUsers(string hobbyId, bool includes = false){
            if(includes){
                return FindAllAsync(b => b.HobbyId == hobbyId, new string[] { "User" });
            }
            return FindAllAsync(b => b.HobbyId == hobbyId);
        }
    }
}
