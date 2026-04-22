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
        public List<RoomFacilityDto> RoomFacilities { get; set; }
    }

    public class RoomCreateDto
    {
        public int RoomTypeId { get; set; }
    }
    public class RoomUpdateDto
    {
        public decimal? PricePerNight { get; set; }
        public int RoomTypeId { get; set; }

    }
    public class RoomFacilityDto
    {
        public int RoomId { get; set; }
        public int FacilityId { get; set; }
    }
    public class RoomFacilityCreateDto
    {
        public int RoomId { get; set; }
        public int FacilityId { get; set; }
    }
}
