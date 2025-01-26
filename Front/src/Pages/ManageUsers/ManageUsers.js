import React from "react";
import MenuAdmin from "../../components/MenuAdmin/MenuAdmin";
import Search from "../../components/Search/Search";
import "./ManageUsers.css";

const ManageUsers = () => {
    const users = ['Utilisateur 1', 'Utilisateur 2', 'Utilisateur 3']; 
  
    return (
      <div className="manage-users-page">
        {}
        <div className="menu-admin-container">
          <MenuAdmin />
        </div>
  
        {}
        <div className="search-container">
          <Search className="search" />
        </div>
  
        {}
        <div className="user-list-container">
          {users.map((user, index) => (
            <div className="user-item" key={index}>
              <span>{user}</span>
              <span className="delete-button">
  <i className="fa-solid fa-trash"></i>
</span>

            </div>
          ))}
        </div>
      </div>
    );
  };
  
  export default ManageUsers;
  