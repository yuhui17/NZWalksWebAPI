using Microsoft.EntityFrameworkCore;
using NZWalks.DataAccess.Data;
using NZWalks.Model.Models.Domain;
using NZWalks.NZWalksDataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NZWalks.DataAccess.Repositories
{
    public class WalkRepository : IWalkRepository
    {
        private readonly ApplicationDbContext _db;

        public WalkRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<Walk> CreateAsync(Walk walk)
        {
            await _db.Walks.AddAsync(walk);
            await _db.SaveChangesAsync();

            return walk;
        }

        public async Task<Walk?> DeleteAsync(Guid id)
        {
            var walkFromDb = await _db.Walks.FirstOrDefaultAsync(u => u.Id == id);
            if (walkFromDb == null) 
            {
                return null;
            }

            _db.Walks.Remove(walkFromDb);
            await _db.SaveChangesAsync();

            return walkFromDb;
        }

        public async Task<List<Walk>> GetAllAsync()
        {
            return await _db.Walks.Include("Difficulty").Include("Region").ToListAsync();
        }
         
        public async Task<Walk?> GetByIdAsync(Guid id)
        {
            return await _db.Walks.Include("Difficulty").Include("Region").FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<Walk?> UpdateAsync(Guid id, Walk walk)
        {
            var walkFromDb = await _db.Walks.FirstOrDefaultAsync(u => u.Id == id);
            if(walkFromDb == null) 
            {
                return null;
            }

            walkFromDb.Name = walk.Name;
            walkFromDb.Desciption = walk.Desciption;
            walkFromDb.LengthInKm = walk.LengthInKm;
            walkFromDb.WalkImageUrl = walk.WalkImageUrl;
            walkFromDb.DifficultyId = walk.DifficultyId;
            walkFromDb.RegionId = walk.RegionId;

            await _db.SaveChangesAsync();
            return walkFromDb; 
        }
    }
}
