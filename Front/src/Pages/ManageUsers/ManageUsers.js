import React, { useState, useEffect } from "react";
import MenuAdmin from "../../components/MenuAdmin/MenuAdmin";
import Search from "../../components/Search/Search";
import "./ManageUsers.css";

const ManageUsers = () => {
  const [users, setUsers] = useState([]);

  // Function to fetch users
  const fetchUsers = async () => {
    try {
      const response = await fetch("http://localhost:5073/api/User/GetAllUsers", {
        method: "GET",
        headers: { "Content-Type": "application/json" },
      });

      if (response.ok) {
        const data = await response.json();
        setUsers(data);
      } else {
        console.error("Failed to fetch users");
      }
    } catch (error) {
      console.error("Error fetching users:", error);
    }
  };

  useEffect(() => {
    fetchUsers();
  }, []);

  // Delete user function
  const deleteUser = async (username) => {
    try {
      const response = await fetch(`http://localhost:5073/api/User/DeleteUserByAdmin?username=${username}`, {
        method: "DELETE",
        headers: { "Content-Type": "application/json" },
      });

      if (response.ok) {
        fetchUsers(); // Refresh the user list
      } else {
        const error = await response.json();
        alert("Error deleting user: " + error);
      }
    } catch (error) {
      console.error("Error deleting user:", error);
    }
  };

  return (
    <div className="manage-users-page">
      <div className="menu-admin-container">
        <MenuAdmin />
      </div>

      <div className="search-container">
        <Search className="search" />
      </div>

      <div className="user-list-container">
        {users.length > 0 ? (
          users.map((user, index) => (
            <div className="user-item" key={index}>
              <span>{user.email}</span>
              <span>{user.userName}</span>
              <span className="delete-button" onClick={() => deleteUser(user.userName)}>
                <i className="fa-solid fa-trash"></i>
              </span>
            </div>
          ))
        ) : (
          <p>No users found</p>
        )}
      </div>
    </div>
  );
};

export default ManageUsers;
