using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;


namespace RGR_BD
{
    public class AppDbContext : DbContext
    {
        public DbSet<Location> Locations { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Bookings> Bookings { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");
            optionsBuilder.UseNpgsql(connectionString);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Bookings
            modelBuilder.Entity<Bookings>(entity =>
            {
                entity.Property(e => e.BookingId).HasColumnOrder(1);
                entity.Property(e => e.UserId).HasColumnOrder(2);
                entity.Property(e => e.SessionId).HasColumnOrder(3);
                entity.Property(e => e.BookingDate).HasColumnOrder(4);
                entity.Property(e => e.Status).HasColumnOrder(5);
            });

            // Session
            modelBuilder.Entity<Session>(entity =>
            {
                entity.Property(e => e.SessionId).HasColumnOrder(1);
                entity.Property(e => e.InstructorId).HasColumnOrder(2);
                entity.Property(e => e.LocationId).HasColumnOrder(3);
                entity.Property(e => e.StartTime).HasColumnOrder(4);
                entity.Property(e => e.EndTime).HasColumnOrder(5);
                entity.Property(e => e.MaxParticipants).HasColumnOrder(6);
                entity.Property(e => e.PriceMoney).HasColumnOrder(7);
            });

            // Instructor
            modelBuilder.Entity<Instructor>(entity =>
            {
                entity.Property(e => e.InstructorId).HasColumnOrder(1);
                entity.Property(e => e.ExperienceYears).HasColumnOrder(2);
                entity.Property(e => e.Bio).HasColumnOrder(3);
                entity.Property(e => e.Rating).HasColumnOrder(4);
            });

            // Location
            modelBuilder.Entity<Location>(entity =>
            {
                entity.Property(e => e.LocationId).HasColumnOrder(1);
                entity.Property(e => e.Name).HasColumnOrder(2);
                entity.Property(e => e.Address).HasColumnOrder(3);
                entity.Property(e => e.City).HasColumnOrder(4);
                entity.Property(e => e.Capacity).HasColumnOrder(5);
            });

            // Client
            modelBuilder.Entity<Client>(entity =>
            {
                entity.Property(e => e.UserId).HasColumnOrder(1);
                entity.Property(e => e.FirstName).HasColumnOrder(2);
                entity.Property(e => e.LastName).HasColumnOrder(3);
                entity.Property(e => e.Email).HasColumnOrder(4);
                entity.Property(e => e.PhoneNumber).HasColumnOrder(5);
                entity.Property(e => e.DateOfRegistration).HasColumnOrder(6);
            });

            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Session>()
                .HasOne(s => s.Instructor)
                .WithMany(i => i.Sessions)
                .HasForeignKey(s => s.InstructorId);

            modelBuilder.Entity<Session>()
                .HasOne(s => s.Location)
                .WithMany(l => l.Sessions)
                .HasForeignKey(s => s.LocationId);

            modelBuilder.Entity<Bookings>()
                .HasOne(b => b.Client)
                .WithMany(c => c.Bookings)
                .HasForeignKey(b => b.UserId);

            modelBuilder.Entity<Bookings>()
                .HasOne(b => b.Session)
                .WithMany(s => s.Bookings)
                .HasForeignKey(b => b.SessionId);
        }
    }
}