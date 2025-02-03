import React, { useEffect, useState } from "react";
import Cookies from "js-cookie";
import MenuUser from "../../components/MenuUser/MenuUser";
import "./AddFriends.css";

function AddFriends() {
    const [users, setUsers] = useState([]);
    const [searchTerm, setSearchTerm] = useState("");
    const [friends, setFriends] = useState([]);

    useEffect(() => {
        const fetchUsers = async () => {
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
                        "Content-Type": "application/json",
                    },
                });

                if (!response.ok) {
                    throw new Error("Failed to search users");
                }

                const data = await response.json();
                setUsers(data);
            } catch (error) {
                console.error("Error fetching users:", error);
            }
        };

        fetchUsers();
    }, [searchTerm]);

    const handleSearchChange = (event) => {
        setSearchTerm(event.target.value);
    };

    const addFriend = async (username) => {
        const token = Cookies.get("token")?.slice(1, -1);

        if (!token) {
            console.error("No authentication token found!");
            return;
        }

        try {
            const response = await fetch(`http://localhost:5073/api/User/AddFriend?username=${encodeURIComponent(username)}`, {
                method: "POST",
                headers: {
                    "Authorization": `Bearer ${token}`,
                    "Content-Type": "application/json",
                },
            });

            if (!response.ok) {
                const errorData = await response.json();
                console.error("Error details:", errorData);
                throw new Error("Failed to add friend");
            }

            // Optionally refresh the list of friends after adding a friend
            alert("Friend added successfully!");
        } catch (error) {
            console.error("Error adding friend:", error);
        }
    };

    return (
        <div className="add-friends-page">
            <MenuUser />
            <div className="search-container">
                <input
                    type="text"
                    placeholder="Search for users..."
                    value={searchTerm}
                    onChange={handleSearchChange}
                />
                <button>Search</button>
            </div>
            <div className="users-list">
                {users.length > 0 ? (
                    users.map((user) => (
                        <div key={user.id} className="user-item">
                            <span>{user.firstName} {user.lastName}</span>
                            <button onClick={() => addFriend(user.userName)}>Add Friend</button>
                        </div>
                    ))
                ) : (
                    <p>No users found</p>
                )}
            </div>
        </div>
    );
}

export default AddFriends;
