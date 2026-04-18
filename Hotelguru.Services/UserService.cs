using AutoMapper;
using Hotelguru.DataContext.Context;
using Hotelguru.DataContext.Dtos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotelguru.Services
{
    public interface IUserService
    {
        Task<UserDto> RegisterAsync(UserRegisterDto dto);
        Task<UserDto?> LoginAsync(UserLoginDto dto);
        Task<UserDto?> GetUserByIdAsync(int id);
        Task<UserDto> UpdateUserAsync(int id, UserUpdateDto dto);
    }
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public UserService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<UserDto> RegisterAsync(UserRegisterDto dto)
        {
            var user = _mapper.Map<DataContext.Entities.User>(dto);
            var guestRole = _context.Roles.FirstOrDefault(r => r.Name == "Guest");
            if (guestRole != null)
            {
                user.Roles.Add(guestRole);
            }

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto?> LoginAsync(UserLoginDto dto)
        {
            var user = await _context.Users
                .Include(u => u.Address) 
                .Include(u => u.Roles)
                .FirstOrDefaultAsync(u => u.Email == dto.Email && u.Password == dto.Password);

            if (user == null) return null;

            return _mapper.Map<UserDto>(user);
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



    }
}
