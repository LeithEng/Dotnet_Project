import './MenuUser.css'
import profilePic from '../../assets/images/ProfilePic.png';

function MenuUser() {
    return (
      <div className="menu-container">
        <div className="profile-section">
          <img src={profilePic} alt="Profile" className="profile-pic" />
          <div className="username">Username</div>
        </div>
        <ul className="menu-list">
          <li>Hobbies</li>
          <li>Events</li>
          <li>Friends</li>
          <li>Pay Premium Membership</li>
        </ul>
        <div className="logout-container">
        <i className="fa-solid fa-right-from-bracket logout-icon"></i>
          <a href="#" className="logout-link">Log Out</a>
        </div>
      </div>
    );
  }
  
  export default MenuUser;
