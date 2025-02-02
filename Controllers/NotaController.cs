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
    [Route("api/[controller]")]
    [ApiController]
    public class NotaController : ControllerBase
    {
        private readonly UniversidadeContext _context;

        public NotaController(UniversidadeContext context)
        {
            _context = context;
        }

        // GET: api/Nota
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Nota>>> GetNota()
        {
            return await _context.Nota.ToListAsync();
        }

        // GET: api/Nota/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Nota>> GetNota(long id)
        {
            var nota = await _context.Nota.FindAsync(id);

            if (nota == null)
            {
                return NotFound();
            }

            return nota;
        }

        // PUT: api/Nota/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutNota(long id, Nota nota)
        {
            if (id != nota.Id)
            {
                return BadRequest();
            }

            _context.Entry(nota).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NotaExists(id))
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

        // POST: api/Nota
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Nota>> PostNota(Nota nota)
        {
            _context.Nota.Add(nota);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetNota", new { id = nota.Id }, nota);
        }

        // DELETE: api/Nota/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNota(long id)
        {
            var nota = await _context.Nota.FindAsync(id);
            if (nota == null)
            {
                return NotFound();
            }

            _context.Nota.Remove(nota);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool NotaExists(long id)
        {
            return _context.Nota.Any(e => e.Id == id);
        }
    }
}
