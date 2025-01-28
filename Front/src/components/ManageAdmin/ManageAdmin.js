import React, { useState } from "react";
import "./ManageAdmin.css";

const ManageAdmin = ({ adminName = "Unknown", initialRoles = [] }) => {
  const [roles, setRoles] = useState(initialRoles);
  const [newRole, setNewRole] = useState("");

  const addRole = () => {
    if (newRole.trim()) {
      setRoles([...roles, newRole.trim()]);
      setNewRole("");
    }
  };

  const removeRole = (index) => {
    setRoles(roles.filter((_, i) => i !== index));
  };

  return (
    <div className="manage-admin-container">
      <h3>Manage admin: {adminName}</h3>
      <div className="roles-container">
      <h2>Roles</h2>
        {roles.map((role, index) => (
          <div key={index} className="role-item">
            <span>{role}</span>
            <button className="delete-button" onClick={() => removeRole(index)}>
              🗑️
            </button>
          </div>
        ))}
      </div>
      <div className="add-role-container">
        <input
          type="text"
          value={newRole}
          onChange={(e) => setNewRole(e.target.value)}
          placeholder="Enter role"
        />
        <button onClick={addRole} disabled={!newRole.trim()} class="add-button">Add role</button>
      </div>
      <div className="button-group">
        <button className="cancel-button">Cancel</button>
        <button className="confirm-button">Confirm</button>
      </div>
    </div>
  );
};

export default ManageAdmin;
