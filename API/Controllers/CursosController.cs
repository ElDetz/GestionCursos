using API.Data;
using API.Dtos.Curso;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CursosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CursosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/cursos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CursoResponseDto>>> GetCursos()
        {
            var cursos = await _context.Cursos.Include(c => c.Docente).ToListAsync();

            var response = cursos.Select(c => new CursoResponseDto
            {
                Id = c.Id,
                Nombre = c.Nombre,
                Creditos = c.Creditos,
                HorasSemanal = c.HorasSemanal,
                Ciclo = c.Ciclo,
                NombreDocente = $"{c.Docente.Nombres} {c.Docente.Apellidos}"
            });

            return Ok(response);
        }

        // GET: api/cursos/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<CursoResponseDto>> GetCurso(int id)
        {
            var curso = await _context.Cursos.Include(c => c.Docente).FirstOrDefaultAsync(c => c.Id == id);
            if (curso == null)
                return NotFound();

            var dto = new CursoResponseDto
            {
                Id = curso.Id,
                Nombre = curso.Nombre,
                Creditos = curso.Creditos,
                HorasSemanal = curso.HorasSemanal,
                Ciclo = curso.Ciclo,
                NombreDocente = $"{curso.Docente.Nombres} {curso.Docente.Apellidos}"
            };

            return Ok(dto);
        }

        // GET: api/cursos/ciclo/{ciclo}
        [HttpGet("ciclo/{ciclo}")]
        public async Task<ActionResult<IEnumerable<CursoResponseDto>>> GetCursosPorCiclo(string ciclo)
        {
            var cursos = await _context.Cursos
                .Include(c => c.Docente)
                .Where(c => c.Ciclo.ToLower() == ciclo.ToLower())
                .ToListAsync();

            var response = cursos.Select(c => new CursoResponseDto
            {
                Id = c.Id,
                Nombre = c.Nombre,
                Creditos = c.Creditos,
                HorasSemanal = c.HorasSemanal,
                Ciclo = c.Ciclo,
                NombreDocente = $"{c.Docente.Nombres} {c.Docente.Apellidos}"
            });

            return Ok(response);
        }

        // POST: api/cursos
        [HttpPost]
        public async Task<ActionResult<Curso>> PostCurso(CursoDto dto)
        {
            var docenteExiste = await _context.Docentes.AnyAsync(d => d.Id == dto.IdDocente);
            if (!docenteExiste)
                return BadRequest("El docente especificado no existe.");

            var curso = new Curso
            {
                Nombre = dto.Nombre,
                Creditos = dto.Creditos,
                HorasSemanal = dto.HorasSemanal,
                Ciclo = dto.Ciclo,
                IdDocente = dto.IdDocente
            };

            _context.Cursos.Add(curso);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCurso), new { id = curso.Id }, curso);
        }

        // PUT: api/cursos/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCurso(int id, CursoDto dto)
        {
            var curso = await _context.Cursos.FindAsync(id);
            if (curso == null)
                return NotFound();

            curso.Nombre = dto.Nombre;
            curso.Creditos = dto.Creditos;
            curso.HorasSemanal = dto.HorasSemanal;
            curso.Ciclo = dto.Ciclo;
            curso.IdDocente = dto.IdDocente;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/cursos/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCurso(int id)
        {
            var curso = await _context.Cursos.FindAsync(id);
            if (curso == null)
                return NotFound();

            _context.Cursos.Remove(curso);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}

