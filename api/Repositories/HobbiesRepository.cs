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
    public class HobbiesRepository : BaseRepository<Hobby>  ,IHobbiesRepository
    {
        protected ApplicationDbContext _context;

        public HobbiesRepository(ApplicationDbContext context) : base(context)
        {
        }


        public Task<IEnumerable<Hobby>> GetFirstLevelAsync()
        {
           return FindAllAsync(b => b.Level == 1);
        }
        public Task<IEnumerable<Hobby>> GetChildren(string id)
        {
            return FindAllAsync(b => b.ParentHobbyId == id);
        }
    }
}
