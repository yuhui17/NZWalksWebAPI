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
    public class RegionRepository : IRegionRepository
    {
        private readonly ApplicationDbContext _db;

        public RegionRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<Region> CreateAsync(Region region)
        {
            await _db.Regions.AddAsync(region);
            await _db.SaveChangesAsync();

            return region;
        }

        public async Task<Region?> DeleteAsync(Guid id)
        {
            var regionFromDb = await _db.Regions.FirstOrDefaultAsync(u => u.Id == id);
            if (regionFromDb == null) 
            {
                return null;
            }

            _db.Regions.Remove(regionFromDb);
            await _db.SaveChangesAsync();

            return regionFromDb;
        }

        public async Task<List<Region>> GetAllAsync()
        {
            return await _db.Regions.ToListAsync();
        }

        public async Task<Region?> GetByIdAsync(Guid id)
        {
            return await _db.Regions.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<Region?> UpdateAsync(Guid id, Region region)
        {
            var regionFromDb = await _db.Regions.FirstOrDefaultAsync(u => u.Id == id);
            if(regionFromDb == null) 
            {
                return null;
            }

            regionFromDb.Code = region.Code;
            regionFromDb.Name = region.Name;
            regionFromDb.RegionImageUrl = region.RegionImageUrl;  
            
            await _db.SaveChangesAsync();
            return regionFromDb; 
        }
    }
}
