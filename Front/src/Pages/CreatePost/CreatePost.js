import React from "react";
import Post from "../../components/Post/post";
import MenuUser from "../../components/MenuUser/MenuUser";
import "./CreatePost.css";

function CreatePost() {

    return (
        <div className="create-post-container">
            <MenuUser />
            <Post />
        </div>
    );
    }

export default CreatePost;