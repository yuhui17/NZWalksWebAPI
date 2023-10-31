using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTOs;
using System.Text.Json.Serialization;

namespace NZWalks.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public RegionsController(ApplicationDbContext db)
        {
            _db = db;
        }

        //GET ALL REGIONS
        //GET : https://localhost:port/api/regions
        [HttpGet]
        public IActionResult GetAll()
        {
            //get data from database - domain models
            var regions = _db.Regions.ToList();

            //map domain model to DTOs
            var regionsDto = new List<RegionDTO>();

            foreach (var region in regions)
            {
                regionsDto.Add(new RegionDTO()
                {
                    Id = region.Id,
                    Name = region.Name,
                    Code = region.Code,
                    RegionImageUrl = region.RegionImageUrl
                });
            }

            //return DTOs
            return Ok(regionsDto);
        }

        //GET ALL ONE REGION (GET REGION BY ID)
        //GET : https://localhost:port/api/regions/{regionId}
        [HttpGet]
        [Route("{regionId:Guid}")]
        public IActionResult GetByRegionId([FromRoute] Guid regionId)
        {
            var region = _db.Regions.FirstOrDefault(u => u.Id == regionId);

            if (region == null)
            {
                return NotFound();
            }

            RegionDTO regionDto = new RegionDTO()
            {
                Id = region.Id,
                Name = region.Name,
                Code = region.Code,
                RegionImageUrl = region.RegionImageUrl
            }; 

            return Ok(regionDto);
        }


        //POST CREATE A NEW REGION
        //POST : https://localhost:port/api/regions
        [HttpPost]
        public IActionResult Create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            //map or convert DTO to domain model
            var region = new Region()
            {
                Code = addRegionRequestDto.Code,
                Name = addRegionRequestDto.Name,
                RegionImageUrl = addRegionRequestDto.RegionImageUrl
            };

            //use domain model to create region
            _db.Regions.Add(region);
            _db.SaveChanges();

            //map domain model back to dto
            var regionDto = new RegionDTO()
            {
                Id= region.Id,
                Name= region.Name,
                Code= region.Code,
                RegionImageUrl= region.RegionImageUrl
            };

            return CreatedAtAction(nameof(GetByRegionId), new { regionId = regionDto.Id}, regionDto);
        }

        //UPDATE REGION
        //PUT : https://localhost:port/api/regions/{regionId}
        [HttpPut]
        [Route("{regionId:Guid}")]  
        public IActionResult Update([FromRoute] Guid regionId, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
            //check if region exists
            var region = _db.Regions.FirstOrDefault(u=>u.Id == regionId);

            if(region == null)
            {
                return NotFound();
            }

            //map DTO to Domain Model
            region.Code = updateRegionRequestDto.Code;
            region.RegionImageUrl = updateRegionRequestDto.RegionImageUrl;
            region.Name = updateRegionRequestDto.Name;

            _db.SaveChanges();

            //convert domain model to Dto
            var regionDto = new RegionDTO
            {
                Id = region.Id,
                Code = region.Code,
                RegionImageUrl = region.RegionImageUrl,
                Name = region.Name
            };

            return Ok(regionDto);
        }

        [HttpDelete]
        [Route("{regionId:Guid}")]
        public IActionResult Delete([FromRoute] Guid regionId)
        {
            var region = _db.Regions.FirstOrDefault(u => u.Id == regionId);

            if (region == null)
            {
                return NotFound();
            }
            
            //delete region
            _db.Regions.Remove(region);
            _db.SaveChanges();

            //***THIS IS OPTIONAL***
            //return deleted region back
            //map domain model to DTO
            var regionDto = new RegionDTO
            {
                Id = region.Id,
                Code = region.Code,
                RegionImageUrl = region.RegionImageUrl,
                Name = region.Name
            };

            return Ok(regionDto);
        }
    }
}
