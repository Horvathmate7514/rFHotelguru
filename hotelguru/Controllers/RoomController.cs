using Hotelguru.DataContext.Context;
using Hotelguru.DataContext.Dtos;
using Hotelguru.DataContext.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
// Ide illeszd be a saját DbContext-ed névterét
// using Hotelguru.DataContext; 

namespace hotelguru.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomController : ControllerBase
    {
        // Cseréld ki a típusnevet a saját DbContext osztályod nevére (pl. HotelguruContext)
        private readonly AppDbContext _context;

        public RoomController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateRoom([FromBody] RoomCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // 1. Szobatípus lekérdezése az adatbázisból
            var roomType = await _context.Set<RoomType>().FindAsync(dto.RoomTypeId);

            // 2. Biztonsági ellenőrzés: Létezik a megadott típus?
            if (roomType == null)
            {
                return NotFound($"Szobatípus nem található a következő ID-val: {dto.RoomTypeId}");
            }

            // 3. Új szoba entitás létrehozása és az ár beállítása
            var newRoom = new Room
            {
                RoomTypeId = dto.RoomTypeId,
                PricePerNight = roomType.BasePrice // Itt emeljük át az alapárat
            };

            // 4. Mentés az adatbázisba
            _context.Set<Room>().Add(newRoom);
            await _context.SaveChangesAsync();

            // 5. Válasz DTO összeállítása
            var resultDto = new RoomDto
            {
                Id = newRoom.Id,
                RoomTypeId = newRoom.RoomTypeId,
                PricePerNight = newRoom.PricePerNight,
                Facilities = new List<Facility>() // Kezdetben üres lista
            };

            // 6. 201 Created válasz, ami tartalmazza az új objektumot
            // Megjegyzés: Ha van GetRoomById végpontod, az első paraméter annak a neve legyen
            return CreatedAtAction(nameof(CreateRoom), new { id = newRoom.Id }, resultDto);
        }


        // GET: api/Room
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoomDto>>> GetRooms()
        {
            var rooms = await _context.Set<Room>()
                .AsNoTracking()
                .Include(r => r.Facilities) // Ha be akarod tölteni a kapcsolatokat
                .Select(r => new RoomDto
                {
                    Id = r.Id,
                    RoomTypeId = r.RoomTypeId,
                    PricePerNight = r.PricePerNight,
                    Facilities = r.Facilities
                })
                .ToListAsync();

            return Ok(rooms);
        }

        // GET: api/Room/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<RoomDto>> GetRoom(int id)
        {
            var room = await _context.Set<Room>()
                .AsNoTracking()
                .Include(r => r.Facilities)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (room == null)
            {
                return NotFound($"Szoba nem található a következő ID-val: {id}");
            }

            var dto = new RoomDto
            {
                Id = room.Id,
                RoomTypeId = room.RoomTypeId,
                PricePerNight = room.PricePerNight,
                Facilities = room.Facilities
            };

            return Ok(dto);
        }

        // PUT: api/Room/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRoom(int id, [FromBody] RoomCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // 1. Meglévő szoba betöltése
            var roomToUpdate = await _context.Set<Room>().FindAsync(id);
            if (roomToUpdate == null)
            {
                return NotFound($"Szoba nem található a következő ID-val: {id}");
            }

            // 2. Ha változott a szobatípus, újra kell húzni az árat
            if (roomToUpdate.RoomTypeId != dto.RoomTypeId)
            {
                var newRoomType = await _context.Set<RoomType>().FindAsync(dto.RoomTypeId);
                if (newRoomType == null)
                {
                    return NotFound($"Az új szobatípus nem található a következő ID-val: {dto.RoomTypeId}");
                }

                roomToUpdate.RoomTypeId = dto.RoomTypeId;
                roomToUpdate.PricePerNight = newRoomType.BasePrice; // Új ár beállítása
            }

            // 3. Mentés (az EF Core trackeli a változásokat, elég a SaveChanges)
            await _context.SaveChangesAsync();

            return NoContent(); // 204 No Content a sikeres PUT standard válasza
        }

        // DELETE: api/Room/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoom(int id)
        {
            var room = await _context.Set<Room>().FindAsync(id);

            if (room == null)
            {
                return NotFound($"Szoba nem található a következő ID-val: {id}");
            }

            _context.Set<Room>().Remove(room);
            await _context.SaveChangesAsync();

            return NoContent(); // 204 No Content
        }
    }
}