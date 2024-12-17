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
            base.OnModelCreating(modelBuilder); // Include Identity table configurations

            modelBuilder.Entity<MeetingAttendee>()
                  .HasKey(ma => new { ma.MeetingId, ma.Id });  // Composite key on MeetingId and Id (UserId)

            modelBuilder.Entity<MeetingAttendee>()
                .HasOne(ma => ma.Meeting)   // Each MeetingAttendee is related to one Meeting
                .WithMany(m => m.Attendees) // Each Meeting has many MeetingAttendees
                .HasForeignKey(ma => ma.MeetingId); // The foreign key is MeetingId

            modelBuilder.Entity<Meeting>()
                .ToTable("Meetings"); // Explicitly set the table name for Meeting (optional)

            modelBuilder.Entity<MeetingAttendee>()
                .ToTable("MeetingAttendees"); // Explicitly set the table name for MeetingAttendees (optional)
        }
    }
}