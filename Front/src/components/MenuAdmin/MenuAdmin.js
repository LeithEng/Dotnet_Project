import './MenuAdmin.css'

import profilePic from '../../assets/images/ProfilePic.png';

function MenuUser() {
    return (
      <div className="menu-container">
        <div className="profile-section">
          <img src="" alt="Profile" className="profile-pic" />
          <div className="adminame">Adminname</div>
        </div>
        <ul className="menu-list">
          <li>Add admin</li>
          <li>Add role</li>
          <li>Dashboard</li>
          <li>Manage admins</li>
          <li>Manage roles</li>
          <li>Mange hobbies</li>
          <li>Mange users</li>
         
        </ul>
        <div className="logout-container">
          <img src="path/to/logout-icon.png" alt="" className="logout-icon" />
          <a href="#" className="logout-link">Log Out</a>
        </div>
      </div>
    );
  }
  
  export default MenuUser;
