import React, { useState } from "react";
import "./Form.css";

const ProfileEditForm = () => {
  const [formData, setFormData] = useState({
    firstName: "",
    lastName: "",
    email: "",
    password: "",
    birthDate: "",
  });

  const handleChange = (e) => {
    setFormData({ ...formData, [e.target.name]: e.target.value });
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    alert("Profile updated!");
  };

  return (
    <form className="profile-edit-form" onSubmit={handleSubmit}>
      <div className="profile-header">
        <div className="profile-pic">Pic</div>
        <button type="button" className="btn change-btn">Change</button>
        <button type="button" className="btn delete-btn">Delete</button>
      </div>

      <div className="form-group">
        <input
          type="text"
          name="firstName"
          placeholder="First name"
          value={formData.firstName}
          onChange={handleChange}
        />
        <input
          type="text"
          name="lastName"
          placeholder="Last name"
          value={formData.lastName}
          onChange={handleChange}
        />
      </div>

      <input
        type="email"
        name="email"
        placeholder="Email"
        value={formData.email}
        onChange={handleChange}
      />

      <input
        type="password"
        name="password"
        placeholder="Password"
        value={formData.password}
        onChange={handleChange}
      />

      

      <button type="submit" className="save-btn">Save changes</button>
    </form>
  );
};

export default ProfileEditForm;
