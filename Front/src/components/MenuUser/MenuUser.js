import './MenuUser.css'
import profilePic from '../../assets/images/ProfilePic.png';

function MenuUser() {
    return (
      <div className="menu-container">
        <div className="profile-section">
          <img src="" alt="Profile" className="profile-pic" />
          <div className="username">Username</div>
        </div>
        <ul className="menu-list">
          <li>Hobbies</li>
          <li>Favourites</li>
          <li>Events</li>
          <li>Friends</li>
          <li>Pay Premium Membership</li>
        </ul>
        <div className="logout-container">
          <img src="path/to/logout-icon.png" alt="" className="logout-icon" />
          <a href="#" className="logout-link">Log Out</a>
        </div>
      </div>
    );
  }
  
  export default MenuUser;
