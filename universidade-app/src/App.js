import React from "react";
import { BrowserRouter as Router, Route, Routes, Link } from "react-router-dom";
import CursoList from "./componentes/CursoList";
import CursoForm from "./componentes/CursoForm";
import AlunoList from "./componentes/AlunoList";
import AlunoForm from "./componentes/AlunoForm";

function App() {
  return (
    <Router>
      <div>
        <h1>Universidade API Frontend</h1>
        
        {/* Navigation Links */}
        <nav>
          <ul>
            <li><Link to="/cursos">View Courses</Link></li>
            <li><Link to="/create-curso">Add Course</Link></li>
            <li><Link to="/alunos">View Students</Link></li>
            <li><Link to="/create-aluno">Add Student</Link></li>
          </ul>
        </nav>

        {/* Define Routes */}
        <Routes>
          <Route path="/cursos" element={<CursoList />} />
          <Route path="/create-curso" element={<CursoForm />} />
          <Route path="/alunos" element={<AlunoList />} />
          <Route path="/create-aluno" element={<AlunoForm />} />
          {/* Default Route */}
          <Route path="/" element={<h2>Welcome to the Universidade API System</h2>} />
        </Routes>
      </div>
    </Router>
  );
}

export default App;
