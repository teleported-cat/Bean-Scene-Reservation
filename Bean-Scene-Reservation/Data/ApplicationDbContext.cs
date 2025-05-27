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

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // In here, we can add custom configuration for our entities, their relationships, and seed data

            // Seed Data for Entities
            // Seeded data is automatically inserted into the database on creation
            /*
            builder.Entity<Area>().HasData
            (
                new Area { Id = 1, Name = "Main"},
                new Area { Id = 2, Name = "Outside" },
                new Area { Id = 3, Name = "Balcony" }
            );

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
            */
            // Pass customisations through to base DbContext
            base.OnModelCreating(builder);
        }
    }
}
