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
                && (r.Status != "Cancelled" || r.Status != "CheckedOut") //A foglalás státusza nem "Cancelled" vagy "CheckedOut"
                && (r.ToDate <= dto.FromDate || r.FromDate >= dto.ToDate))) //A foglalás időszaka nem fed át a foglalási kérelemben megadott időszakkal
            {//Csak akkor lehet foglalni, ha nincs már foglalás az adott szobára ugyanarra az időszakra ami nem "Cancelled" vagy "CheckedOut" státuszú
                var reservation = //_mapper.Map<DataContext.Entities.Reservation>(dto);
                new DataContext.Entities.Reservation
                {
                    UserId = dto.GuestId,
                    RoomId = dto.RoomId,
                    FromDate = dto.FromDate,
                    ToDate = dto.ToDate,
                    CancellationDeadline = dto.FromDate.AddDays(-1), //Lemondási határidő a foglalás kezdete előtt 1 nappal
                                                                     //-(nincs pontos leírás hogy mennyi kell legyen!!!)
                    Status = "Requested",
                    ReservationBenefits = new List<ReservationBenefit>()
                };
                foreach (var serviceDto in dto.ReservationBenefits)
                {
                    var reservationService = new ReservationBenefit
                    {
                        ServiceId = serviceDto.ServiceId,
                        Quantity = serviceDto.Quantity
                    };
                }
                _context.Reservations.Add(reservation);
                await _context.SaveChangesAsync();
                return _mapper.Map<ReservationDto>(reservation);
            }
            else
            {
                throw new Exception("Ez a szoba már foglalt!");
            }
        }
        public async Task<ReservationDto> ReservationCancelAsync(ReservationCancelDto dto)
        {
            var reservation = await _context.Reservations
                .FirstOrDefaultAsync(r => r.Id == dto.ReservationId && r.UserId == dto.GuestId);
            if (reservation == null)
            {
                throw new Exception("A foglalás nem található.");
            }
            if (reservation.CancellationDeadline < DateTime.Now)
            {
                throw new Exception("A foglalás lemondási határideje lejárt.");
            }
            reservation.Status = "Cancelled";
            await _context.SaveChangesAsync();
            return _mapper.Map<ReservationDto>(reservation);
        }
    }
}
