using Hotelguru.DataContext.Context;
using Hotelguru.DataContext.Dtos;
using Hotelguru.DataContext.Entities;
using Hotelguru.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hotelguru.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class BenefitController : ControllerBase
    {
       private readonly IBenefitService _benefitService;

        public BenefitController(IBenefitService benefitService)
        {
            _benefitService = benefitService;
        }


        [HttpPost]
        public async Task<ActionResult<BenefitDto>> Create(BenefitCreateDto dto)
        {
            try
            {
                var result = await _benefitService.BenefitCreateAsync(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<ActionResult<List<BenefitDto>>> GetAll()
        {
            try
            {
                var result = await _benefitService.BenefitGetAllAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BenefitDto>> GetById(int id)
        {
            try
            {
                var result = await _benefitService.BenefitGetByIdAsync(id);
                if (result == null) return NotFound();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<BenefitDto>> Update(int id, BenefitUpdateDto dto)
        {
            try
            {
                var result = await _benefitService.BenefitUpdateAsync(id, dto);
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
                var success = await _benefitService.BenefitDeleteAsync(id);
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