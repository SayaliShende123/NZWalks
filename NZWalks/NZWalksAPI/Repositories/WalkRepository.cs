using Microsoft.EntityFrameworkCore;
using NZWalksAPI.Data;
using NZWalksAPI.Models.Domain;

namespace NZWalksAPI.Repositories
{
    public class WalkRepository : IWalkRepository
    {
        private readonly NZWalksDbContext nZWalksDbContext;

        public WalkRepository(NZWalksDbContext nZWalksDbContext)
        {
            this.nZWalksDbContext = nZWalksDbContext;
        }

        public async Task<Walk> AddAsync(Walk walk)
        {
            //Assign new Id
            walk.Id = Guid.NewGuid();
            await nZWalksDbContext.AddAsync(walk);
            await nZWalksDbContext.SaveChangesAsync();
            return walk;
        }

        public async Task<Walk> DeleteAsync(Guid id)
        {

            var existWalk =await nZWalksDbContext.walks.FindAsync(id);

            if(existWalk==null)
            {
                return null;
            }

            nZWalksDbContext.walks.Remove(existWalk);
            await nZWalksDbContext.SaveChangesAsync();

            return existWalk;


            
        }

        public async Task<IEnumerable<Walk>> GetAllAsync()
        {
            return await nZWalksDbContext.walks
                .Include(x=>x.Region)
                .Include(x=>x.WalkDifficulty)
                .ToListAsync();
           
        }


        
        public async Task<Walk> GetAsync(Guid id)
        {
            return await nZWalksDbContext.walks
                .Include(x=>x.Region)
                .Include(x=>x.WalkDifficulty)
                .FirstOrDefaultAsync(x=>x.Id==id);

        }

        public async Task<Walk> UpdateAsync(Guid id,Walk walk)
        {
            var walkExist = await nZWalksDbContext.walks.FindAsync(id);

            if (walkExist == null)
            {
                return null;
            }

            walkExist.Name= walk.Name;
            walkExist.Length=walk.Length;
            walkExist.RegionId = walk.RegionId;
            walkExist.WalkDifficultyId=walk.WalkDifficultyId;

            await nZWalksDbContext.SaveChangesAsync();


            return walkExist;

        }
    }
}
