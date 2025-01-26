import React, { useState } from 'react';
import MenuAdmin from "../../components/MenuAdmin/MenuAdmin";
import "./AddAdmin.css";
const AddAdmin = () => {
    const [formData, setFormData] = useState({
      firstName: '',
      lastName: '',
      email: '',
      birthDate: '',
      role: '',
    });
  
    const roles = ['Admin', 'Editor', 'Viewer']; // Liste des rôles
  
    const handleChange = (e) => {
      const { name, value } = e.target;
      setFormData({ ...formData, [name]: value });
    };
  
    const handleSubmit = (e) => {
      e.preventDefault();
      console.log('Admin created:', formData);
     
    };
  
    return (
      <div className="add-admin-page">
        {/* MenuAdmin intégré en haut de la page */}
        <MenuAdmin />
  
        <div className="add-admin-container">
          
          <form className="add-admin-form" onSubmit={handleSubmit}>
            <div className="form-group">
              <label>
              First Name:
                <input
                  type="text"
                  name="firstName"
                  value={formData.firstName}
                  onChange={handleChange}
                  required
                />
              </label>
              <label>
              Last Name:
                <input
                  type="text"
                  name="lastName"
                  value={formData.lastName}
                  onChange={handleChange}
                  required
                />
              </label>
            </div>
            <div className="form-group">
              <label>
                Email :
                <input
                  type="email"
                  name="email"
                  value={formData.email}
                  onChange={handleChange}
                  required
                />
              </label>
            </div>
            <div className="form-group">
              <label>
              Birth Date:
                <input
                  type="date"
                  name="birthDate"
                  value={formData.birthDate}
                  onChange={handleChange}
                  required
                />
              </label>
            </div>
            <div className="form-group">
              <label>
                Role:
                <select
                  name="role"
                  value={formData.role}
                  onChange={handleChange}
                  required
                >
                  <option value="">Choose a role</option>
                  {roles.map((role, index) => (
                    <option key={index} value={role}>
                      {role}
                    </option>
                  ))}
                </select>
              </label>
            </div>
            <button type="submit" className="create-btn">
            Create Admin
            </button>
          </form>
        </div>
      </div>
    );
  };
  
  export default AddAdmin;
  