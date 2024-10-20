import React from 'react';
import { useAuth0 } from '@auth0/auth0-react';
import TodoList from './TodoList';

function App() {
  const { isLoading, isAuthenticated, error, user, loginWithRedirect, logout } = useAuth0();

  console.log('Auth State:', { isLoading, isAuthenticated, error, user });

  if (isLoading) {
    return <div>Loading... (Auth0 is still initializing)</div>;
  }

  if (error) {
    return <div>Oops... {error.message}</div>;
  }

  if (isAuthenticated) {
    return (
      <div>
        <h1>Hello {user?.name || 'User'}</h1>
        <button onClick={() => logout({ returnTo: window.location.origin })}>
          Log out
        </button>
        <TodoList />
      </div>
    );
  } else {
    return <button onClick={() => loginWithRedirect()}>Log in</button>;
  }
}

export default App;