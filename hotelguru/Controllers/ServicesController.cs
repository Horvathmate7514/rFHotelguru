using Hotelguru.DataContext.Context;
using Hotelguru.DataContext.Dtos;
using Hotelguru.DataContext.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hotelguru.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServicesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ServicesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Services
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ServicesDto>>> GetServices()
        {
            var services = await _context.Services
                .Select(s => new ServicesDto
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
        public async Task<ActionResult<ServicesDto>> GetService(int id)
        {
            var serviceEntity = await _context.Services.FindAsync(id);

            if (serviceEntity == null)
            {
                return NotFound();
            }

            var dto = new ServicesDto
            {
                Id = serviceEntity.Id,
                Type = serviceEntity.Type,
                Price = serviceEntity.Price
            };

            return Ok(dto);
        }

        // POST: api/Services
        [HttpPost("createServices")]
        public async Task<ActionResult<ServicesDto>> CreateService(ServicesCreateDto createDto)
        {
            var serviceEntity = new Service
            {
                Type = createDto.Type,
                Price = createDto.Price
            };

            _context.Services.Add(serviceEntity);
            await _context.SaveChangesAsync();

            var responseDto = new ServicesDto
            {
                Id = serviceEntity.Id,
                Type = serviceEntity.Type,
                Price = serviceEntity.Price
            };

            return CreatedAtAction(nameof(GetService), new { id = serviceEntity.Id }, responseDto);
        }

        // PUT: api/Services/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateService(int id, ServicesUpdateDto updateDto)
        {
            var serviceEntity = await _context.Services.FindAsync(id);

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
        public async Task<IActionResult> DeleteService(int id)
        {
            var serviceEntity = await _context.Services.FindAsync(id);

            if (serviceEntity == null)
            {
                return NotFound();
            }

            _context.Services.Remove(serviceEntity);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}