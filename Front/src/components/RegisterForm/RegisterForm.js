import React, { useState } from "react";
import "./RegisterForm.css";

function RegisterForm() {
  const [formData, setFormData] = useState({
    firstName: "",
    lastName: "",
    email: "",
    password: "",
    birthDate: "",
  });

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData({ ...formData, [name]: value });
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    console.log("Form Submitted:", formData);
    alert("Registration successful!");
  };

  return (
    <div className="form-container">
      <div className="panel1">
        <h1 class="name">Website name</h1>
        <h2 class="join-text">Join us now!</h2>
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
            <label htmlFor="birthDate">Birth date</label>
            <input
              type="date"
              id="birthDate"
              name="birthDate"
              value={formData.birthDate}
              onChange={handleChange}
              required
            />
          </div>

          <button type="submit" className="submit-btn">
            Get started
          </button>
        </form>
      </div>
    </div>
  );
}

export default RegisterForm;
