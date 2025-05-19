using Microsoft.AspNetCore.Mvc;
using PatientAdmissionService.Data;
using PatientAdmissionService.Models;
using PatientAdmissionService.Services;
using Polly;
using Polly.Retry;
using System.Net.Http;
using Microsoft.EntityFrameworkCore;

namespace PatientAdmissionService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PatientController : ControllerBase
{
    private readonly IPatientService _service;
    private readonly HttpClient _httpClient; 
    private readonly string _doctorBaseUrl = "http://localhost:5217";
    private readonly AsyncRetryPolicy<HttpResponseMessage> _retryPolicy;

    public PatientController(IPatientService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Patient patient)
    {
        try
        {
  var created = await _service.CreateAsync(patient);
        // if (created == null)
        // {
        //     return BadRequest($"Doctor with ID {patient.DoctorId} not found.");
        // }
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }
        
         catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
        
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
         var patient = await _service.GetByIdAsync(id);
        return patient == null ? NotFound() : Ok(patient);
    }
    [HttpGet]
public async Task<IActionResult> GetAll()
{
     var patients = await _service.GetAllAsync();
        return Ok(patients);
}
}
