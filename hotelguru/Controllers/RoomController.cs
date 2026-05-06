using Hotelguru.DataContext.Dtos;
using Hotelguru.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace hotelguru.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class RoomController : ControllerBase
    {
        public readonly IRoomService _roomService;

        public RoomController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<List<RoomDto>>> GetAll()
        {
            try
            {
                var result = await _roomService.RoomGetAllAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<RoomDto>> GetById(int id)
        {
            try
            {
                var result = await _roomService.RoomGetByIdAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        [Authorize(Roles = "Admin, Receptionist")]
        public async Task<ActionResult<RoomDto>> Create(RoomCreateDto dto)
        {
            try
            {
                var result = await _roomService.RoomCreateAsync(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin, Receptionist")]
        public async Task<ActionResult<RoomDto>> Update(int id, RoomUpdateDto dto)
        {
            try
            {
                var result = await _roomService.RoomUpdateAsync(id, dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin, Receptionist")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var result = await _roomService.RoomDeleteAsync(id);
                if (result)
                {
                    return NoContent();
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("{roomId}/{facilityId}")]
        [Authorize(Roles = "Admin, Receptionist")]
        public async Task<ActionResult> AddFacility(RoomFacilityCreateDto dto)
        {
            try
            {
                var result = await _roomService.RoomAddFacilityAsync(dto);
                if (result)
                {
                    return Ok(result);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("{startDate}/{endDate}")]
        [AllowAnonymous]
        public async Task<ActionResult<List<RoomDto>>> GetAvailableInDateRange(DateTime startDate, DateTime endDate)
        {
            try
            {
                var result = await _roomService.RoomGetAvailableInDateRangeAsync(startDate, endDate);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}