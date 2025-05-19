using PatientAdmissionService.Models;
namespace PatientAdmissionService.Services
{
    public interface IPatientService
    {
        Task<IEnumerable<Patient>> GetAllAsync();
        Task<Patient?> GetByIdAsync(int id);
        Task<Patient?> CreateAsync(Patient patient);
    }
}