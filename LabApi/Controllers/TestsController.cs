using Microsoft.AspNetCore.Mvc;
using LabApi.Models;

[ApiController]
[Route("api/[controller]")]
public class TestsController : ControllerBase
{
    private readonly ITestService _service;

    public TestsController(ITestService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var test = await _service.GetByIdAsync(id);
        return test == null ? NotFound() : Ok(test);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Test test)
    {
        var created = await _service.CreateAsync(test);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Test test)
    {
        var updated = await _service.UpdateAsync(id, test);
        return updated == null ? NotFound() : Ok(updated);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _service.DeleteAsync(id);
        return deleted ? NoContent() : NotFound();
    }
    [HttpPost("total-price")]
public async Task<IActionResult> GetTotalPrice([FromBody] TestPriceRequest request)
{
    var total = await _service.GetTotalPriceAsync(request.TestIds);
    return Ok(new { total });
}
}
