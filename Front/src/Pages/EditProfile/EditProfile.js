import React from "react";
import "./EditProfile.css";
import ProfileEditForm from "../../components/EditProfileForm/Form";
import MenuUser from "../../components/MenuUser/MenuUser";

const ProfileEditPage = () => {
  return (
    <div className="profile-edit-page">
      <MenuUser />
      <div className="profile-edit-content">
        <ProfileEditForm />
      </div>
    </div>
  );
};

export default ProfileEditPage;
