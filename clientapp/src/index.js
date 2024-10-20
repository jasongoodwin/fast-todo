import React from 'react';
import ReactDOM from 'react-dom/client';
import { Auth0Provider } from '@auth0/auth0-react';
import App from './App';

console.log('Auth0 Config:', {
  redirectUri: window.location.origin
});

const root = ReactDOM.createRoot(document.getElementById('root'));
root.render(
  <React.StrictMode>
    <Auth0Provider
    domain="dev-e6udwsh7nrspbnaj.us.auth0.com"
    clientId="0IbAJXP2i7pUPcOPFeOkOfJ1z1VkOcxk"
      authorizationParams={{
        redirect_uri: window.location.origin
      }}
    >
      <App />
    </Auth0Provider>
  </React.StrictMode>
);