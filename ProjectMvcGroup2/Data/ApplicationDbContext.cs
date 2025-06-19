using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using ProjectLibraryGroup2;

namespace ProjectMvcGroup2.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public DbSet<Guest> Guest { get; set; }
        //public DbSet<Student> Student { get; set; }
        public DbSet<Lodging> Lodging { get; set; }
        public DbSet<LodgingDates> LodgingDates { get; set; }
        public DbSet<LiftTicket> LiftTicket { get; set; }
        public DbSet<TicketDate> TicketDate { get; set; }
        public DbSet<EquipmentRental> EquipmentRental { get; set; }
        public DbSet<Rents> Rents { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
  
        }
    }
}
