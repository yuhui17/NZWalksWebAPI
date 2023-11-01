﻿
using NZWalks.Model.Models.Domain;

namespace NZWalks.NZWalksDataAccess.Repositories
{

    public interface IWalkRepository
    {

        Task<List<Walk>> GetAllAsync();

        Task<Walk?> GetByIdAsync(Guid id);

        Task<Walk> CreateAsync(Walk walk);

        Task<Walk?> UpdateAsync(Guid id, Walk walk);

        Task<Walk?> DeleteAsync(Guid id);
    }
}
