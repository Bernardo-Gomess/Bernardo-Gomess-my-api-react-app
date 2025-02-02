using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniversidadeApi.Models;

namespace UniversidadeApi.Controllers
{
    [Route("api/Curso")]
    [ApiController]
    public class CursoController : ControllerBase
    {
        private readonly UniversidadeContext _context;

        public CursoController(UniversidadeContext context)
        {
            _context = context;
        }

        // GET: api/Curso
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Curso>>> GetCurso()
        {
            return await _context.Curso.ToListAsync();
        }

        // GET: api/Curso/{identifier}
        [HttpGet("{identifier}")]
        public async Task<ActionResult<Curso>> GetCurso(string identifier)
        {
            // Try to parse identifier as a number (id)
            if (long.TryParse(identifier, out var id))
            {
                var cursoById = await _context.Curso.FindAsync(id);
                if (cursoById == null)
                {
                    return NotFound(); // If no Curso is found by id
                }
                return cursoById; // Return the found Curso by id
            }
            else
            {
                // If identifier is not a number, treat it as a sigla
                var cursoBySigla = await _context.Curso
                    .FirstOrDefaultAsync(c => c.Sigla == identifier);
                if (cursoBySigla == null)
                {
                    return NotFound(); // If no Curso is found by sigla
                }
                return cursoBySigla; // Return the found Curso by sigla
            }
        }




        // PUT: api/Curso/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCurso(long id, Curso curso)
        {
            if (id != curso.Id)
            {
                return BadRequest();
            }

            _context.Entry(curso).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CursoExists(id))
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

        // POST: api/Curso
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Curso>> PostCurso(Curso curso)
        {
            _context.Curso.Add(curso);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCurso), new { id = curso.Id }, curso);
        }

        // DELETE: api/Curso/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCurso(long id)
        {
            var curso = await _context.Curso.FindAsync(id);
            if (curso == null)
            {
                return NotFound();
            }

            _context.Curso.Remove(curso);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CursoExists(long id)
        {
            return _context.Curso.Any(e => e.Id == id);
        }
    }
}
