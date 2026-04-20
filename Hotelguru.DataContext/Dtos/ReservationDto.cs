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
        public int GuestId { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
    }

    public class ReservationCreateDto
    {
        public int RoomId { get; set; }
        public int GuestId { get; set; }
        public List<ReservationServiceCreateDto> ReservationServices { get; set; }
    }

    public class ReservationServiceCreateDto
    {
        public int ServiceId { get; set; }
        public int Quantity { get; set; }
    }
}
