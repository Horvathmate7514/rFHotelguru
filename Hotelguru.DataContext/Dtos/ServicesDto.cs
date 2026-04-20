using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotelguru.DataContext.Dtos
{
    public class ServicesDto
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public decimal Price { get; set; }
    }

    public class ServicesCreateDto
    {
        [Required]
        public string Type { get; set; }
        [Required]
        public decimal Price { get; set; }
    }

    public class ServicesUpdateDto
    {
        [Required]
        public string Type { get; set; }
        [Required]
        public decimal Price { get; set; }
    }
}
