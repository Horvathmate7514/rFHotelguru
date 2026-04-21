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
            // AnyAsync helyett betöltjük a konkrét szobát, hogy módosíthassuk
            var room = await _context.Rooms.FirstOrDefaultAsync(r => r.Id == createDto.RoomId);

            if (room == null)
            {
                throw new KeyNotFoundException($"A megadott szoba (ID: {createDto.RoomId}) nem létezik az adatbázisban.");
            }

            var facilityEntity = _mapper.Map<DataContext.Entities.Facility>(createDto);

            // A szoba árának növelése a facility árával. 
            // Null-coalescing operátorral (??) kezeljük, ha a PricePerNight esetleg null lenne.
            room.PricePerNight = (room.PricePerNight ?? 0) + facilityEntity.Price;

            _context.Facilities.Add(facilityEntity);

            // A SaveChangesAsync egy tranzakcióban menti el az új Facility-t és a Room módosított árát
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

            // 1. Eltároljuk a régi árat a módosítás (térképezés) előtt
            var oldPrice = facility.Price;

            // 2. Az AutoMapper felülírja a facility entitás tulajdonságait az új adatokkal
            _mapper.Map(updateDto, facility);

            // 3. Kiszámítjuk az árváltozás mértékét (Új ár - Régi ár)
            var priceDifference = facility.Price - oldPrice;

            // 4. Csak akkor nyúlunk a szobához és terheljük az adatbázist, ha az ár ténylegesen változott
            if (priceDifference != 0)
            {
                var room = await _context.Rooms.FindAsync(facility.RoomID);
                if (room == null)
                {
                    throw new KeyNotFoundException($"A facility-hez tartozó szoba (ID: {facility.RoomID}) nem található.");
                }

                // A különbséget hozzáadjuk a szoba jelenlegi árához (a nullt 0-ként kezelve)
                room.PricePerNight = (room.PricePerNight ?? 0) + priceDifference;
            }

            // A SaveChanges egyetlen tranzakcióban menti a facility-t és a szobát is
            await _context.SaveChangesAsync();

            return _mapper.Map<FacilityDto>(facility);
        }

        public async Task<bool> DeleteFacilityAsync(int id)
        {
            // 1. Megkeressük a törlendő szolgáltatást
            var facility = await _context.Facilities.FindAsync(id);
            if (facility == null) return false;

            // 2. Betöltjük a hozzá tartozó szobát
            var room = await _context.Rooms.FindAsync(facility.RoomID);

            // 3. Ha a szoba létezik, levonjuk az árat
            if (room != null)
            {
                // Kivonjuk a törölt elem árát, ügyelve a null kezelésre
                room.PricePerNight = (room.PricePerNight ?? 0) - facility.Price;

                // Opcionális biztonsági fék: Ne mehessen az ár 0 alá
                if (room.PricePerNight < 0) room.PricePerNight = 0;
            }

            // 4. Eltávolítjuk a szolgáltatást
            _context.Facilities.Remove(facility);

            // 5. Mentés (törlés + ár módosítás egyszerre)
            await _context.SaveChangesAsync();

            return true;
        }
    }
}