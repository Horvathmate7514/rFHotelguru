using Hotelguru.DataContext.Context;
using Hotelguru.DataContext.Dtos;
using Hotelguru.DataContext.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hotelguru.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class BenefitController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BenefitController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Services
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BenefitDto>>> BenefitGetAll()
        {
            var services = await _context.Benefits
                .Select(s => new BenefitDto
                {
                    Id = s.Id,
                    Type = s.Type,
                    Price = s.Price
                })
                .ToListAsync();

            return Ok(services);
        }

        // GET: api/Services/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<BenefitDto>> GetById(int id)
        {
            var serviceEntity = await _context.Benefits.FindAsync(id);

            if (serviceEntity == null)
            {
                return NotFound();
            }

            var dto = new BenefitDto
            {
                Id = serviceEntity.Id,
                Type = serviceEntity.Type,
                Price = serviceEntity.Price
            };

            return Ok(dto);
        }

        // POST: api/Services
        [HttpPost("createServices")]
        public async Task<ActionResult<BenefitDto>> CreateBenefit(BenefitCreateDto createDto)
        {
            var serviceEntity = new Benefit
            {
                Type = createDto.Type,
                Price = createDto.Price
            };

            _context.Benefits.Add(serviceEntity);
            await _context.SaveChangesAsync();

            var responseDto = new BenefitDto
            {
                Id = serviceEntity.Id,
                Type = serviceEntity.Type,
                Price = serviceEntity.Price
            };

            return CreatedAtAction(nameof(GetById), new { id = serviceEntity.Id }, responseDto);
        }

        // PUT: api/Services/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateService(int id, BenefitUpdateDto updateDto)
        {
            var serviceEntity = await _context.Benefits.FindAsync(id);

            if (serviceEntity == null)
            {
                return NotFound();
            }

            serviceEntity.Type = updateDto.Type;
            serviceEntity.Price = updateDto.Price;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Services/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBenefit(int id)
        {
            var serviceEntity = await _context.Benefits.FindAsync(id);

            if (serviceEntity == null)
            {
                return NotFound();
            }

            _context.Benefits.Remove(serviceEntity);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}