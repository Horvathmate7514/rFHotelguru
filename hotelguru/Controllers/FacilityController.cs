using Microsoft.AspNetCore.Mvc;
using Hotelguru.Services;
using Hotelguru.DataContext.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace hotelguru.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
                var result = await _facilityService.CreateFacilityAsync(dto);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                // Ha a Service nem találja a szobát, 404-es hibát adunk
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FacilityDto>> GetFacility(int id)
        {
            var facility = await _facilityService.GetFacilityByIdAsync(id);

            if (facility == null)
            {
                return NotFound("Nincs ilyen szolgáltatás!");
            }

            return Ok(facility);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<FacilityDto>> Update(int id, FacilityUpdateDto dto)
        {
            try
            {
                var result = await _facilityService.UpdateFacilityAsync(id, dto);

                if (result == null)
                {
                    return NotFound("A módosítani kívánt szolgáltatás nem létezik.");
                }

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
                var success = await _facilityService.DeleteFacilityAsync(id);

                if (!success)
                {
                    return NotFound("A törölni kívánt szolgáltatás nem létezik.");
                }

                return Ok("Sikeres törlés!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}