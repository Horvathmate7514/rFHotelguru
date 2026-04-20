using AutoMapper;
using Hotelguru.DataContext.Context;
using Hotelguru.DataContext.Dtos;
using Hotelguru.DataContext.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotelguru.Services
{
    public interface IRoleService
    {
        Task<RoleDto> RoleCreateAsync(RoleCreateDto dto);
        Task<RoleDto> RoleDeleteAsync(RoleDeleteDto dto);
        Task<List<RoleDto>> RoleListAsync();
    }
    public class RoleService : IRoleService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public RoleService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<RoleDto> RoleCreateAsync(RoleCreateDto dto)
        {
            var role = _mapper.Map<Role>(dto);
            if (await _context.Roles.AnyAsync(r => r.Name == role.Name))
            {
                throw new Exception("Role already exists");
            }
            _context.Roles.Add(role);
            await _context.SaveChangesAsync();
            return _mapper.Map<RoleDto>(role);
        }
        public async Task<RoleDto> RoleDeleteAsync(RoleDeleteDto dto)
        {
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == dto.Name);
            if (role == null)
            {
                throw new Exception("Role not found");
            }
            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();
            return _mapper.Map<RoleDto>(role);
        }
        public async Task<List<RoleDto>> RoleListAsync()
        {
            var roles = await _context.Roles.ToListAsync();
            return _mapper.Map<List<RoleDto>>(roles);
        }
    }
}
