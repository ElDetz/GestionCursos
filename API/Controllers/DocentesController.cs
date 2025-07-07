using API.Data;
using API.Dtos.Docente;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DocentesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DocentesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/docentes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DocenteResponseDto>>> GetDocentes()
        {
            var docentes = await _context.Docentes.Include(d => d.Cursos).ToListAsync();

            var response = docentes.Select(d => new DocenteResponseDto
            {
                Id = d.Id,
                Apellidos = d.Apellidos,
                Nombres = d.Nombres,
                Profesion = d.Profesion,
                Correo = d.Correo,
                TotalCursos = d.Cursos?.Count ?? 0
            });

            return Ok(response);
        }

        // GET: api/docentes/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<DocenteResponseDto>> GetDocente(int id)
        {
            var docente = await _context.Docentes.Include(d => d.Cursos).FirstOrDefaultAsync(d => d.Id == id);
            if (docente == null)
                return NotFound();

            var dto = new DocenteResponseDto
            {
                Id = docente.Id,
                Apellidos = docente.Apellidos,
                Nombres = docente.Nombres,
                Profesion = docente.Profesion,
                Correo = docente.Correo,
                TotalCursos = docente.Cursos?.Count ?? 0
            };

            return Ok(dto);
        }

        // POST: api/docentes
        [HttpPost]
        public async Task<ActionResult<Docente>> PostDocente(DocenteDto dto)
        {
            var docente = new Docente
            {
                Apellidos = dto.Apellidos,
                Nombres = dto.Nombres,
                Profesion = dto.Profesion,
                FechaNacimiento = dto.FechaNacimiento,
                Correo = dto.Correo
            };

            _context.Docentes.Add(docente);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDocente), new { id = docente.Id }, docente);
        }

        // PUT: api/docentes/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDocente(int id, DocenteDto dto)
        {
            var docente = await _context.Docentes.FindAsync(id);
            if (docente == null)
                return NotFound();

            docente.Apellidos = dto.Apellidos;
            docente.Nombres = dto.Nombres;
            docente.Profesion = dto.Profesion;
            docente.FechaNacimiento = dto.FechaNacimiento;
            docente.Correo = dto.Correo;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/docentes/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDocente(int id)
        {
            var docente = await _context.Docentes.FindAsync(id);
            if (docente == null)
                return NotFound();

            _context.Docentes.Remove(docente);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
