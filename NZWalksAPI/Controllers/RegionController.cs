using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalksAPI.Data;
using NZWalksAPI.Models.Domain;
using NZWalksAPI.Models.DTOs;

namespace NZWalksAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;

        public RegionController(NZWalksDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        //the get all region
        [HttpGet]
        public IActionResult GetAll()
        { 
            //get data from database-Domain Models
            var regions = dbContext.Regions.ToList();


            //Map Domain Models to DTOs
            var regionsDto = new List<RegionDto>();
            foreach (var regionDomain in regions)
            {
                regionsDto.Add(new RegionDto()
                {
                    Id = regionDomain.Id,
                    Code = regionDomain.Code,
                    Name = regionDomain.Name,
                    RegionImageUrl = regionDomain.RegionImageUrl

                });

            }
           //return DTOs
            return Ok(regionsDto);
        }

        // get region by Id
        [HttpGet]
        [Route("id:Guid")]
        public IActionResult GetById([FromRoute] Guid id)
        {
           //get region domain model from database
            var regionDomain =dbContext.Regions.FirstOrDefault(x => x.Id == id);
            if(regionDomain == null)
            {
                return NotFound();
            }

            //map region domain to regoin Dto
            var regionDto = new RegionDto
            {
                Id = regionDomain.Id,
                Code = regionDomain.Code,
                Name = regionDomain.Name,
                RegionImageUrl = regionDomain.RegionImageUrl

            };
            //return dto back
            return Ok(regionDto);
        }

        [HttpPost]
        //Post to create a new region
        public IActionResult Create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            //map DTO to Domain Model
            var regionDomainModel = new Region
            {
                Code = addRegionRequestDto.Code,
                Name = addRegionRequestDto.Name,
                RegionImageUrl= addRegionRequestDto.RegionImageUrl
            };

            //Use Domain model to create region

            dbContext.Regions.Add(regionDomainModel);
            dbContext.SaveChanges();

            //map domain model back to DTO
            var regionDto = new RegionDto
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageUrl = regionDomainModel.RegionImageUrl
            };
            return CreatedAtAction(nameof(GetById), new { id=regionDto.Id}, regionDto);
        }

    }
}
