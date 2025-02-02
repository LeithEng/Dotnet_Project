using Microsoft.EntityFrameworkCore;
using api.Interfaces;
using api.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;

namespace Repositories
{
    public class FavoriteHobbiesRepository : BaseRepository<FavoriteHobby>, IFavoriteHobbiesRepository
    {
        public FavoriteHobbiesRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<FavoriteHobby>> GetByUserIdAsync(string userId)
        {
            return await _context.FavoriteHobbies
                .Where(fh => fh.UserId == userId)
                .ToListAsync(); 
        }

        public async Task RemoveByHobbyIdAsync(string hobbyId)
        {
            var favoriteHobbies = await _context.FavoriteHobbies
                .Where(fh => fh.HobbyId == hobbyId)
                .ToListAsync();

            if (favoriteHobbies.Any())
            {
                _context.FavoriteHobbies.RemoveRange(favoriteHobbies);
                await _context.SaveChangesAsync();
            }
        }


        public async Task<IEnumerable<FavoriteHobby>> GetUsersByHobbyAsync(string hobbyId, bool includes = false)
        {
            return includes
                ? await FindAllAsync(fh => fh.HobbyId == hobbyId, new string[] { "User" })
                : await FindAllAsync(fh => fh.HobbyId == hobbyId);
        }

        public async Task<FavoriteHobby> GetByUserAndHobbyIdAsync(string userId, string hobbyId)
        {
            return await FindAsync(fh => fh.UserId == userId && fh.HobbyId == hobbyId);
        }
    }
}
