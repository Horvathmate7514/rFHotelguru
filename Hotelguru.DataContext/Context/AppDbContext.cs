using Hotelguru.DataContext.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotelguru.DataContext.Context
{
    public class AppDbContext : DbContext
    {


        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Room> Rooms { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<ReservationBenefit> ReservationServices { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<RoomType> RoomTypes { get; set; }
        public DbSet<Facility> Facilities { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Role> Roles { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

         
            modelBuilder.Entity<User>()
                .HasOne(u => u.Address)      
                .WithOne(a => a.User)         
                .HasForeignKey<User>(u => u.AddressId);
        }



    }
}
