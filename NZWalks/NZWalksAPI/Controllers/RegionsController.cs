﻿using AutoMapper;
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
        
    }
}
