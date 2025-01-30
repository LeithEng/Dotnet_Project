import React, { useState, useRef } from "react";
import "./Form.css";

const ProfileEditForm = () => {
  const [formData, setFormData] = useState({
    firstName: "",
    lastName: "",
    email: "",
    password: "",
    profilePic: null,
  });

  const fileInputRef = useRef(null); // Reference to file input

  const handleChange = (e) => {
    setFormData({ ...formData, [e.target.name]: e.target.value });
  };

  const handleFileChange = (e) => {
    const file = e.target.files[0];
    if (file) {
      const reader = new FileReader();
      reader.readAsArrayBuffer(file);
      reader.onloadend = () => {
        const byteArray = new Uint8Array(reader.result);
        setFormData((prev) => ({ ...prev, profilePic: Array.from(byteArray) }));
      };
    }
  };

  const handleDeletePic = () => {
    setFormData((prev) => ({ ...prev, profilePic: null }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    const payload = {
      firstName: formData.firstName,
      lastName: formData.lastName,
      email: formData.email,
      password: formData.password,
      profilePic: formData.profilePic,
    };

    try {
      const response = await fetch("http://localhost:5073/api/profile/update", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(payload),
      });

      if (!response.ok) {
        throw new Error("Failed to update profile.");
      }

      alert("Profile updated successfully!");
    } catch (error) {
      console.error(error);
      alert("Error updating profile.");
    }
  };

  return (
    <form className="profile-edit-form" onSubmit={handleSubmit}>
      <div className="profile-header">
        <div className="profile-pic">
          {formData.profilePic ? (
            <img
              src={URL.createObjectURL(new Blob([new Uint8Array(formData.profilePic)]))}
              alt="Profile"
            />
          ) : (
            "Pic"
          )}
        </div>
        
        {/* Hidden File Input */}
        <input
          type="file"
          accept="image/*"
          ref={fileInputRef}
          style={{ display: "none" }}
          onChange={handleFileChange}
        />

        {/* Buttons */}
        <button type="button" className="btn change-btn" onClick={() => fileInputRef.current.click()}>
          Change
        </button>
        <button type="button" className="btn delete-btn" onClick={handleDeletePic}>
          Delete
        </button>
      </div>

      <div className="form-group">
        <input type="text" name="firstName" placeholder="First name" value={formData.firstName} onChange={handleChange} />
        <input type="text" name="lastName" placeholder="Last name" value={formData.lastName} onChange={handleChange} />
      </div>

      <input type="email" name="email" placeholder="Email" value={formData.email} onChange={handleChange} />
      <input type="password" name="password" placeholder="Password" value={formData.password} onChange={handleChange} />

      <button type="submit" className="save-btn">Save changes</button>
    </form>
  );
};

export default ProfileEditForm;
