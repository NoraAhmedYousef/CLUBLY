using Clubly.Model;
using SignUp.Model;


namespace SignUp.Data
{
    using Microsoft.EntityFrameworkCore;

    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // ===== TABLES =====
        public DbSet<User> Users1 { get; set; }
        public DbSet<PasswordResetToken> PasswordResetTokens { get; set; }
        public DbSet<ContactMessage> ContactMessages { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Trainer> Trainers { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<Facility> Facilities { get; set; }
        public DbSet<FacilityCategory> FacilityCategories { get; set; }
        public DbSet<MemberShip> MemberShips { get; set; }
        public DbSet<ActivityGroup> ActivityGroups { get; set; }
        public DbSet<Announcement> Announcements { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<FacilitySchedule> FacilitySchedules { get; set; }
        public DbSet<FacilityTimeSlot> FacilityTimeSlots { get; set; }
        public DbSet<FacilityBooking> FacilityBookings { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);


            builder.Entity<FacilitySchedule>()
                .HasMany(s => s.TimeSlots)
                .WithOne(t => t.Schedule)
                .HasForeignKey(t => t.FacilityScheduleId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<FacilitySchedule>()
                .HasOne(s => s.Facility)
                .WithMany(f => f.Schedules)
                .HasForeignKey(s => s.FacilityId)
                .OnDelete(DeleteBehavior.Restrict);


            builder.Entity<FacilityBooking>()
      .HasOne(b => b.Facility)
      .WithMany(f => f.Bookings)
      .HasForeignKey(b => b.FacilityId)
      .OnDelete(DeleteBehavior.Restrict);   // مهم جداً

            builder.Entity<FacilityBooking>()
                .HasOne(b => b.Schedule)
                .WithMany()
                .HasForeignKey(b => b.FacilityScheduleId)
                .OnDelete(DeleteBehavior.Restrict);   // مهم جداً

            builder.Entity<ActivityGroup>()
                .HasOne(a => a.Activity)
                .WithMany()
                .HasForeignKey(a => a.ActivityId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}