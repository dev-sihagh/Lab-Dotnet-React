using LabApi.Models;
using LabApi.Data;
using LabApi.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

public class TestServiceTests
{
    private readonly TestService _service;
    private readonly LabDbContext _context;

    public TestServiceTests()
    {
        var options = new DbContextOptionsBuilder<LabDbContext>()
            .UseInMemoryDatabase(databaseName: "LabTestDb")
            .Options;

        _context = new LabDbContext(options);
        SeedData();

        _service = new TestService(_context);
    }

    private void SeedData()
    {
        _context.Tests.AddRange(
            new Test { Id = 1, Name = "A", Price = 10 },
            new Test { Id = 2, Name = "B", Price = 15 },
            new Test { Id = 3, Name = "C", Price = 5 }
        );
        _context.SaveChanges();
    }

    [Fact]
    public async Task GetTotalPriceAsync_ReturnsCorrectSum()
    {
        // Arrange
        var testIds = new List<int> { 1, 2 };

        // Act
        var total = await _service.GetTotalPriceAsync(testIds);

        // Assert
        Assert.Equal(25, total); // 10 + 15
    }
}
