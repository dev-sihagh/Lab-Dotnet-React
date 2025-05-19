using Microsoft.EntityFrameworkCore;
using PatientAdmissionService.Models;
using PatientAdmissionService.Data;
using Polly;
using Polly.Retry;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
namespace PatientAdmissionService.Services
{
    public class PatientService : IPatientService
    {
        private readonly AdmissionDbContext _context;
        private readonly HttpClient _httpClient;
        private readonly AsyncRetryPolicy<HttpResponseMessage> _retryPolicy;
        private readonly string _doctorBaseUrl = "http://localhost:5217";
        public PatientService(AdmissionDbContext context)
        {
            _context = context;
            _httpClient = new HttpClient
            {
                Timeout = TimeSpan.FromSeconds(5)
            };
            _retryPolicy = Policy.HandleResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(retryAttempt));
        }
        public async Task<IEnumerable<Patient>> GetAllAsync()
        {
            return await _context.Patients.ToListAsync();
        }
        public async Task<Patient?> GetByIdAsync(int id)
        {
            return await _context.Patients.FindAsync(id);
        }
         public async Task<Patient?> CreateAsync(Patient patient)
        {
            var doctorResponse = await _retryPolicy.ExecuteAsync(() =>
                _httpClient.GetAsync($"{_doctorBaseUrl}/api/doctors/{patient.DoctorId}")
            );

            if (!doctorResponse.IsSuccessStatusCode)
        throw new Exception($"Doctor with ID {patient.DoctorId} not found.");

            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();
            return patient;
        }
        
    }
}