using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bean_Scene_Reservation.Tests.Infrastructure {
    /// <summary>
    /// Base class for all controller unit tests in Bean-Scene-Reservation.
    /// Constructor creates an in-memory database with seeded data to test individual controller methods.
    /// </summary>
    public class BeanSceneTestBase : IDisposable {

        protected readonly ApplicationDbContext _context;

        public BeanSceneTestBase() {
            // DB context options - use in-memory database
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            // Store DB context
            _context = new ApplicationDbContext(options);

            // Cleanup
            _context.Database.EnsureCreated();

            // Exit if DB already has data
            if (_context.Areas.Any()) return;

            // Seed data
            SeedAreaData();
            SeedTableData();

        }

        private void SeedAreaData() {
            // Area collection
            var areas = new[] {
                new Area { Id = 1, Name = "Main"},
                new Area { Id = 2, Name = "Outside"},
                new Area { Id = 3, Name = "Balcony"},
            };

            _context.Areas.AddRange(areas);
            _context.SaveChanges();
        }

        private void SeedTableData() {
            // Table collection
            var tables = new[] {
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
                new Table { TableNumber = "B10", AreaId = 3 },
            };

            _context.Tables.AddRange(tables);
            _context.SaveChanges();
        }

        /// <summary>
        /// Cleans up resources used by each unit test.
        /// </summary>
        public void Dispose() {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
