import React from 'react';
import './LoginForm.css';

function Login() {
  return (
    <div className="login-container">
      <div className="login-left">
        <h1 class="site-name">Website name</h1>
        <h1 class="welcome-text">Your friends are waiting for you!</h1>
        <p>Log in quickly to check their news and share your own! </p>
      </div>
      <div className="login-right">
        <form>
          <div className="input-field">
            <label htmlFor="email">Email</label>
            <input type="email" id="email" name="email" placeholder="Enter your email" required />
          </div>
          <div className="input-field">
            <label htmlFor="password">Password</label>
            <input type="password" id="password" name="password" placeholder="Enter your password" required />
          </div>
          <div className="checkbox-field">
              <input type="checkbox" id="checkbox" name="checkbox" class="checkbox"/> 
              <label htmlFor="checkbox" class="checkbox-text">Remember me</label>
          </div>
          <button type="submit" className="btn-submit">Log in</button>
        </form>
      </div>
    </div>
  );
}

export default Login;
