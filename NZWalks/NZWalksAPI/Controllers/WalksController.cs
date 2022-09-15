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
        private readonly IRegionRepository regionRepository;
        private readonly IWalkDifficultyRepository walkDifficultyRepository;

        public WalksController(IWalkRepository walkRepository,IMapper mapper,
            IRegionRepository regionRepository,IWalkDifficultyRepository walkDifficultyRepository)
        {
            this.walkRepository = walkRepository;
            this.mapper = mapper;
            this.regionRepository = regionRepository;
            this.walkDifficultyRepository = walkDifficultyRepository;
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
            //Validate incoming request
            if(!(await ValidateAddWalkAsync(addWalkRequest)))
            {
                return BadRequest(ModelState);
            }

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

            //validate the incoming request
            if(!(await ValidateUpdateWalkAsync(updateWalkRequest)))
            {
                return BadRequest(ModelState);
            }

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


        #region Private methods

        private async Task<bool> ValidateAddWalkAsync(Models.DTO.AddWalkRequest addWalkRequest)
        {
            //if(addWalkRequest==null)
            //{
            //    ModelState.AddModelError(nameof(addWalkRequest),
            //         $"{addWalkRequest} Cannot be null");

            //    return false;
            //}

            //if(string.IsNullOrWhiteSpace(addWalkRequest.Name))
            //{
            //    ModelState.AddModelError(nameof(addWalkRequest.Name),
            //        $"{addWalkRequest.Name} Cannot be Empty or white space");
            //}

            //if (addWalkRequest.Length<=0)
            //{
            //    ModelState.AddModelError(nameof(addWalkRequest.Length),
            //        $"{addWalkRequest.Length} Should be greater than zero.");
            //}

            var region =await regionRepository.GetAsync(addWalkRequest.RegionId);

            if(region==null)
            {
                ModelState.AddModelError(nameof(addWalkRequest.RegionId),
                   $"{addWalkRequest.RegionId} is Invalid.");
            }

            var walkDifficulty=await regionRepository.GetAsync(addWalkRequest.WalkDifficultyId);
            if (walkDifficulty == null)
            {
                ModelState.AddModelError(nameof(addWalkRequest.WalkDifficultyId),
                   $"{addWalkRequest.WalkDifficultyId} is Invalid.");
            }

            if (ModelState.ErrorCount > 0)
            {
                return false;
            }

            return true;

        }

        private async Task<bool> ValidateUpdateWalkAsync(Models.DTO.UpdateWalkRequest updateWalkRequest)
        {
            //if (updateWalkRequest == null)
            //{
            //    ModelState.AddModelError(nameof(updateWalkRequest),
            //         $"{updateWalkRequest} Cannot be null");

            //    return false;
            //}

            //if (string.IsNullOrWhiteSpace(updateWalkRequest.Name))
            //{
            //    ModelState.AddModelError(nameof(updateWalkRequest.Name),
            //        $"{updateWalkRequest.Name} Cannot be Empty or white space");
            //}

            //if (updateWalkRequest.Length <= 0)
            //{
            //    ModelState.AddModelError(nameof(updateWalkRequest.Length),
            //        $"{updateWalkRequest.Length} Should be greater than zero.");
            //}

            var region = await regionRepository.GetAsync(updateWalkRequest.RegionId);

            if (region == null)
            {
                ModelState.AddModelError(nameof(updateWalkRequest.RegionId),
                   $"{updateWalkRequest.RegionId} is Invalid.");
            }

            var walkDifficulty = await regionRepository.GetAsync(updateWalkRequest.WalkDifficultyId);
            if (walkDifficulty == null)
            {
                ModelState.AddModelError(nameof(updateWalkRequest.WalkDifficultyId),
                   $"{updateWalkRequest.WalkDifficultyId} is Invalid.");
            }

            if (ModelState.ErrorCount > 0)
            {
                return false;
            }

            return true;

        }


        #endregion
    }
}
