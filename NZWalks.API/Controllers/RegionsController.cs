using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.CustomActionFilters;
using NZWalks.DataAccess.Data;
using NZWalks.Model.Models.Domain;
using NZWalks.Model.Models.DTOs;
using NZWalks.NZWalksDataAccess.Repositories;
using System.Text.Json;
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
        private readonly ILogger<RegionsController> _logger;

        public RegionsController(ApplicationDbContext db, IRegionRepository regionRepository, IMapper mapper, ILogger<RegionsController> logger)
        {
            _db = db;
            _regionRepository = regionRepository;
            _mapper = mapper;
            _logger = logger;
        }

        //GET ALL REGIONS
        //GET : https://localhost:port/api/regions
        [HttpGet]
        //[Authorize(Roles = "Reader")]
        public async Task<IActionResult> GetAll()
        {
            //get data from database - domain models
            var regions = await _regionRepository.GetAllAsync();

            _logger.LogInformation($"Finished GetAllRegions request with data: {JsonSerializer.Serialize(regions)}");

            //return DTOs
            return Ok(_mapper.Map<List<RegionDTO>>(regions));
        }

        //GET ALL ONE REGION (GET REGION BY ID)
        //GET : https://localhost:port/api/regions/{id}
        [HttpGet]
        //[Authorize(Roles = "Reader")]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var region = await _regionRepository.GetByIdAsync(id);

            if (region == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<RegionDTO>(region));
        }


        //POST CREATE A NEW REGION
        //POST : https://localhost:port/api/regions
        [HttpPost]
        //[Authorize(Roles = "Writer")]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            //map or convert DTO to domain model
            var region = _mapper.Map<Region>(addRegionRequestDto);

            //use domain model to create region
            region = await _regionRepository.CreateAsync(region);

            //map domain model back to dto
            var regionDto = _mapper.Map<RegionDTO>(region);

            return CreatedAtAction(nameof(GetById), new { id = regionDto.Id }, regionDto);
        }

        //UPDATE REGION
        //PUT : https://localhost:port/api/regions/{id}
        [HttpPut]
        //[Authorize(Roles = "Writer")]
        [ValidateModel]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
            //Map DTO to Domain Model
            var region = _mapper.Map<Region>(updateRegionRequestDto);

            region = await _regionRepository.UpdateAsync(id, region);

            if (region == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<RegionDTO>(region));

        }

        [HttpDelete]
        //[Authorize(Roles = "Writer")]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var region = await _regionRepository.DeleteAsync(id);

            if (region == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<RegionDTO>(region));
        }
    }
}
