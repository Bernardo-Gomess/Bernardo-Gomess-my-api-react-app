import React, { useState, useEffect } from 'react';
import axios from 'axios';

const CursoList = () => {
  const [cursos, setCursos] = useState([]);

  useEffect(() => {
    axios.get('http://localhost:5112/api/Curso')
      .then(response => {
        setCursos(response.data);
      })
      .catch(error => console.error('There was an error fetching courses:', error));
  }, []);

  return (
    <div>
      <h2>List of Courses</h2>
      <ul>
        {cursos.map(curso => (
          <li key={curso.id}>
            {curso.sigla} - {curso.nome}
          </li>
        ))}
      </ul>
    </div>
  );
};

export default CursoList;
