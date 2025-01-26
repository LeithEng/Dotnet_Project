import React from "react";
import MenuUser from "../../components/MenuUser/MenuUser";
import Search from "../../components/Search/Search";
import "./ViewHobbies.css";

const ViewHobbies = () => {
    const hobbies = ['Football', 'Basketball', 'Lecture', 'Cinéma']; 
    const [likedHobbies, setLikedHobbies] = React.useState({});
  
    // Fonction pour gérer l'ajout et la suppression des hobbies favoris
    const toggleLike = (hobby) => {
      setLikedHobbies((prev) => ({
        ...prev,
        [hobby]: !prev[hobby],
      }));
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
          {hobbies.map((hobby, index) => (
            <div className="hobby-item" key={index}>
              <span>{hobby}</span>
              <span
                className="like-button"
                onClick={() => toggleLike(hobby)}
              >
                <i 
                  className={`fa-solid fa-heart ${likedHobbies[hobby] ? 'liked' : ''}`} 
                ></i>
              </span>
            </div>
          ))}
        </div>
      </div>
    );
  };
  
  export default ViewHobbies;
  
