using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LabApi.Data;
using LabApi.Models;

[ApiController]
[Route("api/[controller]")]
public class DoctorsController : ControllerBase
{
    private readonly LabDbContext _context;

    public DoctorsController(LabDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> CreateDoctor([FromBody] Doctor doctor)
    {
        _context.Doctors.Add(doctor);
        await _context.SaveChangesAsync();
        return Ok(doctor);
    }
    [HttpGet]
public async Task<ActionResult<IEnumerable<Doctor>>> GetDoctors()
{
    var doctors = await _context.Doctors.ToListAsync();
    return Ok(doctors);
}
[HttpGet("{id}")]
public async Task<IActionResult> GetById(int id)
{
    var doctor = await _context.Doctors.FindAsync(id);
    return doctor == null ? NotFound() : Ok(doctor);
}
[HttpPut("{id}")]
public async Task<IActionResult> UpdateDoctor(int id, [FromBody] Doctor updatedDoctor)
{
    var doctor = await _context.Doctors.FindAsync(id);
    if (doctor == null)
        return NotFound();

    doctor.Name = updatedDoctor.Name;
    doctor.Specialty = updatedDoctor.Specialty;

    await _context.SaveChangesAsync();

    return Ok(doctor);
}
}
