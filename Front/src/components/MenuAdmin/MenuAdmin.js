
import './MenuAdmin.css'
import profilePic from '../../assets/images/ProfilePic.png';
import "@fortawesome/fontawesome-free/css/all.min.css";

function MenuAdmin() {
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
        <li>Manage hobbies</li>
        <li>Manage users</li>
      </ul>
      <div className="logout-container">
        <i className="fa-solid fa-right-from-bracket logout-icon"></i>
        <a href="#" className="logout-link">Log Out</a>
      </div>
    </div>
  );
}

export default MenuAdmin;