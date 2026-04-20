using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotelguru.DataContext.Entities
{
    public class Room
    {
        public int  Id { get; set; }

        public RoomType RoomType { get; set; }
        [Precision(18, 2)]
        public decimal PricePerNight { get; set; }

        public ICollection<Facility> roomFacilities { get; set; } = new List<Facility>();



    }

   
}
