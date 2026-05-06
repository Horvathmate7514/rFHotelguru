using Hotelguru.DataContext.Dtos;
using Hotelguru.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace hotelguru.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]/[action]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;



        public UserController(IUserService userService)
        {
            _userService = userService;
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<UserDto>> Register(UserRegisterDto dto)
        {
            try
            {
                var result = await _userService.RegisterAsync(dto);

                return Ok(result);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }


        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<ActionResult> Login([FromBody] UserLoginDto dto)
        {
            try
            {
                // Meghívjuk a Service-t, ami most már egy stringet (a tokent) ad vissza
                var token = await _userService.LoginAsync(dto);

                // Visszaadjuk a tokent egy egyszerű JSON objektumban
                return Ok(new { token = token });
            }
            catch (UnauthorizedAccessException ex)
            {
                // Ha rossz a jelszó vagy az email
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // 3. PROFIL LEKÉRDEZÉS (GET: api/user/5)
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<UserDto>> GetProfile(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);

            if (user == null)
            {
                return NotFound("Nincs ilyen felhasználó!");
            }

            return Ok(user);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<UserDto>> Update(int id, UserUpdateDto dto)
        {
            try
            {
                var result = await _userService.UpdateUserAsync(id, dto);
                return Ok(result);
            }
            catch (KeyNotFoundException)
            {
                return NotFound("A módosítani kívánt felhasználó nem létezik.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Receptionist")]
        public async Task<ActionResult<List<UserDto>>> GetAll()
        {
            try
            {
                var result = await _userService.UserGetAllAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
