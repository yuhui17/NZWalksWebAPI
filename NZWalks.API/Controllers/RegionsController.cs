using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.DataAccess.Data;
using NZWalks.Model.Models.Domain;
using NZWalks.Model.Models.DTOs;
using NZWalks.NZWalksDataAccess.Repositories;
using System.Text.Json.Serialization;

namespace NZWalks.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly IRegionRepository _regionRepository;
        private readonly IMapper _mapper;

        public RegionsController(ApplicationDbContext db, IRegionRepository regionRepository, IMapper mapper)
        {
            _db = db;
            _regionRepository = regionRepository;
            _mapper = mapper;
        }

        //GET ALL REGIONS
        //GET : https://localhost:port/api/regions
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            //get data from database - domain models
            var regions = await _regionRepository.GetAllAsync();

            //map domain model to DTOs
            //var regionsDto = new List<RegionDTO>();

            //foreach (var region in regions)
            //{
            //    regionsDto.Add(new RegionDTO()
            //    {
            //        Id = region.Id,
            //        Name = region.Name,
            //        Code = region.Code,
            //        RegionImageUrl = region.RegionImageUrl
            //    });
            //}

            //return DTOs
            return Ok(_mapper.Map<List<RegionDTO>>(regions));
        }

        //GET ALL ONE REGION (GET REGION BY ID)
        //GET : https://localhost:port/api/regions/{regionId}
        [HttpGet]
        [Route("{regionId:Guid}")]
        public async Task<IActionResult> GetByRegionId([FromRoute] Guid regionId)
        {
            var region = await _regionRepository.GetByIdAsync(regionId);

            if (region == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<RegionDTO>(region));
        }


        //POST CREATE A NEW REGION
        //POST : https://localhost:port/api/regions
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            //map or convert DTO to domain model
            var region = _mapper.Map<Region>(addRegionRequestDto);

            //use domain model to create region
            region = await _regionRepository.CreateAsync(region);

            //map domain model back to dto
            var regionDto = _mapper.Map<RegionDTO>(region);

            return CreatedAtAction(nameof(GetByRegionId), new { regionId = regionDto.Id }, regionDto);
        }

        //UPDATE REGION
        //PUT : https://localhost:port/api/regions/{regionId}
        [HttpPut]
        [Route("{regionId:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid regionId, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
            //Map DTO to Domain Model
            var region = _mapper.Map<Region>(updateRegionRequestDto);

            region = await _regionRepository.UpdateAsync(regionId, region);

            if (region == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<RegionDTO>(region));
        }

        [HttpDelete]
        [Route("{regionId:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid regionId)
        {
            var region = await _regionRepository.DeleteAsync(regionId);

            if (region == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<RegionDTO>(region));
        }
    }
}
