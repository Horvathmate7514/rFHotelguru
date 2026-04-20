using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotelguru.DataContext.Entities
{
    public class Service
    {
        public int Id { get; set; }

        public string Type { get; set; }
        [Precision(18, 2)]
        public decimal Price { get; set; }
    }
}
