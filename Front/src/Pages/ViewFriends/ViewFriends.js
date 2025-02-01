import React, { useEffect, useState } from "react";
import Cookies from "js-cookie";
import MenuUser from "../../components/MenuUser/MenuUser";
import "./ViewFriends.css";

function ViewFriends() {
    const [friends, setFriends] = useState([]);
    const [searchTerm, setSearchTerm] = useState("");

    useEffect(() => {
        const fetchFriends = async () => {
            const token = Cookies.get("token")?.slice(1, -1);

            if (!token) {
                console.error("No authentication token found!");
                return;
            }

            try {
                const response = await fetch("http://localhost:5073/api/User/GetFriendsOfUser", {
                    method: "GET",
                    headers: {
                        "Authorization": `Bearer ${token}`,
                        "Content-Type": "application/json"
                    }
                });

                if (!response.ok) {
                    throw new Error("Failed to fetch friends");
                }

                const data = await response.json();
                setFriends(data);
            } catch (error) {
                console.error("Error fetching friends:", error);
            }
        };

        fetchFriends();
    }, []);

    const handleSearch = async () => {
        const token = Cookies.get("token")?.slice(1, -1);

        if (!token) {
            console.error("No authentication token found!");
            return;
        }

        try {
            const response = await fetch(`http://localhost:5073/api/User/SerchUser?searchTerm=${encodeURIComponent(searchTerm)}`, {
                method: "GET",
                headers: {
                    "Authorization": `Bearer ${token}`,
                    "Content-Type": "application/json"
                }
            });

            if (!response.ok) {
                throw new Error("Failed to search users");
            }

            const data = await response.json();
            setFriends(data);
        } catch (error) {
            console.error("Error searching users:", error);
        }
    };

    const handleSearchChange = (event) => {
        setSearchTerm(event.target.value);
    };

    const removeFriend = async (username) => {
        const token = Cookies.get("token")?.slice(1, -1);

        if (!token) {
            console.error("No authentication token found!");
            return;
        }

        try {
            const response = await fetch(`http://localhost:5073/api/User/removeFriend?username=${encodeURIComponent(username)}`, {
                method: "POST",
                headers: {
                    "Authorization": `Bearer ${token}`,
                    "Content-Type": "application/json"
                }
            });

            if (!response.ok) {
                const errorData = await response.json();
                console.error("Error details:", errorData);
                throw new Error("Failed to remove friend");
            }

            // Mettre à jour l'état en retirant l'ami supprimé
            setFriends(prevFriends => prevFriends.filter(friend => friend.userName !== username));
        } catch (error) {
            console.error("Error removing friend:", error);
        }
    };

    return (
        <div className="view-friends-page">
            <MenuUser />
            <div className="search-container">
                <input 
                    type="text" 
                    placeholder="Search for friends..." 
                    value={searchTerm} 
                    onChange={handleSearchChange} 
                />
                <button onClick={handleSearch}>Search</button>
            </div>
            <div className="view-friends-container">
                <div className="content">
                    <div className="friends-count-container">
                        <div className="friends-count">{friends.length}</div>
                    </div>
                    <div className="friends-list">
                        {friends.length > 0 ? (
                            friends.map((friend) => (
                                <div key={friend.id} className="friend-item">
                                    <span>{friend.firstName} {friend.lastName}</span>
                                    <span 
                                        className="remove-icon"
                                        onClick={() => removeFriend(friend.userName)}>
                                        <i className="fas fa-times"></i>
                                    </span>
                                </div>
                            ))
                        ) : (
                            <p>No friends</p>
                        )}
                    </div>
                </div>
            </div>
        </div>
    );
}

export default ViewFriends;
