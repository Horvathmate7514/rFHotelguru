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
    public interface IRoomTypeService
    {
        public Task<RoomTypeDto> RoomTypeCreateAsync(RoomTypeCreateDto dto);
        public Task<RoomTypeDto> RoomTypeUpdateAsync(int roomTypeID, RoomTypeUpdateDto dto);
        public Task<bool> RoomTypeDeleteAsync(int roomTypeID);
        public Task<List<RoomTypeDto>> RoomTypesGetAllAsync();
    }
    public class RoomTypeService : IRoomTypeService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public RoomTypeService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<RoomTypeDto> RoomTypeCreateAsync(RoomTypeCreateDto dto)
        {
            var roomType = _mapper.Map<RoomType>(dto);
            _context.RoomTypes.Add(roomType);
            await _context.SaveChangesAsync();
            return _mapper.Map<RoomTypeDto>(roomType);
        }
        public async Task<RoomTypeDto> RoomTypeUpdateAsync(int roomTypeID, RoomTypeUpdateDto dto)
        {
            var roomType = await _context.RoomTypes.FindAsync(roomTypeID);
            if (roomType == null)
            {
                throw new Exception("No such roomtype found!");
            }
            _mapper.Map(dto, roomType);
            await _context.SaveChangesAsync();
            return _mapper.Map<RoomTypeDto>(roomType);
        }
        public async Task<bool> RoomTypeDeleteAsync(int roomTypeID)
        {
            var roomType = await _context.RoomTypes.FindAsync(roomTypeID);
            if (roomType == null)
            {
                throw new Exception("No such roomtype found!");
            }
            _context.RoomTypes.Remove(roomType);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<List<RoomTypeDto>> RoomTypesGetAllAsync()
        {
            var roomTypes = await _context.RoomTypes.ToListAsync();
            return _mapper.Map<List<RoomTypeDto>>(roomTypes);
        }
    }
}
