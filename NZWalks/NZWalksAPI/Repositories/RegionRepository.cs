using Microsoft.EntityFrameworkCore;
using NZWalksAPI.Data;
using NZWalksAPI.Models.Domain;

namespace NZWalksAPI.Repositories
{
    public class RegionRepository : IRegionRepository
    {
        private readonly NZWalksDbContext nZWalksDbContext;

        public RegionRepository(NZWalksDbContext nZWalksDbContext)
        {
            this.nZWalksDbContext = nZWalksDbContext;
        }

        public async Task<Region> AddAsync(Region region)
        {
            region.Id=Guid.NewGuid();
            await nZWalksDbContext.AddAsync(region);
            await nZWalksDbContext.SaveChangesAsync();
            return region;
        }

        public async Task<Region> DeleteAsync(Guid id)
        {
            var region = await nZWalksDbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);

            if(region == null)
            {
                return null;
            }
            //delete the region

            nZWalksDbContext.Regions.Remove(region);
            await nZWalksDbContext.SaveChangesAsync();

            return region;

        }

        public async Task<IEnumerable<Region>> GetAllAsync()
        {
            return await nZWalksDbContext.Regions.ToListAsync();             
            
        }

        public async Task<Region> GetAsync(Guid id)
        {
            return await nZWalksDbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Region> UpdateAsync(Guid id, Region region)
        {
            var ExistingRegion=await nZWalksDbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
            if(ExistingRegion==null)
            {
                return null;
            }


            ExistingRegion.Code= region.Code;
            ExistingRegion.Name= region.Name;
            ExistingRegion.Area= region.Area;
            ExistingRegion.Lat= region.Lat;
            ExistingRegion.Long=region.Long;
            ExistingRegion.Population=region.Population;

            await nZWalksDbContext.SaveChangesAsync();

            return ExistingRegion;

        }
    }
}
