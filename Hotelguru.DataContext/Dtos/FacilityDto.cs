using System.ComponentModel.DataAnnotations;

namespace Hotelguru.DataContext.Dtos
{
    public class FacilityDto
    {
        public int Id { get; set; }
        public string FacilityName { get; set; }
        public int Price { get; set; }
    }

    public class FacilityCreateDto
    {
        [Required]
        public string FacilityName { get; set; }
        [Required]
        public int Price { get; set; }
    }

    public class FacilityUpdateDto
    {
        [Required]
        public string FacilityName { get; set; }
        [Required]
        public int Price { get; set; }
    }
}