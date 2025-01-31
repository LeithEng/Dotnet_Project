import { useNavigate } from "react-router-dom";
import Cookies from "js-cookie";
import './MenuAdmin.css'
import profilePic from '../../assets/images/ProfilePic.png';


function MenuAdmin() {

  const navigate = useNavigate();

  const handleLogout = async () => {
    try {
      const token = Cookies.get("token").slice(1,-1); // Retrieve the stored JWT token
      const response = await fetch("http://localhost:5073/api/authentication/Logout", {
        method: "POST",
        headers: {
          "Authorization": `Bearer ${token}`,
          "Content-Type": "application/json"
        }
      });

      if (response.ok) {
        Cookies.remove("token"); // Remove token from storage
        navigate("/login"); // Redirect to login page
      } else {
        console.error("Logout failed");
      }
    } catch (error) {
      console.error("An error occurred:", error);
    }
  };

  return (
    <div className="menu-container">
      <div className="profile-section">
      <img src={profilePic} alt="Profile" className="profile-pic" />
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
      <div className="logout-container" onClick={handleLogout}>
        <i className="fa-solid fa-right-from-bracket logout-icon"></i>
        <span className="logout-link">Log Out</span>
      </div>
    </div>
  );
}

export default MenuAdmin;
