import React, { useState, useRef, useEffect } from "react";
import Cookies from "js-cookie";
import { useNavigate } from "react-router-dom";
import "./Form.css";

const ProfileEditForm = () => {
  const navigate = useNavigate();
  const [formData, setFormData] = useState({
    firstName: "",
    lastName: "",
    email: "",
    password: "",
    profilePic: null,
  });

  const fileInputRef = useRef(null); // Reference to file input

  // Load user data on mount
  useEffect(() => {
    const fetchUserProfile = async () => {
      const token = Cookies.get("token").slice(1, -1);
      if (!token) {
        console.error("No token found");
        return;
      }

      try {
        const response = await fetch("http://localhost:5073/api/User/GetProfile", {
          method: "GET",
          headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${token}`,
          },
          credentials: "include",
        });

        if (!response.ok) {
          throw new Error("Failed to fetch user profile");
        }

        const data = await response.json();
        setFormData({
          firstName: data.firstName,
          lastName: data.lastName,
          email: data.email,
          password: "", 
          profilePic: data.avatar || null,
        });
      } catch (error) {
        console.error("Error fetching user profile:", error);
      }
    };

    fetchUserProfile();
  }, []);

  const handleChange = (e) => {
    setFormData({ ...formData, [e.target.name]: e.target.value });
  };

  const handleFileChange = (e) => {
    const file = e.target.files[0];
    if (file) {
      const reader = new FileReader();
      reader.readAsDataURL(file); // Read as base64 string
      reader.onloadend = () => {
        setFormData((prev) => ({ ...prev, profilePic: reader.result }));
      };
    }
  };

  const handleDeletePic = () => {
    setFormData((prev) => ({ ...prev, profilePic: null }));
  };

  const handleSubmit = async (event) => {
    event.preventDefault();
    const token = Cookies.get("token").slice(1, -1);
      if (!token) {
        console.error("No token found");
        return;
      }

    // Fetch the current user data if it's not already available in the form state
    const response = await fetch("http://localhost:5073/api/User/GetProfile", {
        method: "GET",
        headers: {
            "Authorization": `Bearer ${token}`,
        }
    });

    const currentUser = await response.json();

    const updatedUser = {
        avatar: formData.avatar || currentUser.avatar, // Use existing value if not changed
        userName: formData.userName || currentUser.userName, // Keep existing username
        firstName: formData.firstName || currentUser.firstName,
        lastName: formData.lastName || currentUser.lastName,
        email: formData.email || currentUser.email,
        password: formData.password || currentUser.password,  
    };

    try {
        const updateResponse = await fetch("http://localhost:5073/api/User/UpdateProfile", {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                "Authorization": `Bearer ${token}`,
            },
            body: JSON.stringify(updatedUser),
        });

        if (updateResponse.ok) {
          console.log("Profile updated successfully!");
          navigate("/profile");
        } else {
          console.error("Error updating profile:", updateResponse.statusText);
        }
      } catch (error) {
        console.error("Error:", error);
      }
};


  return (
    <form className="profile-edit-form" onSubmit={handleSubmit}>
      <div className="profile-header">
        <div className="profile-pic">
          {formData.profilePic ? (
            <img
              src={formData.profilePic}
              alt="Profile"
            />
          ) : (
            "No Profile Picture"
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
        <input
          type="text"
          name="firstName"
          placeholder="First Name"
          value={formData.firstName}
          onChange={handleChange}
        />
        <input
          type="text"
          name="lastName"
          placeholder="Last Name"
          value={formData.lastName}
          onChange={handleChange}
        />
      </div>

      <div className="form-group">
        <input
          type="email"
          name="email"
          placeholder="Email"
          value={formData.email}
          onChange={handleChange}
        />
      </div>

      <div className="form-group">
        <input
          type="password"
          name="password"
          value={formData.password}
          onChange={handleChange}
        />
      </div>

      <div className="form-group">
        <input
          type="text"
          name="username"
          placeholder="Username"
          value={formData.userName}
          onChange={handleChange}
        />
        </div>

      <button type="submit" className="save-btn">
        Save Changes
      </button>
    </form>
  );
};

export default ProfileEditForm;
