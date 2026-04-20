using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotelguru.DataContext.Dtos
{
    public class RoomDto
    {
        public int Id { get; set; }
        public decimal? PricePerNight { get; set; }
        public RoomTypeDto RoomType { get; set; }
        public List<FacilityDto> Facilities { get; set; }
    }
}
