using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotelguru.DataContext.Dtos
{
    public class UserDto
    {
        
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public AddressDto Address { get; set; }
        public List<RoleDto> Roles { get; set; } = new List<RoleDto>();
    
    }

    public class UserRegisterDto
    {
        [Required]
        [StringLength(50)]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        [Phone]
        public string Mobile { get; set; }

        public AddressCreateDto Address { get; set; }

    }

    public class UserLoginDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }


    
    public class UserUpdateDto
    {
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Mobile { get; set; }
        public AddressUpdateDto? Address { get; set; }
    }


}
