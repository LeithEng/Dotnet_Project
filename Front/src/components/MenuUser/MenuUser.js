import { useNavigate } from "react-router-dom";
import Cookies from "js-cookie";
import './MenuUser.css';
import { useState, useEffect } from 'react';

function MenuUser() {
  const navigate = useNavigate();
  const [userData, setUserData] = useState(null);

  useEffect(() => {
    const fetchUserProfile = async () => {
      const token = Cookies.get("token").slice(1, -1);
      if (!token) {
        navigate("/login");
        return;
      }

      try {
        const response = await fetch("http://localhost:5073/api/user/GetProfile", {
          method: "GET",
          headers: {
            "Authorization": `Bearer ${token}`,
            "Content-Type": "application/json"
          }
        });

        if (response.ok) {
          const data = await response.json();
          setUserData(data);
        } else {
          console.error("Failed to fetch user data");
        }
      } catch (error) {
        console.error("An error occurred:", error);
      }
    };

    fetchUserProfile();
  }, [navigate]);

  const handleLogout = async () => {
    try {
      const token = Cookies.get("token").slice(1, -1); 
      const response = await fetch("http://localhost:5073/api/authentication/Logout", {
        method: "POST",
        headers: {
          "Authorization": `Bearer ${token}`,
          "Content-Type": "application/json"
        }
      });

      if (response.ok) {
        Cookies.remove("token"); 
        navigate("/login");
      } else {
        console.error("Logout failed");
      }
    } catch (error) {
      console.error("An error occurred:", error);
    }
  };

  const handleNavigation = (page) => {
    navigate(`/${page}`);
  };

  return (
    <div className="menu-container">
      {userData ? (
        <>
          <div className="profile-section">
            <img
              src={userData.avatar ? `data:image/jpeg;base64,${userData.avatar}` : require('../../assets/images/ProfilePic.png')}
              alt="Profile"
              className="profile-pic"
            />
            <div className="username">{userData.userName}</div>
          </div>
          <ul className="menu-list">
            <li onClick={() => handleNavigation("ViewHobbies")}>Hobbies</li>
            <li onClick={() => handleNavigation("ViewEvents")}>Events</li>
            <li onClick={() => handleNavigation("ViewFriends")}>Friends</li>
          </ul>
          <div className="logout-container" onClick={handleLogout}>
            <i className="fa-solid fa-right-from-bracket logout-icon"></i>
            <span className="logout-link">Log Out</span>
          </div>
        </>
      ) : (
        <p>Loading...</p>
      )}
    </div>
  );
}

export default MenuUser;
