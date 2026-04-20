using System.ComponentModel.DataAnnotations;

namespace Hotelguru.DataContext.Dtos
{
    public class RoomTypeDto
    {
        public int Id { get; set; }
        public int BedNumber { get; set; }
        public int Capacity { get; set; }
        public decimal BasePrice { get; set; }
    }

    public class RoomTypeCreateDto
    {
        [Required]
        public int BedNumber { get; set; }
        [Required]
        public int Capacity { get; set; }
        [Required]
        public decimal BasePrice { get; set; }
    }
    public class RoomTypeUpdateDto
    {
        [Required]
        public int BedNumber { get; set; }

        [Required]
        public int Capacity { get; set; }

        [Required]
        public decimal BasePrice { get; set; }
    }
}