using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalksAPI.Repositories;

namespace NZWalksAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalkDifficultyController : Controller
    {
        private readonly IWalkDifficultyRepository walkDifficultyRepository;
        private readonly IMapper mapper;

        public WalkDifficultyController(IWalkDifficultyRepository walkDifficultyRepository,IMapper mapper)
        {
            this.walkDifficultyRepository = walkDifficultyRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWalkDifficultyAsync()
        {
            var walkDifficulty=await walkDifficultyRepository.GetAllAsync();

            var walkDifficultyDTO = mapper.Map<List<Models.DTO.WalkDifficulty>>(walkDifficulty);

            return Ok(walkDifficultyDTO);            

        }

        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetWalkDifficultyAsync")]
        public async Task<IActionResult> GetWalkDifficultyAsync(Guid id)
        {
            var walkDifficulty=await walkDifficultyRepository.GetAsync(id);
            
            if(walkDifficulty==null)
            {
                return NotFound();
            }

            var walkDifficultyDTO=mapper.Map<Models.DTO.WalkDifficulty>(walkDifficulty);

            return Ok(walkDifficultyDTO);
        }

        [HttpPost]
        public async Task<IActionResult>AddWalkDifficultyAsync(Models.DTO.AddWalkDifficultyRequest addWalkDifficultyRequest)
        {
            var walkDifficultyDomain = new Models.Domain.WalkDifficulty()
            {
                Code = addWalkDifficultyRequest.Code
            };

            walkDifficultyDomain = await walkDifficultyRepository.AddAsync(walkDifficultyDomain);

            var walkDifficultyDTO = new Models.DTO.WalkDifficulty()
            {
                Id=walkDifficultyDomain.Id,
                Code=walkDifficultyDomain.Code
            };

            return CreatedAtAction(nameof(GetWalkDifficultyAsync), new { id=walkDifficultyDTO.Id}, walkDifficultyDTO);
        }
       
        [HttpPut]
        public async Task<IActionResult>UpdateWalkDifficultyAsync(Guid id,Models.DTO.UpdateWalkDifficultyRequest updateWalkDifficultyRequest)
        {
            var walkDifficultyDomain = new Models.Domain.WalkDifficulty()
            {
                Code=updateWalkDifficultyRequest.Code
            };

            walkDifficultyDomain = await walkDifficultyRepository.UpdateAsync(id, walkDifficultyDomain);

            if(walkDifficultyDomain==null)
            {
                return NotFound();
            }

            var walkDifficultyDTO = new Models.DTO.WalkDifficulty()
            {
                Id = walkDifficultyDomain.Id,
                Code=walkDifficultyDomain.Code

            };

            return Ok(walkDifficultyDTO);
        }

        [HttpDelete]
        [Route("{id:guid}")]

        public async Task<IActionResult> DeleteWalkDifficultyAsync(Guid id)
        {
            var walkDifficultyDomain=await walkDifficultyRepository.DeleteAsync(id);

            if(walkDifficultyDomain== null)
            {
                return NotFound();
            }

            var walkDifficultyDTO = new Models.DTO.WalkDifficulty()
            {
                Id = walkDifficultyDomain.Id,
                Code=walkDifficultyDomain.Code
            };

            return Ok(walkDifficultyDTO);

        }
        

        
        
    }
}

