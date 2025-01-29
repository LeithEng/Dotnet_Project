import React, { useState } from "react";
import "./RegisterForm.css";

function RegisterForm() {
  const [formData, setFormData] = useState({
    firstName: "",
    lastName: "",
    email: "",
    password: "",
    confirmPassword: "",
  });

  const [error, setError] = useState("");

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData({ ...formData, [name]: value });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    if (formData.password !== formData.confirmPassword) {
      setError("Passwords do not match");
      return;
    }

    setError("");

    try {
      const response = await fetch("http://localhost:5073/api/authentication/Register", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({
          firstName: formData.firstName,
          lastName: formData.lastName,
          email: formData.email,
          password: formData.password,
          confirmPassword: formData.confirmPassword,
        }),
      });

      if (!response.ok) {
        const data = await response.json();
        if (data.errors && data.errors.Password) {
          const passwordErrors = data.errors.Password.join(", ");
          setError(passwordErrors);
        } else {
          setError(data.message || "Failed to register.");
        }
  
        return;
      }

      alert("Registration successful!");
    } catch (err) {
      console.error(err);
      alert("An error occurred: " + err.message);
    }
  };

  return (
    <div className="form-container">
      <div className="panel1">
        <h1 className="name">Website name</h1>
        <h2 className="join-text">Join us now!</h2>
        <p>
          Join a community of like-minded hobbyists and showcase your favorite
          activities. Your next great hobby adventure starts here—create your
          account now and start sharing!
        </p>
      </div>

      <div className="panel2">
        <form onSubmit={handleSubmit}>
          <div className="form-group-row">
            <div className="form-group">
              <label htmlFor="firstName">First name</label>
              <input
                type="text"
                id="firstName"
                name="firstName"
                value={formData.firstName}
                onChange={handleChange}
                required
              />
            </div>
            <div className="form-group">
              <label htmlFor="lastName">Last name</label>
              <input
                type="text"
                id="lastName"
                name="lastName"
                value={formData.lastName}
                onChange={handleChange}
                required
              />
            </div>
          </div>

          <div className="form-group">
            <label htmlFor="email">Email</label>
            <input
              type="email"
              id="email"
              name="email"
              value={formData.email}
              onChange={handleChange}
              required
            />
          </div>

          <div className="form-group">
            <label htmlFor="password">Password</label>
            <input
              type="password"
              id="password"
              name="password"
              value={formData.password}
              onChange={handleChange}
              required
            />
          </div>

          <div className="form-group">
            <label htmlFor="confirmPassword">Confirm Password</label>
            <input
              type="password"
              id="confirmPassword"
              name="confirmPassword"
              value={formData.confirmPassword}
              onChange={handleChange}
              required
            />
          </div>

          {error && <p className="error-message">{error}</p>}

          <button type="submit" className="submit-btn">
            Get started
          </button>
        </form>
      </div>
    </div>
  );
}

export default RegisterForm;
