using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PatientApi.Data;
using PatientApi.Models;

namespace PatientApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PatientsController : ControllerBase
    {
        private readonly PatientContext _context;

        public PatientsController(PatientContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Patient>>> GetPatients()
        {
            return await _context.Patients.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Patient>> GetPatient(Guid id)
        {
            var patient = await _context.Patients.FindAsync(id);

            if (patient == null)
            {
                return NotFound();
            }

            return patient;
        }

        [HttpPost]
        public async Task<ActionResult<Patient>> PostPatient(Patient patient)
        {
            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPatient), new { id = patient.Id }, patient);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutPatient(Guid id, Patient patient)
        {
            if (id != patient.Id)
            {
                return BadRequest();
            }

            _context.Entry(patient).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PatientExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePatient(Guid id)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient == null)
            {
                return NotFound();
            }

            _context.Patients.Remove(patient);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PatientExists(Guid id)
        {
            return _context.Patients.Any(e => e.Id == id);
        }

        [HttpGet("search")]
        // FHIR comlient
        public async Task<ActionResult<IEnumerable<Patient>>> SearchPatients([FromQuery] string birthDate)
        {
            if (string.IsNullOrEmpty(birthDate))
            {
                return BadRequest("birthDate parameter is required.");
            }

            var query = _context.Patients.AsQueryable();

            if (birthDate.StartsWith("eq"))
            {
                var date = DateTime.Parse(birthDate.Substring(2));
                query = query.Where(p => p.BirthDate == date);
            }
            else if (birthDate.StartsWith("ne"))
            {
                var date = DateTime.Parse(birthDate.Substring(2));
                query = query.Where(p => p.BirthDate != date);
            }
            else if (birthDate.StartsWith("gt"))
            {
                var date = DateTime.Parse(birthDate.Substring(2));
                query = query.Where(p => p.BirthDate > date);
            }
            else if (birthDate.StartsWith("lt"))
            {
                var date = DateTime.Parse(birthDate.Substring(2));
                query = query.Where(p => p.BirthDate < date);
            }
            else if (birthDate.StartsWith("ge"))
            {
                var date = DateTime.Parse(birthDate.Substring(2));
                query = query.Where(p => p.BirthDate >= date);
            }
            else if (birthDate.StartsWith("le"))
            {
                var date = DateTime.Parse(birthDate.Substring(2));
                query = query.Where(p => p.BirthDate <= date);
            }
            else
            {
                return BadRequest("Invalid birthDate parameter format.");
            }

            return await query.ToListAsync();
        }
    }
}
