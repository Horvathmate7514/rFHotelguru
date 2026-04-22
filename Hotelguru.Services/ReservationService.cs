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
    public interface IReservationService
    {
        Task<ReservationDto> ReservationCreateAsync(ReservationCreateDto dto);
        Task<ReservationDto> ReservationCancelAsync(ReservationCancelDto dto);
        Task<List<ReservationDto>> ReservationGetAllAsync();
        Task<List<ReservationDto>> ReservationListByUserIDAsync(int userID);
        Task<ReservationDto> ReservationInfoByIDAsync(int reservationID);
        Task<bool> ReservationRequestAcceptAsync(int userID, int reservationID);
        Task<bool> ReservationRequestDenyAsync(int userID, int reservationID);
        Task<ReservationDto> ReservationCheckInAsync(ReservationCheckInDto dto);
        Task<ReservationDto> ReservationCheckOutAsync(ReservationCheckOutDto dto);
        Task<InvoiceDto> GenerateInvoiceAsync(int reservationId, int employeeId);
    }
    public class ReservationService : IReservationService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ReservationService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<ReservationDto> ReservationCreateAsync(ReservationCreateDto dto)
        {
            if (!await _context.Reservations
                .AnyAsync(r => r.RoomId == dto.RoomId //Szoba azonosítója megegyezik a foglalási kérelemben megadott szoba azonosítójával
                && (r.Status != "Cancelled" && r.Status != "CheckedOut") //A foglalás státusza nem "Cancelled" vagy "CheckedOut"
                && (r.ToDate >= dto.FromDate && r.FromDate <= dto.ToDate))) //A foglalás időszaka nem fed át a foglalási kérelemben megadott időszakkal
            {//Csak akkor lehet foglalni, ha nincs már foglalás az adott szobára ugyanarra az időszakra ami nem "Cancelled" vagy "CheckedOut" státuszú
                var reservation =
                new DataContext.Entities.Reservation
                {
                    UserId = dto.UserId,
                    RoomId = dto.RoomId,
                    FromDate = dto.FromDate,
                    ToDate = dto.ToDate,
                    CancellationDeadline = dto.FromDate.AddDays(-1), //Lemondási határidő a foglalás kezdete előtt 1 nappal
                                                                     //-(nincs pontos leírás hogy mennyi kell legyen!!!)
                    Status = "Requested",
                    ReservationBenefits = new List<ReservationBenefit>()
                };
                foreach (var benefitDto in dto.ReservationBenefits)
                {
                    var reservationBenefit = new ReservationBenefit
                    {
                        BenefitId = benefitDto.BenefitId,
                        Quantity = benefitDto.Quantity
                    };
                    reservation.ReservationBenefits.Add(reservationBenefit);
                }
                _context.Reservations.Add(reservation);
                await _context.SaveChangesAsync();
                return _mapper.Map<ReservationDto>(reservation);
            }
            else
            {
                throw new Exception("The room is already taken!");
            }
        }
        public async Task<ReservationDto> ReservationCancelAsync(ReservationCancelDto dto)
        {
            var reservation = await _context.Reservations
                .FirstOrDefaultAsync(r => r.Id == dto.ReservationId && r.UserId == dto.UserId);
            if (reservation == null)
            {
                throw new Exception("Reservation not found.");
            }
            if (reservation.CancellationDeadline < DateTime.Now)
            {
                throw new Exception("Cancellation deadline reached.");
            }
            reservation.Status = "Cancelled";
            await _context.SaveChangesAsync();
            return _mapper.Map<ReservationDto>(reservation);
        }
        public async Task<List<ReservationDto>> ReservationGetAllAsync()
        {
            var reservations = await _context.Reservations.ToListAsync();

            if (reservations == null || !reservations.Any())
            {
                throw new Exception("Reservations not found.");
            }

            return _mapper.Map<List<ReservationDto>>(reservations);
        }

        public async Task<List<ReservationDto>> ReservationListByUserIDAsync(int userID)
        {
            var reservations = _context.Reservations.Where(r => r.UserId == userID).ToListAsync();
            if (reservations == null)
            {
                throw new Exception("Reservations not found.");
            }
            else
            {
                return _mapper.Map<List<ReservationDto>>(reservations);
            }
        }
        public async Task<ReservationDto> ReservationInfoByIDAsync(int reservationID)
        {
            var reservation = await _context.Reservations.FirstOrDefaultAsync(r => r.Id == reservationID);
            if (reservation == null)
            {
                throw new Exception("Reservation not found.");
            }
            else
            {
                return _mapper.Map<ReservationDto>(reservation);
            }
        }
        public async Task<bool> ReservationRequestAcceptAsync(int userID, int reservationID)
        {
            var user = await _context.Users
                .Include(u => u.Roles)
                .FirstOrDefaultAsync(u => u.Id == userID);

            if (user == null)
            {
                throw new Exception("User not found.");
            }

            var hasPermission = user.Roles.Any(r => r.Name == "Admin" || r.Name == "Receptionist");
            if (!hasPermission)
            {
                throw new Exception("Permission denied!");
            }

            var reservation = await _context.Reservations
                .FirstOrDefaultAsync(r => r.Id == reservationID);

            if (reservation == null)
            {
                throw new Exception("Reservation not found.");
            }
            if (reservation.Status != "Requested")
            {
                throw new Exception("Only reservations with 'Requested' status can be accepted.");
            }
            reservation.Status = "Accepted";
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> ReservationRequestDenyAsync(int userID, int reservationID)
        {
            var user = await _context.Users
                .Include(u => u.Roles)
                .FirstOrDefaultAsync(u => u.Id == userID);
            if (user == null)
            {
                throw new Exception("User not found.");
            }
            var hasPermission = user.Roles.Any(r => r.Name == "Admin" || r.Name == "Receptionist");
            if (!hasPermission)
            {
                throw new Exception("Permission denied!");
            }
            var reservation = await _context.Reservations
                .FirstOrDefaultAsync(r => r.Id == reservationID);
            if (reservation == null)
            {
                throw new Exception("Reservation not found.");
            }
            if (reservation.Status != "Requested")
            {
                throw new Exception("Only reservations with 'Requested' status can be denied.");
            }
            reservation.Status = "Denied";
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<ReservationDto> ReservationCheckInAsync(ReservationCheckInDto dto)
        {
            var reservation = await _context.Reservations.FindAsync(dto.ReservationId);

            if (reservation == null) throw new Exception("Reservation not found.");

            if (reservation.Status != "Accepted")
                throw new Exception("Only accepted reservations can check-in.");

            reservation.Status = "CheckedIn";
            reservation.CheckInDate = DateTime.Now;

            await _context.SaveChangesAsync();
            return _mapper.Map<ReservationDto>(reservation);
        }

        public async Task<ReservationDto> ReservationCheckOutAsync(ReservationCheckOutDto dto)
        {
            var reservation = await _context.Reservations.FindAsync(dto.ReservationId);

            if (reservation == null) throw new Exception("Reservation not found.");

            if (reservation.Status != "CheckedIn")
                throw new Exception("Guest must be checked-in before checking-out.");

            reservation.Status = "CheckedOut";
            reservation.CheckOutDate = DateTime.Now;

            await _context.SaveChangesAsync();
            return _mapper.Map<ReservationDto>(reservation);
        }

        public async Task<InvoiceDto> GenerateInvoiceAsync(int reservationId, int employeeId)
        {
            // 1. Foglalás betöltése az összes szükséges kapcsolódó adattal
            var reservation = await _context.Reservations
                .Include(r => r.Room)
                .Include(r => r.ReservationBenefits)
                    .ThenInclude(rb => rb.Benefit)
                .FirstOrDefaultAsync(r => r.Id == reservationId);

            if (reservation == null) throw new Exception("Foglalás nem található.");

            // 2. Szoba árának kiszámítása (FromDate és ToDate közötti éjszakák)
            // Megjegyzés: A .Days kiszámolja a különbséget
            var nights = (reservation.ToDate - reservation.FromDate).Days;
            if (nights <= 0) nights = 1; // Ha aznap távozik, akkor is 1 éjszaka

            decimal roomTotal = (decimal)(nights * reservation.Room.PricePerNight);

            // 3. Szolgáltatások (Benefits) árának kiszámítása
            // Itt a Quantity-t és a Service.Price-t szorozzuk össze
            decimal serviceTotal = reservation.ReservationBenefits
                .Sum(rb => rb.Quantity * rb.Benefit.Price);

            // 4. Az új Invoice objektum összeállítása a te entitásod alapján
            var invoice = new Invoice
            {
                ReservationId = reservationId,
                RoomTotal = roomTotal,
                ServiceTotal = serviceTotal,
                GrandTotal = roomTotal + serviceTotal,
                IssuedBy = employeeId, // Az alkalmazott ID-ja, aki generálja
                IssuedAt = DateTime.Now
            };

            _context.Invoices.Add(invoice);
            await _context.SaveChangesAsync();

            // 5. Visszaadjuk a mappelt DTO-t
            return _mapper.Map<InvoiceDto>(invoice);
        }
    }
}
