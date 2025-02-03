import React, { useEffect, useState } from "react";
import MenuUser from "../../components/MenuUser/MenuUser";
import { jwtDecode } from "jwt-decode";
import Cookies from "js-cookie";
import Search from "../../components/Search/Search";
import "./ViewHobbies.css";

const ViewHobbies = () => {
  const [hobbies, setHobbies] = useState([]);
  const [likedHobbies, setLikedHobbies] = useState(new Set());
  const [userId, setUserId] = useState(""); 
  const token = Cookies.get("token").slice(1,-1);

  useEffect(() => {
    if (token) {
      try {
        const decodedToken = jwtDecode(token);
        const id = decodedToken.userId || decodedToken.sub;
        setUserId(id); 
        console.log("User ID:", id);
      } catch (error) {
        console.error("Invalid token:", error);
      }
    } else {
      console.log("No token found.");
    }
  }, [token]); 

  useEffect(() => {
    if (userId) {
      fetchHobbies();
      fetchFavoriteHobbies();
    }
  }, [userId]);

  
  const fetchHobbies = async () => {
    try {
      const response = await fetch("http://localhost:5073/api/Hobby/first-level", {
        method: "GET",
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${token}`,
        },
      });

      if (!response.ok) throw new Error("Failed to fetch hobbies");

      const data = await response.json();
      setHobbies(data);
    } catch (error) {
      console.error("Error fetching hobbies:", error);
    }
  };

  const fetchFavoriteHobbies = async () => {
    try {
      const response = await fetch(`http://localhost:5073/FavoriteHobby/${userId}`, {
        method: "GET",
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${token}`,
        },
      });

      if (!response.ok) throw new Error("Failed to fetch favorite hobbies");

      const data = await response.json();
      setLikedHobbies(new Set(data));
    } catch (error) {
      console.error("Error fetching favorite hobbies:", error);
    }
  };

  const toggleLike = async (hobbyId) => {
    const isLiked = likedHobbies.has(hobbyId);

    try {
      if (isLiked) {
      
        await fetch(`http://localhost:5073/FavoriteHobby/remove/${userId}/${hobbyId}`, {
          method: "DELETE",
          headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${token}`,
          },
        });

        setLikedHobbies((prev) => {
          const newSet = new Set(prev);
          newSet.delete(hobbyId);
          return newSet;
        });
      } else {
    
        await fetch("http://localhost:5073/FavoriteHobby/Create", {
          method: "POST",
          headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${token}`,
          },
          body: JSON.stringify({ userId, hobbyId }),
        });

        setLikedHobbies((prev) => new Set(prev).add(hobbyId));
      }
    } catch (error) {
      console.error("Error updating favorite hobby:", error);
    }
  };

  return (
    <div className="view-hobbies-page">
      <div className="menu-user-container">
        <MenuUser />
      </div>

      <div className="search-container">
        <Search className="search" />
      </div>

      <div className="hobby-list-container">
        {hobbies.map((hobby) => (
          <div className="hobby-item" key={hobby.id}>
            <span>{hobby.name}</span>
            <span className="like-button" onClick={() => toggleLike(hobby.id)}>
              <i className={`fa-solid fa-heart ${likedHobbies.has(hobby.id) ? "liked" : ""}`}></i>
            </span>
          </div>
        ))}
      </div>
    </div>
  );
};

export default ViewHobbies;
