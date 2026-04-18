using Microsoft.AspNetCore.Mvc;
using Hotelguru.Services;
using Hotelguru.DataContext.Dtos;
namespace hotelguru.Controllers
{
        [ApiController] 
        [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;



        public UserController(IUserService userService)
        {
            _userService = userService;
        }


        [HttpPost("register")]
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

        
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(UserLoginDto dto)
        {

            var user = await _userService.LoginAsync(dto);

            if (user == null)
            {
               
                return Unauthorized("Hibás email cím vagy jelszó!");
            }

            return Ok(user);
        }

        // 3. PROFIL LEKÉRDEZÉS (GET: api/user/5)
        [HttpGet("{id}")]
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
    }
}
