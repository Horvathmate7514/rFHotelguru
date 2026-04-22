using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotelguru.DataContext.Dtos
{
    public class InvoiceDto
    {
        public int Id { get; set; }
        public int ReservationId { get; set; }
        public decimal RoomTotal { get; set; }
        public decimal ServiceTotal { get; set; }
        public decimal GrandTotal { get; set; }
        public DateTime IssuedAt { get; set; }
    }
}
