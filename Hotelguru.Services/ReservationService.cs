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
            if (await _context.Reservations.AnyAsync(r => r.RoomId == dto.RoomId))
            {
                // Handle the case where a reservation for this room already exists
                throw new InvalidOperationException("A reservation for this room already exists.");
            }
            var reservation = //_mapper.Map<DataContext.Entities.Reservation>(dto);
                new DataContext.Entities.Reservation
                {
                    UserId = dto.GuestId,
                    RoomId = dto.RoomId,
                    Status = "Requested",
                    ReservationBenefits = new List<ReservationBenefit>()
                };
            foreach(var serviceDto in dto.ReservationServices)
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
    }
}
