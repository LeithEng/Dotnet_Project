import React from "react";
import "./HomePage.css";
import home from "../../assets/images/home.png";

const HomePage = () => {
  return (
    <div className="container">
      <div className="left-panel">
        <header>
          <h1>Website name and logo</h1>
        </header>
        <main>
          <h2>Discover a new hobby or share yours!</h2>
          <p>
            Join a platform that was specifically made for users to share their
            hobbies and interests
          </p>
          <div className="buttons">
            <button className="join-now">Join now</button>
            <button className="login">Log in</button>
          </div>
        </main>
      </div>
      <img src={home} alt="Home" class="home-pic" />
      <div className="right-panel"></div>
    </div>
  );
};

export default HomePage;
