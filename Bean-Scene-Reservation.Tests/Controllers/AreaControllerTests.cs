using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bean_Scene_Reservation.Tests.Controllers {
    public class AreaControllerTests : BeanSceneTestBase {

        #region Index
        [Fact]
        public async void AreasController_Index_ReturnsView()
        {
            // Arrange
            var controller = new AreasController(_context);

            // Act
            var result = await controller.Index();

            // Assert
            // Makes sure it returns a view
            Assert.IsType<ViewResult>(result);

        }
        #endregion

        #region Create
        [Fact]
        public async void AreasController_Create_Success_ReturnsRedirect()
        {
            // Arrange
            var controller = new AreasController(_context);
            var newArea = new Area { Id = 4, Name = "New Area" };

            // Act 
            var result = await controller.Create(newArea);

            // Assert
            // Should be redirected to the index page of the Area controller
            // (controllerName = null, controllerAction = "Index")
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Null(redirectToActionResult.ControllerName);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }

        [Fact]
        public async void AreasController_Create_InvalidName_ReturnsView()
        {
            // Arrange
            var controller = new AreasController(_context);
            var newArea = new Area { Id = 4, Name = "An extremely long name that won't ever be considered valid. Five Four Three Two One" };

            // Act 
            var result = await controller.Create(newArea);

            // Assert
            // Should be stay on the create page
            var viewResult = Assert.IsType<ViewResult>(result);
        }
        #endregion

        #region Edit
        [Fact]
        public async void AreasController_Edit_Success_ReturnsRedirect()
        {
            // Arrange
            var controller = new AreasController(_context);
            const int AREA_ID = 1;
            var newArea = new Area { Id = AREA_ID, Name = "New Main" };

            // Act 
            var result = await controller.Edit(AREA_ID, newArea);

            // Assert
            // Should be redirected to the index page of the Area controller
            // (controllerName = null, controllerAction = "Index")
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Null(redirectToActionResult.ControllerName);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }

        [Fact]
        public async void AreasController_Edit_InvalidId_ReturnsNotFound()
        {
            // Arrange
            var controller = new AreasController(_context);
            const int AREA_ID = 10;
            var newArea = new Area { Id = AREA_ID, Name = "New Main" };

            // Act 
            var result = await controller.Edit(AREA_ID, newArea);

            // Assert
            var redirectToActionResult = Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async void AreasController_Edit_InvalidName_ReturnsView()
        {
            // Arrange
            var controller = new AreasController(_context);
            const int AREA_ID = 1;
            var newArea = new Area { Id = AREA_ID, Name = "An extremely long name that won't ever be considered valid. Five Four Three Two One" };

            // Act 
            var result = await controller.Edit(AREA_ID, newArea);

            // Assert
            // Should be stay on the edit page
            var viewResult = Assert.IsType<ViewResult>(result);
        }
        #endregion

        #region Delete
        [Fact]
        public async void AreasController_Delete_Success_ReturnsRedirect()
        {
            // Arrange
            var controller = new AreasController(_context);
            const int AREA_ID = 1;

            // Act 
            var result = await controller.DeleteConfirmed(AREA_ID);

            // Assert
            // Should be redirected to the index page of the Area controller
            // (controllerName = null, controllerAction = "Index")
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
        }
        #endregion
    }
}
