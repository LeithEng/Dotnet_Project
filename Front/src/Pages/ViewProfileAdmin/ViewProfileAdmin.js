import React from "react";
import "./ViewProfileAdmin.css";
import AdminProfile from "../../components/AdminProfile/AdminProfile";
import MenuAdmin from "../../components/MenuAdmin/MenuAdmin";

const AdminProfilePage = () => {
  return (
    <div className="profile-page">
      <MenuAdmin />
      <div className="profile-content">
        <AdminProfile />
      </div>
    </div>
  );
};

export default AdminProfilePage;
