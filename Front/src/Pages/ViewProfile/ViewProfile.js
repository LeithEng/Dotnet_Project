import React, { useState } from "react";
import MenuUser from "../../components/MenuUser/MenuUser";
import "./ViewProfile.css";

const ViewProfile = () => {
  const [isEditing, setIsEditing] = useState(false);
  const [user, setUser] = useState({
    name: 'Hiba Chabbouh',
    photo: '/images/user-photo.jpg',
    firstName: 'Hiba',
    lastName: 'Chabbouh',
    email: 'hibachabbouh@gmail.com',
    birthDate: '2002-06-26'
  });

  // Fonction pour activer/désactiver le mode édition
  const toggleEdit = () => {
    setIsEditing(!isEditing);
  };

  // Fonction pour mettre à jour les données de l'utilisateur
  const handleChange = (e) => {
    const { name, value } = e.target;
    setUser(prevUser => ({
      ...prevUser,
      [name]: value
    }));
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

          {/* Bouton EditProfile */}
          <div className="edit-profile-btn">
            <button onClick={toggleEdit}>
              {isEditing ? 'Save Changes' : 'Edit Profile'}
            </button>
          </div>
        </div>
      </div>
    </div>
  );
};

export default ViewProfile;
