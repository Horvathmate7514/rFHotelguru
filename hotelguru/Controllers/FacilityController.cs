using Microsoft.AspNetCore.Mvc;
using Hotelguru.Services;
using Hotelguru.DataContext.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace hotelguru.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class FacilityController : ControllerBase
    {
        private readonly IFacilityService _facilityService;

        public FacilityController(IFacilityService facilityService)
        {
            _facilityService = facilityService;
        }
        [HttpPost]
        public async Task<ActionResult<FacilityDto>> Create(FacilityCreateDto dto)
        {
            try
            {
                var result = await _facilityService.FacilityCreateAsync(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        public async Task<ActionResult<List<FacilityDto>>> GetAll()
        {
            try
            {
                var result = await _facilityService.FacilityGetAllAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<FacilityDto>> GetById(int id)
        {
            try
            {
                var result = await _facilityService.FacilityGetByIdAsync(id);
                if (result == null) return NotFound();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<FacilityDto>> Update(int id, FacilityUpdateDto dto)
        {
            try
            {
                var result = await _facilityService.FacilityUpdateAsync(id, dto);
                if (result == null) return NotFound();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var success = await _facilityService.FacilityDeleteAsync(id);
                if (!success) return NotFound();
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}