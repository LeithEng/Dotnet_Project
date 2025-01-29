import React from "react";
import MenuAdmin from "../../components/MenuAdmin/MenuAdmin";
import Search from "../../components/Search/Search";
import "./ManageRoles.css";

const ManageRoles = () => {
    const users = ['Utilisateur 1', 'Utilisateur 2', 'Utilisateur 3']; 

    return (
      <div className="manage-roles-page">
        <div className="menu-admin-container">
          <MenuAdmin />
        </div>

        <div className="search-container">
          <Search className="search" />
          <button className="add-role-button">Add Role</button> 
        </div>

        <div className="user-list-container">
          {users.map((user, index) => (
            <div className="user-item" key={index}>
              <span>{user}</span>
              <div className="action-buttons">
                <span className="edit-button">
                  <i className="fa-solid fa-pen" style={{ color: 'black' }}></i>
                </span>
                <span className="delete-button">
                  <i className="fa-solid fa-trash"></i>
                </span>
              </div>
            </div>
          ))}
        </div>
      </div>
    );
};

export default ManageRoles;
