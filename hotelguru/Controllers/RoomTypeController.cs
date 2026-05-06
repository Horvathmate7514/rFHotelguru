using Hotelguru.DataContext.Dtos;
using Hotelguru.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace hotelguru.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class RoomTypeController : ControllerBase
    {
        private readonly IRoomTypeService _roomTypeService;

        public RoomTypeController(IRoomTypeService roomTypeService)
        {
            _roomTypeService = roomTypeService;
        }
        [HttpPost]
        [Authorize(Roles = "Admin, Receptionist")]
        public async Task<ActionResult<RoomTypeDto>> Create(RoomTypeCreateDto dto)
        {
            try
            {
                var result = await _roomTypeService.RoomTypeCreateAsync(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("{roomTypeID}")]
        [Authorize(Roles = "Admin, Receptionist")]
        public async Task<ActionResult<RoomTypeDto>> Update(int roomTypeID, RoomTypeUpdateDto dto)
        {
            try
            {
                var result = await _roomTypeService.RoomTypeUpdateAsync(roomTypeID, dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete("{roomTypeID}")]
        [Authorize(Roles = "Admin, Receptionist")]
        public async Task<ActionResult> Delete(int roomTypeID)
        {
            try
            {
                var result = await _roomTypeService.RoomTypeDeleteAsync(roomTypeID);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        [Authorize(Roles = "Admin, Receptionist")]
        public async Task<ActionResult<List<RoomTypeDto>>> GetAll()
        {
            try
            {
                var result = await _roomTypeService.RoomTypesGetAllAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
