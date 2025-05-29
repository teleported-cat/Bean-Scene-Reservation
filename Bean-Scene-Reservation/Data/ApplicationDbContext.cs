using Bean_Scene_Reservation.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Bean_Scene_Reservation.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        // Initialise the DbSets of all entities
        public DbSet<Area> Areas { get; set; }
        public DbSet<Table> Tables { get; set; }
        public DbSet<Timeslot> Timeslots { get; set; }
        public DbSet<SittingType> SittingTypes { get; set; }
        public DbSet<Sitting> Sittings { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // In here, we can add custom configuration for our entities, their relationships, and seed data

            // Seed Data for Entities
            // Seeded data is automatically inserted into the database on creation
            
            // Seed Data for Area
            builder.Entity<Area>().HasData
            (
                new Area { Id = 1, Name = "Main"},
                new Area { Id = 2, Name = "Outside" },
                new Area { Id = 3, Name = "Balcony" }
            );

            // Seed Data for Table
            builder.Entity<Table>().HasData
            (
                new Table { TableNumber = "M1", AreaId = 1 },
                new Table { TableNumber = "M2", AreaId = 1 },
                new Table { TableNumber = "M3", AreaId = 1 },
                new Table { TableNumber = "M4", AreaId = 1 },
                new Table { TableNumber = "M5", AreaId = 1 },
                new Table { TableNumber = "M6", AreaId = 1 },
                new Table { TableNumber = "M7", AreaId = 1 },
                new Table { TableNumber = "M8", AreaId = 1 },
                new Table { TableNumber = "M9", AreaId = 1 },
                new Table { TableNumber = "M10", AreaId = 1 },

                new Table { TableNumber = "O1", AreaId = 2 },
                new Table { TableNumber = "O2", AreaId = 2 },
                new Table { TableNumber = "O3", AreaId = 2 },
                new Table { TableNumber = "O4", AreaId = 2 },
                new Table { TableNumber = "O5", AreaId = 2 },
                new Table { TableNumber = "O6", AreaId = 2 },
                new Table { TableNumber = "O7", AreaId = 2 },
                new Table { TableNumber = "O8", AreaId = 2 },
                new Table { TableNumber = "O9", AreaId = 2 },
                new Table { TableNumber = "O10", AreaId = 2 },

                new Table { TableNumber = "B1", AreaId = 3 },
                new Table { TableNumber = "B2", AreaId = 3 },
                new Table { TableNumber = "B3", AreaId = 3 },
                new Table { TableNumber = "B4", AreaId = 3 },
                new Table { TableNumber = "B5", AreaId = 3 },
                new Table { TableNumber = "B6", AreaId = 3 },
                new Table { TableNumber = "B7", AreaId = 3 },
                new Table { TableNumber = "B8", AreaId = 3 },
                new Table { TableNumber = "B9", AreaId = 3 },
                new Table { TableNumber = "B10", AreaId = 3 }
            );

            /* 
             * Seeding data for timeslots
             */
            //// We are going to create the timeslots through code
            //// List to all timeslots
            var timeslotsToAdd = new List<Timeslot>();

            //// Earliest time is 0800, latest is 2200
            var startTime = new TimeOnly(8, 0);
            var endTime = new TimeOnly(22, 0);

            //// Loop through all possible 30-minute increments in range of the start and end times
            for (int minutes = 0; startTime.AddMinutes(minutes) <= endTime; minutes += 30)
            {
                timeslotsToAdd.Add(new Timeslot { Time = startTime.AddMinutes(minutes) });
            }

            //// Add list to EF collection
            builder.Entity<Timeslot>().HasData(timeslotsToAdd);

            // Seed Data for SittingType
            builder.Entity<SittingType>().HasData
            (
                new SittingType { Id = 1, Name = "Breakfast" },
                new SittingType { Id = 2, Name = "Lunch" },
                new SittingType { Id = 3, Name = "Dinner" }
            );

            // Pass customisations through to base DbContext
            base.OnModelCreating(builder);
        }
    }
}
