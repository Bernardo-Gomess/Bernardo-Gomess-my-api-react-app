import React, { useState } from "react";

const CursoForm = () => {
  const [sigla, setSigla] = useState("");
  const [nome, setNome] = useState("");
  const [error, setError] = useState("");

  const handleSubmit = async (e) => {
    e.preventDefault(); // Prevent page reload

    const newCurso = { sigla, nome };

    try {
      const response = await fetch("http://localhost:5112/api/Curso", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(newCurso),
      });

      if (!response.ok) {
        throw new Error("Failed to add course");
      }

      alert("Course added successfully!");
      setSigla("");
      setNome("");
    } catch (error) {
      setError(error.message);
    }
  };

  return (
    <div>
      <h2>Add Course</h2>
      {error && <p style={{ color: "red" }}>{error}</p>}
      <form onSubmit={handleSubmit}>
        <div>
          <label>Sigla:</label>
          <input type="text" value={sigla} onChange={(e) => setSigla(e.target.value)} required />
        </div>
        <div>
          <label>Nome:</label>
          <input type="text" value={nome} onChange={(e) => setNome(e.target.value)} required />
        </div>
        <button type="submit">Add Course</button>
      </form>
    </div>
  );
};

export default CursoForm;
