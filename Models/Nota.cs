namespace UniversidadeApi.Models;

public class Nota
{
    public long Id { get; set; }
    public int Valor { get; set; }
    public Aluno Aluno { get; set; }

    public ICollection <UnidadeCurricular> UnidadeCurricular { get; set; }
}