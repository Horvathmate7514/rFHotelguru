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
    public interface IRoomService
    {
        Task<RoomDto> RoomCreateAsync(RoomCreateDto dto);
        Task<RoomDto> RoomGetByIdAsync(int id);
        Task<List<RoomDto>> RoomGetAllAsync();
        Task<RoomDto> RoomUpdateAsync(int id, RoomUpdateDto dto);
        Task<bool> RoomDeleteAsync(int id);
        Task<bool> RoomAddFacilityAsync(RoomFacilityCreateDto dto);
    }
    public class RoomService : IRoomService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public RoomService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<RoomDto> RoomCreateAsync(RoomCreateDto dto)
        {
            var room =
                new Room
                {
                    RoomTypeId = dto.RoomTypeId,
                    PricePerNight = null, //Ár megadása opcionális, alapértelmezetten null értékű
                    //Status = "Available", //Új szoba mindig "Available" státuszú lesz
                    RoomFacilities = new List<RoomFacility>() //Kezdetben üres lista a szoba szolgáltatásaihoz
                };
            _context.Rooms.Add(room);
            await _context.SaveChangesAsync();
            return _mapper.Map<RoomDto>(room);
        }
        public async Task<RoomDto> RoomGetByIdAsync(int id)
        {
            var room = await _context.Rooms.FirstOrDefaultAsync(r => r.Id == id);
            if (room == null)
            {
                throw new Exception("Room not found.");
            }
            else
            {
                return _mapper.Map<RoomDto>(room);
            }
        }
        public async Task<List<RoomDto>> RoomGetAllAsync()
        {
            var rooms = await _context.Rooms
                .Include(r => r.RoomFacilities) // Eagerly load RoomFacilities
                .ToListAsync();
            return _mapper.Map<List<RoomDto>>(rooms);
        }
        public async Task<RoomDto> RoomUpdateAsync(int id, RoomUpdateDto dto)
        {
            var room = await _context.Rooms.FirstOrDefaultAsync(r => r.Id == id);
            if (room == null)
            {
                throw new Exception("Room not found.");
            }
            else
            {
                room.PricePerNight = dto.PricePerNight;
                room.RoomTypeId = dto.RoomTypeId;
                //room.Status = dto.Status;
                await _context.SaveChangesAsync();
                return _mapper.Map<RoomDto>(room);
            }
        }
        public async Task<bool> RoomDeleteAsync(int id)
        {
            var room = await _context.Rooms.FirstOrDefaultAsync(r => r.Id == id);
            if (room == null)
            {
                throw new Exception("Room not found.");
            }
            else
            {
                _context.Rooms.Remove(room);
                await _context.SaveChangesAsync();
                return true;
            }
        }
        public async Task<bool> RoomAddFacilityAsync(RoomFacilityCreateDto dto)
        {
            var room = await _context.Rooms
                .Include(r => r.RoomFacilities)
                .FirstOrDefaultAsync(r => r.Id == dto.RoomId);
            if (room == null)
            {
                throw new Exception("Room not found.");
            }
            var facility = await _context.Facilities.FirstOrDefaultAsync(f => f.Id == dto.FacilityId);
            if (facility == null)
            {
                throw new Exception("Facility not found.");
            }
            // Ellenőrizzük, hogy már hozzá van-e adva ez a facility
            if (room.RoomFacilities.Any(rf => rf.FacilityId == dto.FacilityId))
            {
                // Már hozzá van adva, nem duplikáljuk
                return true;
            }
            var roomFacility = new RoomFacility
            {
                RoomId = dto.RoomId,
                FacilityId = dto.FacilityId
            };
            room.RoomFacilities.Add(roomFacility);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
