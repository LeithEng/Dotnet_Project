import React, { useState, useEffect } from "react";
import Cookies from "js-cookie"; 
import "./LoginForm.css";

function Login() {
  const [formData, setFormData] = useState({
    email: "",
    password: "",
    rememberMe: false,
  });

  const [error, setError] = useState("");
  const [message, setMessage] = useState("");
  const [loggedIn, setLoggedIn] = useState(false); // État pour gérer la connexion
  const [cookieValue, setCookieValue] = useState(""); // État pour stocker la valeur du cookie

  const handleChange = (e) => {
    const { name, value, type, checked } = e.target;
    setFormData({
      ...formData,
      [name]: type === "checkbox" ? checked : value,
    });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError("");
    setMessage("");

    try {
      const response = await fetch("http://localhost:5073/api/Authentication/Login", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(formData),
      });

      const data = await response.json();

      if (!response.ok) {
        throw new Error(data.message || "Login failed");
      }

      setMessage(data.message);
      setLoggedIn(true); // Mettez à jour l'état de connexion

      // Stocker le token dans sessionStorage
      sessionStorage.setItem("token", data.token);

      // Stocker le token dans un cookie
      const cookieOptions = {
        expires: formData.rememberMe ? 7 : 1,
        secure: true,
        sameSite: "strict",
      };
      Cookies.set("token", data.token, cookieOptions);

      // Tester la valeur du cookie après la connexion
      setCookieValue(Cookies.get("token")); // Récupérer la valeur du cookie

    } catch (err) {
      setError(err.message);
    }
  };

  return (
    <div className="login-container">
      <div className="login-left">
        <h1 className="site-name">Website name</h1>
        <h1 className="welcome-text">Your friends are waiting for you!</h1>
        <p>Log in quickly to check their news and share your own!</p>
      </div>
      <div className="login-right">
        <form onSubmit={handleSubmit}>
          <div className="input-field">
            <label htmlFor="email">Email</label>
            <input
              type="email"
              id="email"
              name="email"
              placeholder="Enter your email"
              required
              value={formData.email}
              onChange={handleChange}
            />
          </div>
          <div className="input-field">
            <label htmlFor="password">Password</label>
            <input
              type="password"
              id="password"
              name="password"
              placeholder="Enter your password"
              required
              value={formData.password}
              onChange={handleChange}
            />
          </div>
          <div className="checkbox-field">
            <input
              type="checkbox"
              id="rememberMe"
              name="rememberMe"
              className="checkbox"
              checked={formData.rememberMe}
              onChange={handleChange}
            />
            <label htmlFor="rememberMe" className="checkbox-text">
              Remember me
            </label>
          </div>
          <button type="submit" className="btn-submit">Log in</button>
          {error && <p className="error">{error}</p>}
          {message && <p className="success">{message}</p>}
        </form>
        
       
      </div>
    </div>
  );
}

export default Login;
