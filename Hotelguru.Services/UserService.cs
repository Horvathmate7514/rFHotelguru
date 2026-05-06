using AutoMapper;
using Hotelguru.DataContext.Context;
using Hotelguru.DataContext.Dtos;
using Hotelguru.DataContext.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text;
using System.Threading.Tasks;

namespace Hotelguru.Services
{
    public interface IUserService
    {
        Task<UserDto> RegisterAsync(UserRegisterDto dto);
        Task<string> LoginAsync(UserLoginDto dto);
        Task<UserDto?> GetUserByIdAsync(int id);
        Task<UserDto> UpdateUserAsync(int id, UserUpdateDto dto);
        Task<List<UserDto>> UserGetAllAsync();
        Task<UserDto> UserLinkRoleAsync(UserLinkRoleDto dto);
        Task<UserDto> UserDetachRoleAsync(UserLinkRoleDto dto);
    }
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public UserService(AppDbContext context, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<UserDto> RegisterAsync(UserRegisterDto dto)
        {
            var user = _mapper.Map<DataContext.Entities.User>(dto);
            user.Roles = new List<Role>();
            var guestRole = _context.Roles.FirstOrDefault(r => r.Name == "Guest");
            if (guestRole != null)
            {
                user.Roles.Add(guestRole);
            }

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return _mapper.Map<UserDto>(user);
        }

        public async Task<string> LoginAsync(UserLoginDto userDto) // Itt sincs kérdőjel, és nem UserDto!
        {
            var user = await _context.Users
                .Include(u => u.Roles)
                .FirstOrDefaultAsync(x => x.Email == userDto.Email);

            if (user == null || user.Password != userDto.Password) // (vagy BCrypt)
            {
                throw new UnauthorizedAccessException("Helytelen email vagy jelszó!");
            }

            return await GenerateToken(user);
        }

        public async Task<UserDto?> GetUserByIdAsync(int id)
        {
            var user = _context.Users.Include(u => u.Address)
                .Include(u => u.Roles)
                .FirstOrDefault(u => u.Id == id);
            return user != null ? _mapper.Map<UserDto>(user) : null;
        }


        public async Task<UserDto> UpdateUserAsync(int id, UserUpdateDto dto)
        {

            var user = await _context.Users
                .Include(u => u.Address)
                .Include(u => u.Roles)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                throw new KeyNotFoundException("A felhasználó nem található.");
            }


            _mapper.Map(dto, user);


            await _context.SaveChangesAsync();


            return _mapper.Map<UserDto>(user);
        }

        public async Task<List<UserDto>> UserGetAllAsync()
        {
            var users = await _context.Users
                .Include(u => u.Address)
                .Include(u => u.Roles)
                .ToListAsync();
            return _mapper.Map<List<UserDto>>(users);

        }

        public async Task<string> LoginAsync(LoginDto userDto)
        {
            var user = await _context.Users
                .Include(u => u.Roles)
                .FirstOrDefaultAsync(x => x.Email == userDto.Email);

            if (user == null || user.Password != userDto.Password)
            {
                throw new UnauthorizedAccessException("Helytelen email vagy jelszó!");
            }

            return await GenerateToken(user);
        }

        public async Task<UserDto> UserLinkRoleAsync(UserLinkRoleDto dto)
        {
            var user = await _context.Users
                .Include(u => u.Address)
                .Include(u => u.Roles)
                .FirstOrDefaultAsync(u => u.Id == dto.UserId);
            if (user == null)
            {
                throw new KeyNotFoundException("A felhasználó nem található.");
            }
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Id == dto.RoleId);
            if (role == null)
            {
                throw new KeyNotFoundException("A szerep nem található.");
            }
            if (!user.Roles.Any(r => r.Id == role.Id))
            {
                user.Roles.Add(role);
                await _context.SaveChangesAsync();
            }
            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> UserDetachRoleAsync(UserLinkRoleDto dto)
        {
            var user = await _context.Users
                .Include(u => u.Address)
                .Include(u => u.Roles)
                .FirstOrDefaultAsync(u => u.Id == dto.UserId);
            if (user == null)
            {
                throw new KeyNotFoundException("A felhasználó nem található.");
            }
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Id == dto.RoleId);
            if (role == null)
            {
                throw new KeyNotFoundException("A szerep nem található.");
            }
            var userRole = user.Roles.FirstOrDefault(r => r.Id == role.Id);
            if (userRole != null)
            {
                user.Roles.Remove(userRole);
                await _context.SaveChangesAsync();
            }
            return _mapper.Map<UserDto>(user);
        }

        private async Task<string> GenerateToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expires = DateTime.Now.AddDays(Convert.ToDouble(_configuration["Jwt:ExpireDays"]));

            var id = await GetClaimsIdentity(user);
            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                id.Claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private async Task<ClaimsIdentity> GetClaimsIdentity(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.FullName), 
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Sid, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.AuthTime, DateTime.Now.ToString(CultureInfo.InvariantCulture))
            };

            if (user.Roles != null && user.Roles.Any())
            {
                claims.AddRange(user.Roles.Select(role => new Claim("roleIds", Convert.ToString(role.Id))));
                claims.AddRange(user.Roles.Select(role => new Claim(ClaimTypes.Role, role.Name)));
            }

            return new ClaimsIdentity(claims, "Token");
        }
    }
}
