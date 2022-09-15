using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalksAPI.Models.DTO;
using NZWalksAPI.Repositories;

namespace NZWalksAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RegionsController : Controller
    {
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;

        public RegionsController(IRegionRepository RegionRepository,IMapper mapper)
        {
            regionRepository = RegionRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRegionsAsync()
        {

            var regions=await regionRepository.GetAllAsync();

            //return DTO regions

            //var regionsDTO = new List<Models.DTO.Region>();
            //regions.ToList().ForEach(region =>
            //{
            //    var regionDTO = new Models.DTO.Region()
            //    {
            //        Id = region.Id,
            //        Name = region.Name,
            //        Code = region.Code,
            //        Area = region.Area,
            //        Lat = region.Lat,
            //        Long = region.Long,
            //        Population = region.Population
            //    };
            //    regionsDTO.Add(regionDTO);


            //});

            var regionsDTO = mapper.Map<List<Models.DTO.Region>>(regions);

            return Ok(regionsDTO);

            

        }

        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetRegionAsync")]
        public async Task<IActionResult>GetRegionAsync(Guid id)
        {
            var region= await regionRepository.GetAsync(id);

            if(region==null)
            {
                return NotFound();
            }
            var regionDTO=mapper.Map<Models.DTO.Region>(region);    
            
            return Ok(regionDTO);

        }

        [HttpPost]
        public async Task<IActionResult>AddRegionAsync(Models.DTO.AddRegionRequest addRegionRequest)
        {
            //Validate the Request
            //if(!ValidateAddRegionAsync(addRegionRequest))
            //{
            //    return BadRequest(ModelState);
            //}

            //Request(DTO) to domain model

            var region = new Models.Domain.Region() 
            { 
                Code=addRegionRequest.Code,
                Name=addRegionRequest.Name,
                Area=addRegionRequest.Area,
                Lat=addRegionRequest.Lat,
                Long=addRegionRequest.Long,
                Population=addRegionRequest.Population
            
            };

            //pass details to repository

              region =await regionRepository.AddAsync(region);

            //Convert back to DTO

            var regionDTO = new Models.DTO.Region()
            {
                Id=region.Id,
                Code = region.Code,
                Name = region.Name,
                Area = region.Area,
                Lat = region.Lat,
                Long = region.Long,
                Population = region.Population

            };

            return CreatedAtAction(nameof(GetRegionAsync), new {id=regionDTO.Id},regionDTO);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult>DeleteRegionAsync(Guid id)
        {
            //Get Region from Database
            var region = await regionRepository.DeleteAsync(id);

            //If null NotFound

            if (region == null)
            {
                return NotFound();
            }

            //Convert response back to DTO

            var regionDTO = new Models.DTO.Region()
            {
                Id= region.Id,
                Code = region.Code,
                Name=region.Name,
                Area=region.Area,
                Lat=region.Lat,
                Long=region.Long,
                Population=region.Population
            };
            //return Ok response

            return Ok(regionDTO);

        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult>UpdateRegionAsync([FromRoute]Guid id,[FromBody]UpdateRegionRequest updateRegionRequest)
        {
            //Validate the incoming request data

            //if(!ValidateUpdateRegionAsync(updateRegionRequest))
            //{
            //    return BadRequest(ModelState);
            //}

            //Convert DTO to domain model

            var region = new Models.Domain.Region()
            {
                Name=updateRegionRequest.Name,
                Code=updateRegionRequest.Code,
                Area=updateRegionRequest.Area,
                Lat= updateRegionRequest.Lat,
                Long = updateRegionRequest.Long,
                Population = updateRegionRequest.Population
            };

            //update region using repository
            region=await regionRepository.UpdateAsync(id,region);

            //If NULL then Notfound
            if(region == null)
            {
                return NotFound();
            }

            //Convert again domain to DTO
            var regionDTO = new Models.DTO.Region()
            {
                
                Name = region.Name,
                Code = region.Code,
                Area = region.Area,
                Lat = region.Lat,
                Long = region.Long,
                Population = region.Population
            };

            //Return ok response

            return Ok(regionDTO);

        }


        #region Private methods

        private bool ValidateAddRegionAsync(Models.DTO.AddRegionRequest addRegionRequest)
        {
            if(addRegionRequest==null)
            {
                ModelState.AddModelError(nameof(addRegionRequest.Code),
                    $"Add Region Data is Required.");

                return false;

            }


            if (string.IsNullOrWhiteSpace(addRegionRequest.Code))
            {
                ModelState.AddModelError(nameof(addRegionRequest.Code), $"{nameof(addRegionRequest.Code)} Cannot be null or empty or white space.");

            }

            if (string.IsNullOrWhiteSpace(addRegionRequest.Name))
            {
                ModelState.AddModelError(nameof(addRegionRequest.Name), $"{nameof(addRegionRequest.Name)} Cannot be null or empty or white space.");

            }

            if(addRegionRequest.Area<=0)
            {
                ModelState.AddModelError(nameof(addRegionRequest.Area), 
                    $"{nameof(addRegionRequest.Area)} Cannot be less than or equal to zero.");

            }

            

            if (addRegionRequest.Population <= 0)
            {
                ModelState.AddModelError(nameof(addRegionRequest.Population),
                    $"{nameof(addRegionRequest.Population)} Cannot be less than zero.");

            }
            if (ModelState.ErrorCount > 0)
            {
                return false;
            }

            return true;


        }

        private bool ValidateUpdateRegionAsync(UpdateRegionRequest updateRegionRequest)
        {
            if (updateRegionRequest == null)
            {
                ModelState.AddModelError(nameof(updateRegionRequest.Code),
                    $"Update Region Data is Required.");

                return false;

            }


            if (string.IsNullOrWhiteSpace(updateRegionRequest.Code))
            {
                ModelState.AddModelError(nameof(updateRegionRequest.Code), $"{nameof(updateRegionRequest.Code)} Cannot be null or empty or white space.");

            }

            if (string.IsNullOrWhiteSpace(updateRegionRequest.Name))
            {
                ModelState.AddModelError(nameof(updateRegionRequest.Name), $"{nameof(updateRegionRequest.Name)} Cannot be null or empty or white space.");

            }

            if (updateRegionRequest.Area <= 0)
            {
                ModelState.AddModelError(nameof(updateRegionRequest.Area),
                    $"{nameof(updateRegionRequest.Area)} Cannot be less than or equal to zero.");

            }

           

            if (updateRegionRequest.Population <= 0)
            {
                ModelState.AddModelError(nameof(updateRegionRequest.Population),
                    $"{nameof(updateRegionRequest.Population)} Cannot be less than zero.");

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
