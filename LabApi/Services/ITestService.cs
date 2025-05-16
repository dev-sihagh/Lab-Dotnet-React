using LabApi.Models;

public interface ITestService
{
    Task<IEnumerable<Test>> GetAllAsync();
    Task<Test?> GetByIdAsync(int id);
    Task<Test> CreateAsync(Test test);
    Task<Test?> UpdateAsync(int id, Test test);
    Task<bool> DeleteAsync(int id);
    Task<decimal> GetTotalPriceAsync(List<int> testIds);
}
