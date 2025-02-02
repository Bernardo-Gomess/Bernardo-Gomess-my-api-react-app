import React, { useState, useEffect } from 'react';
import axios from 'axios';

const AlunoList = () => {
  const [alunos, setAlunos] = useState([]);

  useEffect(() => {
    axios.get('http://localhost:5112/api/Aluno')
      .then(response => {
        setAlunos(response.data);
      })
      .catch(error => console.error('There was an error fetching students:', error));
  }, []);

  return (
    <div>
      <h2>List of Students</h2>
      <ul>
        {alunos.map(aluno => (
          <li key={aluno.id}>
            {aluno.nome} - {aluno.siglaCurso}
          </li>
        ))}
      </ul>
    </div>
  );
};

export default AlunoList;
