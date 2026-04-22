using Microsoft.AspNetCore.Mvc;
using Hotelguru.Services;
using Hotelguru.DataContext.Dtos;
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
