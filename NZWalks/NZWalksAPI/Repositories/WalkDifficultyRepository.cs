using Microsoft.EntityFrameworkCore;
using NZWalksAPI.Data;
using NZWalksAPI.Models.Domain;

namespace NZWalksAPI.Repositories
{
    public class WalkDifficultyRepository : IWalkDifficultyRepository
    {
        private readonly NZWalksDbContext nZWalksDbContext;

        public WalkDifficultyRepository(NZWalksDbContext nZWalksDbContext)
        {
            this.nZWalksDbContext = nZWalksDbContext;
        }

        public async Task<WalkDifficulty> AddAsync(WalkDifficulty walkDifficulty)
        {
            walkDifficulty.Id = Guid.NewGuid();

            await nZWalksDbContext.walksDifficulty.AddAsync(walkDifficulty);
            await nZWalksDbContext.SaveChangesAsync();

            return walkDifficulty;
        }

        public async Task<WalkDifficulty> DeleteAsync(Guid id)
        {
            var walkDifficultyExist = await nZWalksDbContext.walksDifficulty.FindAsync(id);

            if(walkDifficultyExist==null)
            {
                return null;
            }

            nZWalksDbContext.walksDifficulty.Remove(walkDifficultyExist);
            await nZWalksDbContext.SaveChangesAsync();

            return walkDifficultyExist;
        }

        public async Task<IEnumerable<WalkDifficulty>> GetAllAsync()
        {
            return await nZWalksDbContext.walksDifficulty.ToListAsync();
        }

        public async Task<WalkDifficulty> GetAsync(Guid id)
        {
            return await nZWalksDbContext.walksDifficulty.FirstOrDefaultAsync(x => x.Id== id);
            
        }

        public async Task<WalkDifficulty> UpdateAsync(Guid id, WalkDifficulty walkDifficulty)
        {
            var walkDifficultyExist =await nZWalksDbContext.walksDifficulty.FindAsync(id);

            if(walkDifficultyExist==null)
            {
                return null;
            }

            walkDifficultyExist.Code = walkDifficulty.Code;

            await nZWalksDbContext.SaveChangesAsync();

            return walkDifficultyExist;


        }
    }
}
