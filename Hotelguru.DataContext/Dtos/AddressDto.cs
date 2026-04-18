using System.ComponentModel.DataAnnotations;

namespace Hotelguru.DataContext.Dtos
{
    public class AddressDto
    {
        public int Id { get; set; }
        [Required] public string PostCode { get; set; }
        [Required] public string Country { get; set; }
        [Required] public string City { get; set; }
        [Required] public string Street { get; set; }
        [Required] public string HouseNumber { get; set; }
    }

    public class AddressCreateDto
    {
        
        [Required] public string PostCode { get; set; }
        [Required] public string Country { get; set; }
        [Required] public string City { get; set; }
        [Required] public string Street { get; set; }
        [Required] public string HouseNumber { get; set; }
    }


    public class AddressUpdateDto
    {
       
        public string? PostCode { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
        public string? Street { get; set; }
        public string? HouseNumber { get; set; }
    }
}