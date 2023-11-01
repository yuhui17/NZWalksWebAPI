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

        public async Task<List<Walk>> GetAllAsync(string? filterOn = null, string? filterQuery = null, string? sortBy = null, bool isAscending = true, int pageNumber = 1, int pageSize = 1000)
        {
            var walks = _db.Walks.Include("Difficulty").Include("Region").AsQueryable();

            //filtering
            if (string.IsNullOrWhiteSpace(filterOn) == false && string.IsNullOrWhiteSpace(filterQuery) == false)
            {
                if (filterOn.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    walks = walks.Where(u => u.Name.Contains(filterQuery));
                }
            }

            //sorting
            if (string.IsNullOrWhiteSpace(sortBy) == false)
            {
                if (sortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    walks = isAscending ? walks.OrderBy(u => u.Name) : walks.OrderByDescending(u => u.Name);
                }
                else if (sortBy.Equals("Length", StringComparison.OrdinalIgnoreCase))
                {
                    walks = isAscending ? walks.OrderBy(u => u.LengthInKm) : walks.OrderByDescending(u => u.LengthInKm);
                }
            }

            //pagination
            //for example pagenumber is 1, it will take 1-1, that mean skip 0 page we take the first pagesize.
            //for example pagesize is 10 it will skip 0 page and take 10 result.
            //if pagenumber = 2, skip 1, take next 10 result.
            var skipResults = (pageNumber -1) * pageSize;

            return await walks.Skip(skipResults).Take(pageSize).ToListAsync();
            //return await _db.Walks.Include("Difficulty").Include("Region").ToListAsync();
        }

        public async Task<Walk?> GetByIdAsync(Guid id)
        {
            return await _db.Walks.Include("Difficulty").Include("Region").FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<Walk?> UpdateAsync(Guid id, Walk walk)
        {
            var walkFromDb = await _db.Walks.FirstOrDefaultAsync(u => u.Id == id);
            if (walkFromDb == null)
            {
                return null;
            }

            walkFromDb.Name = walk.Name;
            walkFromDb.Description = walk.Description;
            walkFromDb.LengthInKm = walk.LengthInKm;
            walkFromDb.WalkImageUrl = walk.WalkImageUrl;
            walkFromDb.DifficultyId = walk.DifficultyId;
            walkFromDb.RegionId = walk.RegionId;

            await _db.SaveChangesAsync();
            return walkFromDb;
        }
    }
}
