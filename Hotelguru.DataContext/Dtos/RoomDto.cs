using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hotelguru.DataContext.Entities;

namespace Hotelguru.DataContext.Dtos
{
    public class RoomDto
    {
        public int Id { get; set; }
        public decimal? PricePerNight { get; set; }
        public int RoomTypeId { get; set; }
        public ICollection<Facility> Facilities { get; set; } = new List<Facility>();
    }

    public class RoomCreateDto
    {
        [Required]
        public int RoomTypeId { get; set; }
    }

}
