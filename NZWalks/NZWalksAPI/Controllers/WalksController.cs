using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalksAPI.Repositories;

namespace NZWalksAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalksController : Controller
    {
        private readonly IWalkRepository walkRepository;
        private readonly IMapper mapper;

        public WalksController(IWalkRepository walkRepository,IMapper mapper)
        {
            this.walkRepository = walkRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWalksAsync()
        {
            //fetchhe data from databse----domain walk
            var walksDomain=await walkRepository.GetAllAsync();

            //Convert domain walks to DTO walks

            var walksDTO = mapper.Map<List<Models.DTO.Walk>>(walksDomain);


            //Return Response
            return Ok(walksDTO);
        }

        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetWalkAsync")]
        public async Task<IActionResult> GetWalkAsync(Guid id)
        {
            var walkDomain=await walkRepository.GetAsync(id);

            if(walkDomain==null)
            {
                return NotFound();
            }

            var walkDTO = mapper.Map<Models.DTO.Walk>(walkDomain);

            return Ok(walkDTO);

        }


        [HttpPost]
        public async Task<IActionResult> AddWalkAsync(Models.DTO.AddWalkRequest addWalkRequest)


        {
            //Convert DTO to domain object
            var walkDomain = new Models.Domain.Walk()
            {
                Name=addWalkRequest.Name,
                Length=addWalkRequest.Length,
                RegionId=addWalkRequest.RegionId,
                WalkDifficultyId=addWalkRequest.WalkDifficultyId
                              
            };

            //pass domain object to rpository to persist this

            walkDomain = await walkRepository.AddAsync(walkDomain);

            //Convert domain object to DTO object

            var WalkDTO = new Models.DTO.Walk()
            {
                Id=walkDomain.Id,
                Name=walkDomain.Name,
                Length=walkDomain.Length,
                RegionId=walkDomain.RegionId,
                WalkDifficultyId=walkDomain.WalkDifficultyId
            };

            return CreatedAtAction(nameof(GetWalkAsync), new {id=WalkDTO.Id},WalkDTO);
            
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateWalkAsync([FromRoute] Guid id,[FromBody] Models.DTO.UpdateWalkRequest updateWalkRequest)
        {

            //DTO to Domain objects

            var walkDomain = new Models.Domain.Walk()
            {
                Name = updateWalkRequest.Name,
                Length = updateWalkRequest.Length,
                RegionId = updateWalkRequest.RegionId,
                WalkDifficultyId = updateWalkRequest.WalkDifficultyId
            };

            walkDomain = await walkRepository.UpdateAsync(id,walkDomain);


            if(walkDomain==null)
            {
                return NotFound();
            }
            //Domain Objects to DTO Objects
            var walkDTO = new Models.DTO.Walk()
            {
                Id = walkDomain.Id,
                Name = walkDomain.Name,
                Length = walkDomain.Length,
                RegionId = walkDomain.RegionId,
                WalkDifficultyId = walkDomain.WalkDifficultyId
            };

            return Ok(walkDTO);



        }

        [HttpDelete]
        public async Task<IActionResult> DeleteWalkAsync(Guid id)
        {
            var walk=await walkRepository.DeleteAsync(id);

            if(walk==null)
            {
                return NotFound();
            }

            var walkDTO = new Models.DTO.Walk()
            {
                Id = walk.Id,
                Name=walk.Name,
                Length=walk.Length,
                RegionId=walk.RegionId,
                WalkDifficultyId=walk.WalkDifficultyId

            };

            return Ok(walkDTO);

        }
    }
}
