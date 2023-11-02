using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.CustomActionFilters;
using NZWalks.Model.Models.Domain;
using NZWalks.Model.Models.DTOs;
using NZWalks.NZWalksDataAccess.Repositories;
using System.Net;

namespace NZWalks.API.Controllers
{
    // /api/walks
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IWalkRepository _walkRepository;

        public WalksController(IMapper mapper, IWalkRepository walkRepository)
        {
            _mapper = mapper;
            _walkRepository = walkRepository;
        }

        //CREATE WALKS
        //POST: /api/Walks
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] AddWalkRequestDto addWalkRequestDto)
        {
            //Map DTO to Domain Model
            var walk = _mapper.Map<Walk>(addWalkRequestDto);

            await _walkRepository.CreateAsync(walk);

            //map domain model to DTO
            return Ok(_mapper.Map<WalkDto>(walk));
        }

        //GET WALKS
        //GET: /api/Walks?filterOn=Name&filterQuery=Track&sortBy=Name&isAscending=true&pageNumber=1&pageSize=10
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? filterOn, [FromQuery] string? filterQuery, [FromQuery] string? sortBy, [FromQuery] bool? isAscending, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 1000)
        {

            var walk = await _walkRepository.GetAllAsync(filterOn, filterQuery, sortBy, isAscending ?? true, pageNumber, pageSize);

            if (walk == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<List<WalkDto>>(walk));
        }

        //GET WALKS BY ID
        //GET: /api/Walks/{id}
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var walk = await _walkRepository.GetByIdAsync(id);

            if (walk == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<WalkDto>(walk));
        }
        //UPDATE WALKS BY ID
        //PUT: /api/Walks/{id}
        [HttpPut]
        [ValidateModel]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, UpdateWalkRequestDto updateWalkRequestDto)
        {
            var walk = _mapper.Map<Walk>(updateWalkRequestDto);

            walk = await _walkRepository.UpdateAsync(id, walk);

            if (walk == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<WalkDto>(walk));
        }

        //DELETE WALKS BY ID
        //GET: /api/Walks/{id}
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var walk = await _walkRepository.DeleteAsync(id);

            if (walk == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<WalkDto>(walk));
        }
    }
}
