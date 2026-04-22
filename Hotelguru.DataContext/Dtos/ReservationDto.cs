using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotelguru.DataContext.Dtos
{
    public class ReservationDto
    {
        public int Id { get; set; }
        public int RoomId { get; set; }
        public int UserId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public string Status { get; set; }
        public List<ReservationBenefitCreateDto> ReservationBenefits { get; set; }
    }

    public class ReservationCreateDto
    {
        public int RoomId { get; set; }
        public int UserId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public List<ReservationBenefitCreateDto> ReservationBenefits { get; set; }
    }

    public class ReservationBenefitCreateDto
    {
        public int BenefitId { get; set; }
        public int Quantity { get; set; }
    }

    public class ReservationCancelDto
    {
        public int ReservationId { get; set; }
        public int UserId { get; set; }
    }

    public class ReservationCheckInDto
    {
        public int ReservationId { get; set; }
    }

    public class ReservationCheckOutDto
    {
        public int ReservationId { get; set; }
    }
}
