using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalksAPI.Models.Domain;
using NZWalksAPI.Repositories;

namespace NZWalksAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalksDifficultyController : Controller
    {
        private readonly IWalkDifficultyRepository walkDifficultyRepository;
        private readonly IMapper mapper;

        public WalksDifficultyController(IWalkDifficultyRepository walkDifficultyRepository,IMapper mapper)
        {
            this.walkDifficultyRepository = walkDifficultyRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWalksDifficultyAsync()
        {
            var walks=await walkDifficultyRepository.GetAllAsync();

            var walksDTO = mapper.Map<List<Models.DTO.WalkDifficulty>>(walks);

            return Ok(walksDTO);
            
        }

        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetWalksDifficultyAsync")]
        public async Task<IActionResult>GetWalksDifficultyAsync(Guid id)
        {
            var walk=await walkDifficultyRepository.GetAsync(id);

            if(walk==null)
            {
                return NotFound();
            }

            var walkDTO=mapper.Map<Models.DTO.WalkDifficulty>(walk);

            return Ok(walkDTO);

        }

        [HttpPost]
        public async Task<IActionResult> AddWalksDifficultyAsync(WalkDifficulty walkDifficulty)
        {
            var walkDifficultyDomain = new Models.Domain.WalkDifficulty()
            {
                Code=walkDifficulty.Code

            };

            walkDifficultyDomain = await walkDifficultyRepository.AddAsync(walkDifficultyDomain);

            var walkDifficultyDTO=mapper.Map<Models.DTO.WalkDifficulty>(walkDifficulty);

            return Ok(walkDifficultyDTO);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateWalksDifficultyAsync(Guid id,Models.DTO.UpdateWalkDifficultyRequest updateWalkDifficultyRequest)
        {

            var walkDifficultyDomain = new Models.Domain.WalkDifficulty()
            {
                Code= updateWalkDifficultyRequest.Code
            };


            walkDifficultyDomain = await walkDifficultyRepository.UpdateAsync(id, walkDifficultyDomain); 

            if(walkDifficultyDomain==null)
            {
                return NotFound();
            }

            var walkDTO=mapper.Map<Models.DTO.WalkDifficulty>(walkDifficultyDomain);

            return Ok(walkDTO);
   

        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult>DeleteWalksDifficulty(Guid id)
        {
            var walkDifficultyDomain=await walkDifficultyRepository.DeleteAsync(id);

            if(walkDifficultyRepository==null)
            {
                return NotFound();
            }

            var walkDifficultyDTO = new Models.DTO.WalkDifficulty()
            {
                Id=walkDifficultyDomain.Id,
                Code=walkDifficultyDomain.Code
            };

            return CreatedAtAction("GetWalksDifficultyAsync", new { id=walkDifficultyDTO.Id}, walkDifficultyDTO);

        }
    }
}
