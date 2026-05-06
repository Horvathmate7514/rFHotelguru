using Hotelguru.DataContext.Dtos;
using Hotelguru.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        [Authorize]
        public async Task<ActionResult<ReservationDto>> Create(ReservationCreateDto dto)
        {
            try
            {
                var result = await _reservationService.ReservationCreateAsync(dto);
                foreach (var benefit in dto.ReservationBenefits)
                {
                    var addBenefitDto = new ReservationBenefitCreateDto
                    {
                        ReservationId = result.Id,
                        BenefitId = benefit.BenefitId,
                        Quantity = benefit.Quantity
                    };
                    await _reservationService.ReservationAddBenefitAsync(addBenefitDto);
                }
                result = await _reservationService.ReservationInfoByIDAsync(result.Id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut]
        [Authorize]
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
        [HttpGet]
        [Authorize(Roles = "Admin, Receptionist")]
        public async Task<ActionResult<List<ReservationDto>>> GetAll()
        {
            try
            {
                var result = await _reservationService.ReservationGetAllAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("{userID}")]
        [Authorize]
        public async Task<ActionResult<List<ReservationDto>>> ListByUserID(int userID)
        {
            try
            {
                var result = await _reservationService.ReservationListByUserIDAsync(userID);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("{reservationID}")]
        [Authorize]
        public async Task<ActionResult<ReservationDto>> InfoByID(int reservationID)
        {
            try
            {
                var result = await _reservationService.ReservationInfoByIDAsync(reservationID);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("{userID}/{reservationID}")]
        [Authorize(Roles = "Admin, Receptionist")]
        public async Task<ActionResult<bool>> RequestAccept(int userID, int reservationID)
        {
            try
            {
                var result = await _reservationService.ReservationRequestAcceptAsync(userID, reservationID);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("{userID}/{reservationID}")]
        [Authorize(Roles = "Admin, Receptionist")]
        public async Task<ActionResult<bool>> RequestDeny(int userID, int reservationID)
        {
            try
            {
                var result = await _reservationService.ReservationRequestDenyAsync(userID, reservationID);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("check-in")]
        [Authorize(Roles = "Admin, Receptionist")]
        public async Task<ActionResult<ReservationDto>> CheckIn(ReservationCheckInDto dto)
        {
            try
            {
                var result = await _reservationService.ReservationCheckInAsync(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("check-out")]
        [Authorize(Roles = "Admin, Receptionist")]
        public async Task<ActionResult<ReservationDto>> CheckOut(ReservationCheckOutDto dto)
        {
            try
            {
                var result = await _reservationService.ReservationCheckOutAsync(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut]
        [Authorize(Roles = "Admin, Receptionist")]
        public async Task<ActionResult<ReservationDto>> AddService(ReservationBenefitCreateDto dto)
        {
            try
            {
                var result = await _reservationService.ReservationAddBenefitAsync(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("{reservationId}")]
        [Authorize(Roles = "Admin, Receptionist")]
        public async Task<ActionResult<InvoiceDto>> GenerateInvoice(int reservationId, [FromBody] int issuedBy)
        {
            try
            {
                var invoice = await _reservationService.GenerateInvoiceAsync(reservationId, issuedBy);
                return Ok(invoice);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
