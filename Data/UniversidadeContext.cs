using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UniversidadeApi.Models;

    public class UniversidadeContext : DbContext
    {
        public UniversidadeContext (DbContextOptions<UniversidadeContext> options)
            : base(options)
        {
        }

        public DbSet<UniversidadeApi.Models.Aluno> Aluno { get; set; } = default!;

        public DbSet<UniversidadeApi.Models.Curso> Curso { get; set; } = default!;

        public DbSet<UniversidadeApi.Models.Nota> Nota { get; set; } = default!;

        public DbSet<UniversidadeApi.Models.UnidadeCurricular> UnidadeCurricular { get; set; } = default!;
    }
