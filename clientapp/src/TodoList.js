import React, { useState, useEffect } from 'react';
import { useAuth0 } from '@auth0/auth0-react';

const TodoList = () => {
  const [todos, setTodos] = useState([]);
  const [newTodo, setNewTodo] = useState('');
  const [error, setError] = useState(null);  // Properly define error state
  const { getAccessTokenSilently } = useAuth0();

  useEffect(() => {
    fetchTodos();
  }, []);

  const fetchTodos = async () => {
    try {
      const token = await getAccessTokenSilently();
      console.log('Fetched token:', token);
      const response = await fetch('http://localhost:5271/todolist', {
        headers: {
          Authorization: `Bearer ${token}`
        }
      });
      if (!response.ok) {
        throw new Error(`HTTP error! status: ${response.status}`);
      }
      const data = await response.json();
      setTodos(data.items || []);
      setError(null);  // Clear any previous errors
    } catch (error) {
      console.error('Error fetching todos:', error);
      setError(error.message);
    }
  };

  const addTodo = async () => {
    try {
      const token = await getAccessTokenSilently();
      const response = await fetch('http://localhost:5271/todolist', {
        method: 'PUT',
        headers: {
          'Content-Type': 'application/json',
          Authorization: `Bearer ${token}`
        },
        body: JSON.stringify({ title: 'My Todo List', items: [...todos, { title: newTodo, isCompleted: false }] })
      });
      if (!response.ok) {
        throw new Error(`HTTP error! status: ${response.status}`);
      }
      setNewTodo('');
      fetchTodos();
      setError(null);  // Clear any previous errors
    } catch (error) {
      console.error('Error adding todo:', error);
      setError(error.message);
    }
  };

  return (
    <div>
      <h2>My Todo List</h2>
      {error && <p style={{color: 'red'}}>Error: {error}</p>}
      <ul>
        {todos.map((todo, index) => (
          <li key={index}>{todo.title}</li>
        ))}
      </ul>
      <input
        type="text"
        value={newTodo}
        onChange={(e) => setNewTodo(e.target.value)}
        placeholder="New todo"
      />
      <button onClick={addTodo}>Add Todo</button>
    </div>
  );
};

export default TodoList;