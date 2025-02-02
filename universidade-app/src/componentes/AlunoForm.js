import React, { useState } from "react";

const AlunoForm = () => {
  const [nome, setNome] = useState("");
  const [siglaCurso, setSiglaCurso] = useState("");
  const [error, setError] = useState("");

  const handleSubmit = async (e) => {
    e.preventDefault();

    const newAluno = { nome, siglaCurso };

    try {
      const response = await fetch("http://localhost:5112/api/Aluno", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(newAluno),
      });

      if (!response.ok) {
        throw new Error("Failed to add student");
      }

      alert("Student added successfully!");
      setNome("");
      setSiglaCurso("");
    } catch (error) {
      setError(error.message);
    }
  };

  return (
    <div>
      <h2>Add Student</h2>
      {error && <p style={{ color: "red" }}>{error}</p>}
      <form onSubmit={handleSubmit}>
        <div>
          <label>Nome:</label>
          <input type="text" value={nome} onChange={(e) => setNome(e.target.value)} required />
        </div>
        <div>
          <label>Sigla Curso:</label>
          <input type="text" value={siglaCurso} onChange={(e) => setSiglaCurso(e.target.value)} required />
        </div>
        <button type="submit">Add Student</button>
      </form>
    </div>
  );
};

export default AlunoForm;
