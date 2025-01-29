import React from "react";
import MenuUser from "../../components/MenuUser/MenuUser";
import Search from "../../components/Search/Search";
import "./ViewFriends.css";

function ViewFriends() {
    return (
        <div className="view-friends-page">
            <MenuUser />
            <div className="search-container">
        <Search />
    </div>
            <div className="view-friends-container">
                <div className="content">
                <div className="friends-count-container">
    <div className="friends-count">123</div>
</div>
                    <div className="friends-list">
                        <div className="friend-item">
                            <span>John Doe</span>
                            <span className="remove-icon">&#x274C;</span>
                        </div>
                        <div className="friend-item">
                            <span>Jane Smith</span>
                            <span className="remove-icon">&#x274C;</span>
                        </div>
                        <div className="friend-item">
                            <span>Michael Johnson</span>
                            <span className="remove-icon">&#x274C;</span>
                        </div>
                        <div className="friend-item">
                            <span>Emily Davis</span>
                            <span className="remove-icon">&#x274C;</span>
                        </div>
                        <div className="friend-item">
                            <span>Chris Lee</span>
                            <span className="remove-icon">&#x274C;</span>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
}

export default ViewFriends;
