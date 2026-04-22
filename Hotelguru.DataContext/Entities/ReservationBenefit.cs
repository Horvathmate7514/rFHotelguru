using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotelguru.DataContext.Entities
{
    public class ReservationBenefit
    {
        public int Id { get; set; }
        public int ReservationId { get; set; }
        public Reservation? Reservation { get; set; }

        public int Quantity { get; set; }
        public int BenefitId { get; set; }
        public Benefit? Benefit { get; set; }
        [Precision(18, 2)]
        public decimal ChargedPrice { get; set; }

        public DateTime OrderedAt { get; set; }
    }
}
