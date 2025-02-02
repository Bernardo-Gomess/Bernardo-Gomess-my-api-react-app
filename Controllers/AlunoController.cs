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
    [Route("api/Aluno")]
    [ApiController]
    public class AlunoController : ControllerBase
    {
        private readonly UniversidadeContext _context;

        public AlunoController(UniversidadeContext context)
        {
            _context = context;
        }

        // GET: api/Aluno
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AlunoDTO>>> GetAlunoDTO()
        {
            var alunoDTOs = await _context.Aluno
            .Select(x => new AlunoDTO
            {
                Id = x.Id,
                Nome = x.Nome,
                SiglaCurso = x.Curso.Sigla // Assuming you have a reference to Curso in Aluno
            })
        .ToListAsync();

            return alunoDTOs;
        }
        

        // GET: api/Aluno/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AlunoDTO>> GetAluno(long id)
        {
            var aluno = await _context.Aluno
                .Include(a => a.Curso)  // Make sure to include the Curso
                .FirstOrDefaultAsync(a => a.Id == id);

            if (aluno == null)
            {
                return NotFound(); // If no Aluno is found, return 404
            }

            // Map the Aluno to an AlunoDTO
            var alunoDto = new AlunoDTO
            {
                Id = aluno.Id,
                Nome = aluno.Nome,
                SiglaCurso = aluno.Curso.Sigla
            };

            return alunoDto; // Return the AlunoDTO
        }


        // PUT: api/Aluno/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAluno(long id, AlunoDTO alunoDto)
        {
            if (id != alunoDto.Id)
            {
                return BadRequest(); // Ensure the IDs match
            }

            // Find the existing Aluno entity
            var aluno = await _context.Aluno.FindAsync(id);
            if (aluno == null)
            {
                return NotFound(); // If Aluno not found, return 404
            }

            // Look up the Curso by SiglaCurso
            var curso = await _context.Curso
                .FirstOrDefaultAsync(c => c.Sigla == alunoDto.SiglaCurso);

            if (curso == null)
            {
                return NotFound($"Curso with Sigla {alunoDto.SiglaCurso} not found.");
            }

            // Update the Aluno entity with values from AlunoDTO
            aluno.Nome = alunoDto.Nome;
            aluno.Curso = curso;

            _context.Entry(aluno).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent(); // Return 204 No Content on success
        }

        // POST: api/Aluno
        [HttpPost]
        public async Task<ActionResult<Aluno>> PostAluno(AlunoDTO alunoDto)
        {
            // Look up the Curso by SiglaCurso (this assumes the SiglaCurso exists)
            var curso = await _context.Curso
                .FirstOrDefaultAsync(c => c.Sigla == alunoDto.SiglaCurso);

            if (curso == null)
            {
                return NotFound($"Curso with Sigla {alunoDto.SiglaCurso} not found.");
            }

            // Create a new Aluno entity from the AlunoDTO
            var aluno = new Aluno
            {
                Nome = alunoDto.Nome,
                Curso = curso
            };

            _context.Aluno.Add(aluno);
            await _context.SaveChangesAsync();

            // Return the created Aluno as a DTO
            var createdAlunoDto = new AlunoDTO
            {
                Id = aluno.Id,
                Nome = aluno.Nome,
                SiglaCurso = aluno.Curso.Sigla
            };

            return CreatedAtAction(nameof(GetAluno), new { id = aluno.Id }, createdAlunoDto);
        }


        // DELETE: api/Aluno/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAluno(long id)
        {
            var aluno = await _context.Aluno.FindAsync(id);
            if (aluno == null)
            {
                return NotFound();
            }

            _context.Aluno.Remove(aluno);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AlunoExists(long id)
        {
            return _context.Aluno.Any(e => e.Id == id);
        }
    }
}
