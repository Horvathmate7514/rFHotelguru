using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotelguru.DataContext.Entities
{
    public class RoomType
    {
        public int Id { get; set; }
        public int BedNumber { get; set; }
        public int Capacity { get; set; }
        public decimal BasePrice { get; set; }
    }
}
