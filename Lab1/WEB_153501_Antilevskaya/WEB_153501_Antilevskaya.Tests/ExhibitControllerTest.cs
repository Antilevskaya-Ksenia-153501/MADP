using Moq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WEB_153501_Antilevskaya.Controllers;
using WEB_153501_Antilevskaya.Domain.Entities;
using WEB_153501_Antilevskaya.Domain.Models;
using WEB_153501_Antilevskaya.Services.CategoryService;
using WEB_153501_Antilevskaya.Services.ExhibitService;

namespace WEB_153501_Antilevskaya.Tests
{
    public class ExhibitControllerTest
    {
        [Fact]
        public void Index_ReturnsNotFound_WhenCategoryServiceReturnsUnsuccessfulResponse()
        {
            Mock<ICategoryService> categoryService = new Mock<ICategoryService>();
            categoryService.Setup(mock => mock.GetCategoryListAsync()).ReturnsAsync(new ResponseData<List<Category>>()
            {
                Success = false,
                ErrorMessage = "������ �� �������� �� �������. Error."
            });

            Mock<IExhibitService> exhibitService = new Mock<IExhibitService>();
            exhibitService.Setup(mock => mock.GetExhibitListAsync(null, 1)).ReturnsAsync(new ResponseData<ListModel<Exhibit>> { Success = true });

            //�������� �����������
            var controllerContext = new ControllerContext();
            //����� HttpContext
            var moqHttpContext = new Mock<HttpContext>();
            moqHttpContext.Setup(c => c.Request.Headers).Returns(new HeaderDictionary());
            controllerContext.HttpContext = moqHttpContext.Object;
            var controller = new ExhibitController(categoryService.Object, exhibitService.Object) { ControllerContext = controllerContext };

            var result = controller.Index(null, 1).Result;
            Assert.NotNull(result);
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
        }

        [Fact]
        public void Index_ReturnsNotFound_WhenExhibitServiceReturnsUnsuccessfulResponse()
        {
            Mock<ICategoryService> categoryService = new Mock<ICategoryService>();
            categoryService.Setup(mock => mock.GetCategoryListAsync()).ReturnsAsync(new ResponseData<List<Category>> { Success = true });

            Mock<IExhibitService> exhibitService = new Mock<IExhibitService>();
            exhibitService.Setup(mock => mock.GetExhibitListAsync(null, 1)).ReturnsAsync(new ResponseData<ListModel<Exhibit>> 
            { 
                Success = false,
                ErrorMessage = "������ �� �������� �� �������. Error."
            });

            var controllerContext = new ControllerContext();
            var moqHttpContext = new Mock<HttpContext>();
            moqHttpContext.Setup(c => c.Request.Headers).Returns(new HeaderDictionary());
            controllerContext.HttpContext = moqHttpContext.Object;
            var controller = new ExhibitController(categoryService.Object, exhibitService.Object) { ControllerContext= controllerContext };

            var result = controller.Index(null, 1).Result;
            Assert.NotNull(result);
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
        }

        [Fact]
        public void Index_ReturnsViewBagWithCategories()
        {
            List<Category> categories = new List<Category>()
            {
                new Category { Id = 1, Name = "Category1", NormalizedName = "category1" },
                new Category { Id = 2, Name = "Category2", NormalizedName = "category2" },
                new Category { Id = 3, Name = "Category3", NormalizedName = "category3" }
            };

            ListModel<Exhibit> exhibits = new ListModel<Exhibit>()
            {
                Items = new List<Exhibit>() 
                { 
                    new Exhibit { Id = 1, Title = "Exhibit1", Description = "", CategoryId = 1, Price = 10 },
                    new Exhibit { Id = 2, Title = "Exhibit2", Description = "", CategoryId = 2, Price = 20 },
                    new Exhibit { Id = 3, Title = "Exhibit3", Description = "", CategoryId = 3, Price = 30 },
                },
                CurrentPage = 1,
                TotalPages = 1
            }; 
            Mock<ICategoryService> categoryService = new Mock<ICategoryService>();
            categoryService.Setup(mock => mock.GetCategoryListAsync()).ReturnsAsync(new ResponseData<List<Category>> 
            { 
                Success = true,
                Data = categories
            });

            Mock<IExhibitService> exhibitService = new Mock<IExhibitService>();
            exhibitService.Setup(mock => mock.GetExhibitListAsync(null, 1)).ReturnsAsync(new ResponseData<ListModel<Exhibit>>
            {
                Success = true,
                Data = exhibits
            }); 

            var controllerContext = new ControllerContext();
            var moqHttpContext = new Mock<HttpContext>();
            moqHttpContext.Setup(c => c.Request.Headers).Returns(new HeaderDictionary());
            controllerContext.HttpContext = moqHttpContext.Object;
            var controller = new ExhibitController(categoryService.Object, exhibitService.Object) { ControllerContext = controllerContext };

            var result = controller.Index(null, 1).Result;
            Assert.NotNull(result);
            Assert.NotNull(controller.ViewBag.Categories);
            Assert.Equal(categories, controller.ViewBag.Categories);
        }

        [Fact]
        public void Index_ReturnsCurrentCategoryAll_WhenCategoryIsNull()
        {

            List<Category> categories = new List<Category>()
            {
                new Category { Id = 1, Name = "Category1", NormalizedName = "category1" },
                new Category { Id = 2, Name = "Category2", NormalizedName = "category2" },
                new Category { Id = 3, Name = "Category3", NormalizedName = "category3" }
            };

            ListModel<Exhibit> exhibits = new ListModel<Exhibit>()
            {
                Items = new List<Exhibit>()
                {
                    new Exhibit { Id = 1, Title = "Exhibit1", Description = "", CategoryId = 1, Price = 10 },
                    new Exhibit { Id = 2, Title = "Exhibit2", Description = "", CategoryId = 2, Price = 20 },
                    new Exhibit { Id = 3, Title = "Exhibit3", Description = "", CategoryId = 3, Price = 30 },
                },
                CurrentPage = 1,
                TotalPages = 1
            };
            Mock<ICategoryService> categoryService = new Mock<ICategoryService>();
            categoryService.Setup(mock => mock.GetCategoryListAsync()).ReturnsAsync(new ResponseData<List<Category>>
            {
                Success = true,
                Data = categories
            });

            Mock<IExhibitService> exhibitService = new Mock<IExhibitService>();
            exhibitService.Setup(mock => mock.GetExhibitListAsync(null, 1)).ReturnsAsync(new ResponseData<ListModel<Exhibit>>
            {
                Success = true,
                Data = exhibits
            });

            var controllerContext = new ControllerContext();
            var moqHttpContext = new Mock<HttpContext>();
            moqHttpContext.Setup(c => c.Request.Headers).Returns(new HeaderDictionary());
            controllerContext.HttpContext = moqHttpContext.Object;
            var controller = new ExhibitController(categoryService.Object, exhibitService.Object) { ControllerContext = controllerContext };

            var result = controller.Index(null, 1).Result;
            Assert.NotNull(result);
            Assert.NotNull(controller.ViewData["CurrentCategory"]);
            Assert.Equal("All", controller.ViewData["CurrentCategory"]);

        }

        [Fact]
        public void Index_ReturnsValidCurrentCategory_WhenCategoryIsNotNull()
        {

            List<Category> categories = new List<Category>()
            {
                new Category { Id = 1, Name = "Category1", NormalizedName = "category1" },
                new Category { Id = 2, Name = "Category2", NormalizedName = "category2" },
                new Category { Id = 3, Name = "Category3", NormalizedName = "category3" }
            };

            ListModel<Exhibit> exhibits = new ListModel<Exhibit>()
            {
                Items = new List<Exhibit>()
                {
                    new Exhibit { Id = 1, Title = "Exhibit1", Description = "", CategoryId = 1, Price = 10 },
                    new Exhibit { Id = 2, Title = "Exhibit2", Description = "", CategoryId = 2, Price = 20 },
                    new Exhibit { Id = 3, Title = "Exhibit3", Description = "", CategoryId = 3, Price = 30 },
                },
                CurrentPage = 1,
                TotalPages = 1
            };
            Mock<ICategoryService> categoryService = new Mock<ICategoryService>();
            categoryService.Setup(mock => mock.GetCategoryListAsync()).ReturnsAsync(new ResponseData<List<Category>>
            {
                Success = true,
                Data = categories
            });

            Mock<IExhibitService> exhibitService = new Mock<IExhibitService>();
            exhibitService.Setup(mock => mock.GetExhibitListAsync("Category1", 1)).ReturnsAsync(new ResponseData<ListModel<Exhibit>>
            {
                Success = true,
                Data = exhibits
            });

            var controllerContext = new ControllerContext();
            var moqHttpContext = new Mock<HttpContext>();
            moqHttpContext.Setup(c => c.Request.Headers).Returns(new HeaderDictionary());
            controllerContext.HttpContext = moqHttpContext.Object;
            var controller = new ExhibitController(categoryService.Object, exhibitService.Object) { ControllerContext = controllerContext };

            var result = controller.Index("Category1", 1).Result;
            Assert.NotNull(result);
            Assert.NotNull(controller.ViewData["CurrentCategory"]);
            Assert.Equal("Category1", controller.ViewData["CurrentCategory"]);
        }
    }
}