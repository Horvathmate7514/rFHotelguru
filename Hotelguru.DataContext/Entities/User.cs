using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotelguru.DataContext.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public int? AddressId { get; set; } 
        public Address Address { get; set; }
        public string Password { get; set; }

        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();

        public List<Role> Roles  { get; set; }

    }
   
}
