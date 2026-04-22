using AutoMapper;
using Hotelguru.DataContext.Context;
using Hotelguru.DataContext.Dtos;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hotelguru.Services
{
    public interface IFacilityService
    {
        Task<FacilityDto> FacilityCreateAsync(FacilityCreateDto createDto);
        Task<List<FacilityDto>> FacilityGetAllAsync();
        Task<FacilityDto?> FacilityGetByIdAsync(int id);
        Task<FacilityDto?> FacilityUpdateAsync(int id, FacilityUpdateDto updateDto);
        Task<bool> FacilityDeleteAsync(int id);
    }

    public class FacilityService : IFacilityService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public FacilityService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<FacilityDto> FacilityCreateAsync(FacilityCreateDto createDto)
        {
            var facility = _mapper.Map<DataContext.Entities.Facility>(createDto);
            _context.Facilities.Add(facility);
            await _context.SaveChangesAsync();
            return _mapper.Map<FacilityDto>(facility);
        }
        public async Task<List<FacilityDto>> FacilityGetAllAsync()
        {
            var facilities = await _context.Facilities.ToListAsync();
            return _mapper.Map<List<FacilityDto>>(facilities);
        }
        public async Task<FacilityDto?> FacilityGetByIdAsync(int id)
        {
            var facility = await _context.Facilities.FindAsync(id);
            if (facility == null) return null;
            return _mapper.Map<FacilityDto>(facility);
        }
        public async Task<FacilityDto?> FacilityUpdateAsync(int id, FacilityUpdateDto updateDto)
        {
            var facility = await _context.Facilities.FindAsync(id);
            if (facility == null) return null;
            _mapper.Map(updateDto, facility);
            await _context.SaveChangesAsync();
            return _mapper.Map<FacilityDto>(facility);
        }
        public async Task<bool> FacilityDeleteAsync(int id)
        {
            var facility = await _context.Facilities.FindAsync(id);
            if (facility == null) return false;
            _context.Facilities.Remove(facility);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}