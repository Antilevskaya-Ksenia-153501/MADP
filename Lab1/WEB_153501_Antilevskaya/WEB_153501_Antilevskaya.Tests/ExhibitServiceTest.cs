using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using WEB_153501_Antilevskaya.API.Data;
using WEB_153501_Antilevskaya.API.Services.ExhibitService;
using WEB_153501_Antilevskaya.Domain.Entities;
using WEB_153501_Antilevskaya.Domain.Models;

namespace WEB_153501_Antilevskaya.Tests;

public class ExhibitServiceTest
{
    private readonly DbConnection _connection;
    private readonly DbContextOptions<AppDbContext> _contextOptions;

    public ExhibitServiceTest()
    {
        _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();

        _contextOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(_connection)
            .Options;

        using var context = new AppDbContext(_contextOptions);
        context.Database.EnsureCreated();
        context.Category.AddRange(
            new Category { Id = 1, Name = "Category1", NormalizedName = "category1" },
            new Category { Id = 2, Name = "Category2", NormalizedName = "category2" },
            new Category { Id = 3, Name = "Category3", NormalizedName = "category3" });
        context.Exhibit.AddRange(
            new Exhibit { Id = 1, Title = "Exhibit1", Description = "", CategoryId = 1, Price = 10 },
            new Exhibit { Id = 2, Title = "Exhibit2", Description = "", CategoryId = 2, Price = 20 },
            new Exhibit { Id = 3, Title = "Exhibit3", Description = "", CategoryId = 3, Price = 30 },
            new Exhibit { Id = 4, Title = "Exhibit4", Description = "", CategoryId = 1, Price = 40 },
            new Exhibit { Id = 5, Title = "Exhibit5", Description = "", CategoryId = 2, Price = 50 },
            new Exhibit { Id = 6, Title = "Exhibit6", Description = "", CategoryId = 3, Price = 60 });
        context.SaveChanges();
    }

    public AppDbContext CreateContext() => new(_contextOptions);
    public void Dispose() => _connection.Dispose();

    [Fact]
    public void ServiceReturnsFirstPageOfThreeItems()
    {
        using var context = CreateContext();
        var service = new ExhibitService(context, null, null, null);

        var result = service.GetExhibitListAsync(null).Result;

        Assert.IsType<ResponseData<ListModel<Exhibit>>>(result);
        Assert.True(result.Success);
        Assert.Equal(1, result.Data.CurrentPage);
        Assert.Equal(2, result.Data.TotalPages);
        Assert.Equal(3, result.Data.Items.Count);
        Assert.Equal(context.Exhibit.First(), result.Data.Items[0]);
    }

    [Fact]
    public void CurrentPageGreaterThanTotalPages()
    {
        using var context = CreateContext();
        var service = new ExhibitService(context, null, null, null);

        var result = service.GetExhibitListAsync(null, 3).Result;

        Assert.False(result.Success);
        Assert.Equal(result.ErrorMessage, "No such page");
    }

    [Fact]
    public void ReturnsCorrectPage()
    {
        using var context = CreateContext();
        var service = new ExhibitService(context, null, null, null);
        var pageSize = 3;
        var pageNumber = 2;

        var result = service.GetExhibitListAsync(null, pageNumber, pageSize).Result;

        Assert.IsType<ResponseData<ListModel<Exhibit>>>(result);
        Assert.True(result.Success);
        Assert.Equal(2, result.Data.CurrentPage);
        Assert.Equal(3, result.Data.Items.Count());

        var expectedExhibits = context.Exhibit.Skip((pageNumber-1)*pageSize).Take(pageSize).ToList();
        Assert.Equal(expectedExhibits, result.Data.Items);
    }

    [Fact]
    public void ServiceReturnsFilteredItemsByCategory()
    {
        var context = CreateContext();
        var service = new ExhibitService(context, null, null, null);
        var categoryNormalizedName = "category1";
        var categoryId = 1;

        var result = service.GetExhibitListAsync(categoryNormalizedName).Result;

        Assert.IsType<ResponseData<ListModel<Exhibit>>>(result);
        Assert.True(result.Success);
        Assert.All(result.Data.Items, exhibit => Assert.Equal(categoryId, exhibit.CategoryId));
    }

    [Fact]
    public void ServiceDoesNotAllowPageSizeGreaterThanMax()
    {
        var context = CreateContext();
        var service = new ExhibitService(context, null, null, null);

        var result = service.GetExhibitListAsync(null, 1, service.MaxSizePage + 1).Result;

        Assert.IsType<ResponseData<ListModel<Exhibit>>>(result);
        Assert.True(result.Success);
        Assert.True(result.Data.Items.Count() <= service.MaxSizePage);
    }
}
