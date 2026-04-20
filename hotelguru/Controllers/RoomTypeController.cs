using Hotelguru.DataContext.Context;
using Hotelguru.DataContext.Dtos;
using Hotelguru.DataContext.Entities;
using Microsoft.AspNetCore.Mvc;

namespace hotelguru.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomTypeController : ControllerBase
    {
        private readonly AppDbContext _context;
        
        public RoomTypeController(AppDbContext context)
        {
            _context = context;
        }


        // POST /api/RoomType/createRoomType
        [HttpPost("createRoomType")]
        // A visszatérési típus a válasz DTO
        public async Task<ActionResult<RoomTypeDto>> CreateRoomType(RoomTypeCreateDto createDto)
        {
            // 1. Extra validáció (a [Required] annotációk mellett)
            if (createDto.Capacity <= 0 || createDto.BasePrice < 0)
            {
                return BadRequest("Érvénytelen kapacitás vagy ár.");
            }

            // 2. Mapelés DTO-ból Entitásba (itt jön létre a mentendő objektum id nélkül)
            var roomTypeEntity = new RoomType
            {
                BedNumber = createDto.BedNumber,
                Capacity = createDto.Capacity,
                BasePrice = createDto.BasePrice
            };

            // 3. Mentés az adatbázisba
            _context.RoomTypes.Add(roomTypeEntity);
            await _context.SaveChangesAsync();
            // A SaveChangesAsync() után az EF Core automatikusan kitölti a roomTypeEntity.Id mezőt az adatbázis által generált értékkel.

            // 4. Mapelés Entitásból válasz DTO-ba
            var responseDto = new RoomTypeDto
            {
                Id = roomTypeEntity.Id,
                BedNumber = roomTypeEntity.BedNumber,
                Capacity = roomTypeEntity.Capacity,
                BasePrice = roomTypeEntity.BasePrice
            };

            // 5. Válasz a 201 Created státuszkóddal és a DTO-val
            return CreatedAtAction(nameof(GetRoomType), new { id = roomTypeEntity.Id }, responseDto);
        }

        // GET /api/RoomType/{id}
        [HttpGet("{id}")]
        // 1. A visszatérési típust RoomType-ról RoomTypeDto-ra módosítjuk
        public async Task<ActionResult<RoomTypeDto>> GetRoomType(int id)
        {
            // 2. Lekérjük az entitást az adatbázisból
            var roomTypeEntity = await _context.RoomTypes.FindAsync(id);

            // Ha nincs ilyen id, 404-et adunk vissza
            if (roomTypeEntity == null)
            {
                return NotFound();
            }

            // 3. Mapelés: Létrehozzuk a DTO-t, és beletöltjük az entitás adatait
            var roomTypeDto = new RoomTypeDto
            {
                Id = roomTypeEntity.Id,
                BedNumber = roomTypeEntity.BedNumber,
                Capacity = roomTypeEntity.Capacity,
                BasePrice = roomTypeEntity.BasePrice
            };

            // 4. Az entitás helyett a biztonságos DTO-t adjuk vissza a kliensnek
            return roomTypeDto;
        }

        // PUT /api/RoomType/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRoomType(int id, RoomTypeUpdateDto updateDto)
        {
            // 1. Megkeressük a módosítandó entitást az adatbázisban
            var roomTypeEntity = await _context.RoomTypes.FindAsync(id);

            // Ha nem létezik, 404 hiba
            if (roomTypeEntity == null)
            {
                return NotFound();
            }

            // 2. Extra validáció
            if (updateDto.Capacity <= 0 || updateDto.BasePrice < 0)
            {
                return BadRequest("Érvénytelen kapacitás vagy ár.");
            }

            // 3. Entitás adatainak felülírása a DTO-ból érkező új adatokkal
            roomTypeEntity.BedNumber = updateDto.BedNumber;
            roomTypeEntity.Capacity = updateDto.Capacity;
            roomTypeEntity.BasePrice = updateDto.BasePrice;

            // 4. Mentés az adatbázisba. Mivel az Entity Framework betöltötte az entitást,
            // "követi" a változásait (tracking), így elég csak a SaveChangesAsync()-et meghívni.
            await _context.SaveChangesAsync();

            // 5. Válasz visszaküldése (204 No Content)
            return NoContent();
        }

        // DELETE /api/RoomType/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoomType(int id)
        {
            // 1. Megkeressük az entitást
            var roomTypeEntity = await _context.RoomTypes.FindAsync(id);

            // Ha nincs meg, 404
            if (roomTypeEntity == null)
            {
                return NotFound();
            }

            // 2. Eltávolítjuk a memóriában lévő adathalmazból
            _context.RoomTypes.Remove(roomTypeEntity);

            // 3. Véglegesítjük a törlést az adatbázisban
            await _context.SaveChangesAsync();

            // 4. Válasz visszaküldése (204 No Content)
            return NoContent();
        }
    }
}
