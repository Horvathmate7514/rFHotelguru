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
        Task<FacilityDto> CreateFacilityAsync(FacilityCreateDto createDto);
        Task<FacilityDto?> GetFacilityByIdAsync(int id);
        Task<FacilityDto?> UpdateFacilityAsync(int id, FacilityUpdateDto updateDto);
        Task<bool> DeleteFacilityAsync(int id);
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

        public async Task<FacilityDto> CreateFacilityAsync(FacilityCreateDto createDto)
        {
            var roomExists = await _context.Rooms.AnyAsync(r => r.Id == createDto.RoomId);
            if (!roomExists)
            {
                throw new KeyNotFoundException($"A megadott szoba (ID: {createDto.RoomId}) nem létezik az adatbázisban.");
            }

            var facilityEntity = _mapper.Map<DataContext.Entities.Facility>(createDto);

            _context.Facilities.Add(facilityEntity);
            await _context.SaveChangesAsync();
            return _mapper.Map<FacilityDto>(facilityEntity);
        }

        public async Task<FacilityDto?> GetFacilityByIdAsync(int id)
        {
            var facility = await _context.Facilities.FindAsync(id);
            if (facility == null) return null;
            return _mapper.Map<FacilityDto>(facility);
        }

        public async Task<FacilityDto?> UpdateFacilityAsync(int id, FacilityUpdateDto updateDto)
        {
            var facility = await _context.Facilities.FindAsync(id);
            if (facility == null) return null;

            _mapper.Map(updateDto, facility);
            await _context.SaveChangesAsync();

            return _mapper.Map<FacilityDto>(facility);
        }

        public async Task<bool> DeleteFacilityAsync(int id)
        {
            var facility = await _context.Facilities.FindAsync(id);
            if (facility == null) return false;

            _context.Facilities.Remove(facility);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}