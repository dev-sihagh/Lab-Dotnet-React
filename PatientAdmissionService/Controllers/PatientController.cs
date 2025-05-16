using Microsoft.AspNetCore.Mvc;
using PatientAdmissionService.Data;
using PatientAdmissionService.Models;
using Polly;
using Polly.Retry;
using System.Net.Http;
using Microsoft.EntityFrameworkCore;

namespace PatientAdmissionService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PatientController : ControllerBase
{
    private readonly AdmissionDbContext _context;
    private readonly HttpClient _httpClient; 
    private readonly string _doctorBaseUrl = "http://localhost:5217";
    private readonly AsyncRetryPolicy<HttpResponseMessage> _retryPolicy;

    public PatientController(AdmissionDbContext context)
    {
        _context = context;

        _httpClient = new HttpClient
        {
            Timeout = TimeSpan.FromSeconds(5) 
        };

        
        _retryPolicy = Policy
            .HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
            .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(retryAttempt));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Patient patient)
    {
      
        var doctorResponse = await _retryPolicy.ExecuteAsync(() =>
            _httpClient.GetAsync($"{_doctorBaseUrl}/api/doctors/{patient.DoctorId}")
        );

        if (!doctorResponse.IsSuccessStatusCode)
        {
            return BadRequest($"Doctor with ID {patient.DoctorId} not found.");
        }

        _context.Patients.Add(patient);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = patient.Id }, patient);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var patient = await _context.Patients.FindAsync(id);
        return patient == null ? NotFound() : Ok(patient);
    }
    [HttpGet]
public async Task<IActionResult> GetAll()
{
    var patients = await _context.Patients.ToListAsync();
    return Ok(patients);
}
}
