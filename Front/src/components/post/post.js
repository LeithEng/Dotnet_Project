import React from "react";
import { FaMapMarkerAlt, FaPaperclip, FaPlus, FaPen } from "react-icons/fa";
import "./post.css";

function Post() {
  return (
    <div className="post-container">
      <div className="post-header">
        <h3>Hobby</h3>
      </div>
      <div className="post-content">
        <div className="textarea-container">
          <textarea
            id="text"
            placeholder="Write something..."
            className="post-input"
          ></textarea>
          <div className="icon-container">
            <FaMapMarkerAlt className="icon" />
            <FaPaperclip className="icon" />
            <FaPen className="icon" />
          </div>
        </div>
      </div>
    </div>
  );
}

export default Post;
