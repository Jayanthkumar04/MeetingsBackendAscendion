using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MeetingsBackendAscendion.Models.Domain;

namespace MeetingsBackendAscendion.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>  // Inherit from IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Add DbSet properties for your models
        public DbSet<Meeting> Meetings { get; set; }
        public DbSet<MeetingAttendee> MeetingAttendees { get; set; }

        // Optional: Configure models using Fluent API
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // Include the Identity table configurations

            // Fluent API configuration (optional)
            modelBuilder.Entity<MeetingAttendee>()
                .HasKey(ma => new { ma.MeetingId, ma.Id }); // Composite key for MeetingAttendee table

            // You can add additional configurations for the Meeting and MeetingAttendee entities here if needed
        }
    }
}
