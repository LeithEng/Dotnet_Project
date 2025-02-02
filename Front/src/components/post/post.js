import './Post.css'
import React from "react";

function Post()
{


    return (
        <div className="post-card">
          <div className="header">
            <div className="profile-pic">Pic</div>
            <div className="username">Username</div>
          </div>
          <div className="content">
            <p>This is the description of the post</p>
            <p className="hobby">Hobby1</p>
            <p className="date">Date</p>
          </div>
          <div className="reactions">
            <div className="likes">
              <button className="like-button">👍</button>
              <span className="like-count">220</span>
            </div>
            <div className="emojis">
              <button>👍</button>
              <button>❤️</button>
              <button>😂</button>
              <button>😢</button>
              <button>😡</button>
            </div>
          </div>
          <button className="comment-button">Add a comment</button>
        </div>
      );


}





export default Post;