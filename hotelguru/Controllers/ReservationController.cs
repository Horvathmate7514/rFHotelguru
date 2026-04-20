using Microsoft.AspNetCore.Mvc;
using Hotelguru.Services;
using Hotelguru.DataContext.Dtos;
namespace hotelguru.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationService _reservationService;

        public ReservationController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }
        [HttpPost]
        public async Task<ActionResult<ReservationDto>> Create(ReservationCreateDto dto)
        {
            try
            {
                var result = await _reservationService.ReservationCreateAsync(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut]
        public async Task<ActionResult<ReservationDto>> Cancel(ReservationCancelDto dto)
        {
            try
            {
                var result = await _reservationService.ReservationCancelAsync(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
