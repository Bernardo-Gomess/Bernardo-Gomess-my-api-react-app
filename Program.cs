using Microsoft.EntityFrameworkCore;
using UniversidadeApi.Models;

var builder = WebApplication.CreateBuilder(args);

// ✅ 1. Add CORS BEFORE `var app = builder.Build();`
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<UniversidadeContext>(opt =>
    opt.UseInMemoryDatabase("Universidade"));

// var app = builder.Build();
var app = builder.Build(); // ✅ Build after adding services

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Universidade API v1");
        c.RoutePrefix = "api"; // ✅ This changes the Swagger URL to http://localhost:5112/api/index.html
    });
}

app.UseCors("AllowAll"); // ✅ Apply CORS policy here

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// ✅ Seed the database with initial data
SeedDatabase(app);

app.Run();

// ✅ Method to seed the database
void SeedDatabase(WebApplication app)
{
    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<UniversidadeContext>();

        // ✅ Ensure Cursos exist before inserting Alunos or UnidadeCurricular
        if (!context.Curso.Any())
        {
            context.Curso.AddRange(
                new Curso { Id = 1, Sigla = "LES", Nome = "Engenharia de Sistemas" },
                new Curso { Id = 2, Sigla = "INF", Nome = "Engenharia Informática" }
            );
            context.SaveChanges(); // ✅ Save before proceeding!
        }

        // ✅ Fetch Cursos again after they are saved
        var cursoLES = context.Curso.FirstOrDefault(c => c.Sigla == "LES");
        var cursoINF = context.Curso.FirstOrDefault(c => c.Sigla == "INF");

        if (!context.Aluno.Any() && cursoLES != null && cursoINF != null)
        {
            context.Aluno.AddRange(
                new Aluno { Nome = "Carlos Silva", Curso = cursoLES },
                new Aluno { Nome = "Maria Oliveira", Curso = cursoINF }
            );
            context.SaveChanges();
        }

        // ✅ Ensure Curso exists before adding UnidadeCurricular
        if (!context.UnidadeCurricular.Any() && cursoLES != null && cursoINF != null)
        {
            context.UnidadeCurricular.AddRange(
                new UnidadeCurricular { Sigla = "UC1", Nome = "Programação", Ano = 1, Curso = cursoLES },
                new UnidadeCurricular { Sigla = "UC2", Nome = "Bases de Dados", Ano = 1, Curso = cursoINF },
                new UnidadeCurricular { Sigla = "UC3", Nome = "Redes de Computadores", Ano = 2, Curso = cursoLES },
                new UnidadeCurricular { Sigla = "UC4", Nome = "Segurança Informática", Ano = 2, Curso = cursoINF }
            );
            context.SaveChanges();
        }
    }
}
