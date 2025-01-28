import React from "react";
import "./AdminProfile.css";

const AdminProfile = () => {
  return (
    <div className="profile-card">
      <div className="profile-header">
        <div className="profile-pic">Pic</div>
        <div className="profile-info">
          <div className="username">Username</div>
          <div className="role">Admin</div>
        </div>
      </div>
      <div className="profile-details">
        <div className="detail">
          <span>First name:</span>
        </div>
        <div className="detail">
          <span>Last name:</span>
        </div>
        <div className="detail">
          <span>Email:</span>
        </div>
        <br></br>
        <div className="detail">
          <span>Permissions:</span>
        </div>
      </div>
      <button className="edit-button">Edit profile</button>
    </div>
  );
};

export default AdminProfile;
