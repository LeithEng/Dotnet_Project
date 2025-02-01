import React, { useState, useEffect } from "react";
import Cookies from "js-cookie";
import { useNavigate } from "react-router-dom";  // Import useNavigate from react-router-dom
import MenuUser from "../../components/MenuUser/MenuUser";
import "./ViewProfile.css";

const ViewProfile = () => {
  const [isEditing, setIsEditing] = useState(false);
  const [user, setUser] = useState({
    name: '',
    photo: '',
    firstName: '',
    lastName: '',
    email: '',
  });
  const token = Cookies.get("token");
  const navigate = useNavigate();  // Create navigate function

  useEffect(() => {
    const fetchUserProfile = async () => {
      const token = Cookies.get("token").slice(1,-1);

      console.log("Retrieved token:", token); // Debugging step

      if (!token) {
        console.error("No authentication token found!");
        return;
      }

      try {
        const response = await fetch("http://localhost:5073/api/User/GetProfile", {
          method: "GET",
          headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${token}`, // Include token in request headers
          },
        });

        if (!response.ok) {
          throw new Error(`Failed to fetch user profile. Status: ${response.status}`);
        }

        const data = await response.json();
        console.log("User data:", data);
        setUser({
          name: `${data.firstName} ${data.lastName}`,
          photo: data.avatar || "/images/default-avatar.jpg",
          firstName: data.firstName,
          lastName: data.lastName,
          email: data.email,
        });
      } catch (error) {
        console.error("Error fetching user profile:", error);
      }
    };

    fetchUserProfile();
  }, [token]);

  const toggleEdit = () => {
    setIsEditing(!isEditing);
  };

  const handleChange = (e) => {
    const { name, value } = e.target;
    setUser(prevUser => ({
      ...prevUser,
      [name]: value
    }));
  };

  const handleSave = async () => {
    const token = Cookies.get("token").slice(1,-1);

    if (!token) {
      console.error("No authentication token found!");
      return;
    }

    try {
      const response = await fetch("http://localhost:5073/api/User/UpdateProfile", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${token}`,
        },
        body: JSON.stringify({
          firstName: user.firstName,
          lastName: user.lastName,
          email: user.email,
          userName: user.firstName + user.lastName,
          password: "",
        }),
      });

      if (!response.ok) {
        throw new Error("Failed to update profile");
      }

      alert("Profile updated successfully!");
      setIsEditing(false);
    } catch (error) {
      console.error("Error updating profile:", error);
    }
  };

  const redirectToEditPage = () => {
    // Use navigate to redirect to the edit profile page
    navigate("/EditProfile");
  };

  return (
    <div className="profile-page">
      <MenuUser />

      <div className="profile-container">
        <div className="profile-header">
          <img 
            src={user.photo} 
            alt="User Profile" 
            className="profile-image"
          />
          <div className="profile-name">
            <p>{user.name}</p>
          </div>
        </div>

        <div className="profile-details">
          <div className="row">
            <div className="col">
              <div className="label">First Name:</div>
              {isEditing ? (
                <input
                  type="text"
                  name="firstName"
                  value={user.firstName}
                  onChange={handleChange}
                  className="value"
                />
              ) : (
                <span className="value">{user.firstName}</span>
              )}
            </div>
            <div className="col">
              <div className="label">Last Name:</div>
              {isEditing ? (
                <input
                  type="text"
                  name="lastName"
                  value={user.lastName}
                  onChange={handleChange}
                  className="value"
                />
              ) : (
                <span className="value">{user.lastName}</span>
              )}
            </div>
          </div>

          <div className="row">
            <div className="col">
              <div className="label">Email:</div>
              {isEditing ? (
                <input
                  type="email"
                  name="email"
                  value={user.email}
                  onChange={handleChange}
                  className="value"
                />
              ) : (
                <span className="value">{user.email}</span>
              )}
            </div>
          </div>

          <div className="edit-profile-btn">
            {isEditing ? (
              <button onClick={handleSave}>Save Changes</button>
            ) : (
              <button onClick={redirectToEditPage}>Edit Profile</button>
            )}
          </div>
        </div>
      </div>
    </div>
  );
};

export default ViewProfile;
