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
    public class UnidadeCurricularController : ControllerBase
    {
        private readonly UniversidadeContext _context;

        public UnidadeCurricularController(UniversidadeContext context)
        {
            _context = context;
        }

        // GET: api/UnidadeCurricular
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UC_DTO>>> GetUC_DTO()
        {
            var UC_DTOs = await _context.UnidadeCurricular
                .Include(uc => uc.Curso) // ✅ Ensure Curso is included
                .Select(x => new UC_DTO
                {
                    Id = x.Id,
                    Sigla = x.Sigla,
                    Nome = x.Nome,
                    Ano = x.Ano,
                    SiglaCurso = x.Curso != null ? x.Curso.Sigla : "N/A" // ✅ Prevent null reference
                })
                .ToListAsync();

            return UC_DTOs;
        }


        // GET: api/UnidadeCurricular/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UC_DTO>> GetUnidadeCurricular(long id)
        {
            var unidadeCurricular = await _context.UnidadeCurricular
                .Include(uc => uc.Curso) // ✅ Ensure Curso is included
                .FirstOrDefaultAsync(uc => uc.Id == id);

            if (unidadeCurricular == null)
            {
                return NotFound();
            }

            // Convert to DTO
            var unidadeCurricularDTO = new UC_DTO
            {
                Id = unidadeCurricular.Id,
                Sigla = unidadeCurricular.Sigla,
                Nome = unidadeCurricular.Nome,
                Ano = unidadeCurricular.Ano,
                SiglaCurso = unidadeCurricular.Curso != null ? unidadeCurricular.Curso.Sigla : "N/A"
            };

            return unidadeCurricularDTO;
        }


        // PUT: api/UnidadeCurricular/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
public async Task<IActionResult> PutUnidadeCurricular(long id, UC_DTO unidadeCurricularDTO)
{
    if (id != unidadeCurricularDTO.Id)
    {
        return BadRequest();
    }

    var unidadeCurricular = await _context.UnidadeCurricular
        .Include(uc => uc.Curso) // Ensure Curso is included
        .FirstOrDefaultAsync(uc => uc.Id == id);

    if (unidadeCurricular == null)
    {
        return NotFound();
    }

    // Find Curso by Sigla
    var curso = await _context.Curso.FirstOrDefaultAsync(c => c.Sigla == unidadeCurricularDTO.SiglaCurso);
    if (curso == null)
    {
        return NotFound($"Curso with Sigla {unidadeCurricularDTO.SiglaCurso} not found.");
    }

    // Update properties
    unidadeCurricular.Sigla = unidadeCurricularDTO.Sigla;
    unidadeCurricular.Nome = unidadeCurricularDTO.Nome;
    unidadeCurricular.Ano = unidadeCurricularDTO.Ano;
    unidadeCurricular.Curso = curso; // Update Curso relationship

    try
    {
        await _context.SaveChangesAsync();
    }
    catch (DbUpdateConcurrencyException)
    {
        if (!UnidadeCurricularExists(id))
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


        // POST: api/UnidadeCurricular
        [HttpPost]
        public async Task<ActionResult<UnidadeCurricular>> PostUC_DTO(UC_DTO UCs_DTO)
        {
            // Find the Curso entity based on SiglaCurso from the DTO
            var curso = await _context.Curso
                .FirstOrDefaultAsync(c => c.Sigla == UCs_DTO.SiglaCurso);

            // If no matching Curso is found, return NotFound
            if (curso == null)
            {
                return NotFound($"Curso with Sigla {UCs_DTO.SiglaCurso} not found.");
            }

            // Create a new UnidadeCurricular entity based on the DTO
            var UCs = new UnidadeCurricular
            {
                Id = UCs_DTO.Id,
                Sigla = UCs_DTO.Sigla,
                Nome = UCs_DTO.Nome,
                Curso = curso,
                Ano = UCs_DTO.Ano
            };

            // Add the new UnidadeCurricular to the correct DbSet (UnidadeCurricular)
            _context.UnidadeCurricular.Add(UCs);

            // Save changes to the database
            await _context.SaveChangesAsync();

            // Map the created UnidadeCurricular entity back to the DTO
            var createdUC_Dto = new UC_DTO
            {
                Id = UCs.Id,
                Sigla = UCs.Sigla,
                Nome = UCs.Nome,
                SiglaCurso = UCs.Curso.Sigla,
                Ano = UCs.Ano
            };

            // Return a CreatedAtAction response, which includes the created resource
            return CreatedAtAction(nameof(GetUnidadeCurricular), new { id = UCs.Id }, createdUC_Dto);
        }



        // DELETE: api/UnidadeCurricular/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUnidadeCurricular(long id)
        {
            var unidadeCurricular = await _context.UnidadeCurricular.FindAsync(id);
            if (unidadeCurricular == null)
            {
                return NotFound();
            }

            _context.UnidadeCurricular.Remove(unidadeCurricular);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UnidadeCurricularExists(long id)
        {
            return _context.UnidadeCurricular.Any(e => e.Id == id);
        }
    }
}
