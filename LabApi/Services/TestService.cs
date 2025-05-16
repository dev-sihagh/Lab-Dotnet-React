using LabApi.Data;
using LabApi.Models;
using Microsoft.EntityFrameworkCore;
namespace LabApi.Services
{
public class TestService : ITestService
{
    private readonly LabDbContext _context;

    public TestService(LabDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Test>> GetAllAsync()
        => await _context.Tests.ToListAsync();

    public async Task<Test?> GetByIdAsync(int id)
        => await _context.Tests.FindAsync(id);

    public async Task<Test> CreateAsync(Test test)
    {
        _context.Tests.Add(test);
        await _context.SaveChangesAsync();
        return test;
    }

    public async Task<Test?> UpdateAsync(int id, Test updatedTest)
    {
        var test = await _context.Tests.FindAsync(id);
        if (test == null) return null;

        test.Name = updatedTest.Name;
        test.Description = updatedTest.Description;

        await _context.SaveChangesAsync();
        return test;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var test = await _context.Tests.FindAsync(id);
        if (test == null) return false;

        _context.Tests.Remove(test);
        await _context.SaveChangesAsync();
        return true;
    }
    public async Task<decimal> GetTotalPriceAsync(List<int> testIds)
{
    return await _context.Tests
        .Where(t => testIds.Contains(t.Id))
        .SumAsync(t => t.Price);
}
}
}
