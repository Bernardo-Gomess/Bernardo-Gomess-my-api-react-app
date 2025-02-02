namespace UniversidadeApi.Models;

public class Aluno
{
    public long Id { get; set; }
    public string? Nome { get; set; }
    public Curso Curso  { get; set; }

    public ICollection <UnidadeCurricular> UnidadeCurricular { get; set; }
}