using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bean_Scene_Reservation.Tests.Controllers {
    public class TableControllerTests : BeanSceneTestBase {

        #region Index
        // Unit_Test_Method naming convention: class_method_other-info_expected-result()
        [Fact]
        public async void TablesController_Index_ReturnsView() {
            // Arrange
            var controller = new TablesController(_context);

            // Act
            var result = await controller.Index();

            // Assert
            // Makes sure it returns a view
            Assert.IsType<ViewResult>(result);

        }
        #endregion

        #region Details
        [Fact]
        public async void TableController_Details_ReturnsView() {
            // Arrange
            var controller = new TablesController(_context);
            const string TABLE_NUMBER = "M1";

            // Act
            var result = await controller.Details(TABLE_NUMBER);

            // Assert
            Assert.IsType<ViewResult>(result);

        }

        [Fact]
        public async void TableController_Detail_NoTableNum_ReturnsNotFound() {
            // Arrange
            var controller = new TablesController(_context);

            // Act
            var result = await controller.Details(null!);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async void TableController_Detail_NonExistingTable_ReturnsNotFound() {
            // Arrange
            var controller = new TablesController(_context);
            const string TABLE_NUMBER = "X0";

            // Act
            var result = await controller.Details(TABLE_NUMBER);

            // Assert
            Assert.IsType<NotFoundObjectResult>(result);
        }
        #endregion

        #region Create
        [Fact]
        public async void TablesController_Create_Success_ReturnsRedirect() {
            // Arrange
            var controller = new TablesController(_context);
            var newTable = new Table { TableNumber = "M11", AreaId = 1};

            // Act 
            var result = await controller.Create(newTable);

            // Assert
            // Should be redirected to the index page of the Table controller
            // (controllerName = null, controllerAction = "Index")
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Null(redirectToActionResult.ControllerName);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }

        [Fact]
        public async void TablesController_Create_InvalidArea_ReturnsNotFound() {
            // Arrange
            var controller = new TablesController(_context);
            // Invalid table: bad area id
            var newTable = new Table { TableNumber = "M11", AreaId = 0 };

            // Act 
            var result = await controller.Create(newTable);

            // Assert
            // Should be redirected to a not found page
            var viewResult = Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async void TablesController_Create_InvalidTable_ReturnsView() {
            // Arrange
            var controller = new TablesController(_context);
            // Invalid table: bad table number
            var newTable = new Table { TableNumber = "I LOVE ASP.NET!!!", AreaId = 1 };

            // Act 
            var result = await controller.Create(newTable);

            // Assert - reload of the create view 
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Null(viewResult.ViewName);

        }
        #endregion

    }
}
