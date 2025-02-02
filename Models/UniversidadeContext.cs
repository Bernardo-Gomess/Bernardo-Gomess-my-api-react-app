using Microsoft.EntityFrameworkCore;

namespace UniversidadeApi.Models;

public class UniversidadeContext : DbContext
{
    public UniversidadeContext(DbContextOptions<UniversidadeContext> options)
        : base(options)
    {
    }

    // DbSet properties for each entity
    public DbSet<Curso> Curso { get; set; }
    public DbSet<UnidadeCurricular> UnidadesCurriculare { get; set; }
    public DbSet<Aluno> Aluno { get; set; }
    public DbSet<Nota> Nota{ get; set; }


}