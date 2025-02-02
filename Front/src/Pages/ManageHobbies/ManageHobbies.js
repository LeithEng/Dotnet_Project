import React, { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom"; // Import pour la redirection
import Cookies from "js-cookie";
import MenuAdmin from "../../components/MenuAdmin/MenuAdmin";
import Search from "../../components/Search/Search";
import "./ManageHobbies.css";

const API_BASE_URL = "http://localhost:5073/api/Hobby";

const ManageHobbies = () => {
  const [hobbies, setHobbies] = useState([]);
  const [editing, setEditing] = useState(null);
  const navigate = useNavigate(); // Hook pour la navigation

  useEffect(() => {
    fetchHobbies();
  }, []);

  const fetchHobbies = async () => {
    const token = Cookies.get("token")?.slice(1, -1); // Vérifie si le token existe
    try {
      const response = await fetch(`${API_BASE_URL}/first-level`);
      if (!response.ok) throw new Error("Failed to fetch hobbies");

      const data = await response.json();
      console.log("Fetched hobbies:", data);
      setHobbies(data);
    } catch (error) {
      console.error("Error fetching hobbies:", error);
    }
  };

  const handleDelete = async (id) => {
    try {
      const response = await fetch(`${API_BASE_URL}/delete/${id}`, {
        method: "DELETE",
        headers: { "Content-Type": "application/json" },
      });

      if (!response.ok) throw new Error("Failed to delete hobby");

      setHobbies((prevHobbies) => prevHobbies.filter((hobby) => hobby.id !== id));
    } catch (error) {
      console.error("Error deleting hobby:", error);
    }
  };

  const handleEdit = (id, newName) => {
    const updateHobbyName = (list) => {
      list.forEach((hobby) => {
        if (hobby.id === id) {
          hobby.name = newName;
        } else {
          updateHobbyName(hobby.subHobbies);
        }
      });
    };
  };

  const renderRows = (list, level = 0) => {
    return list.map((hobby) => (
      <React.Fragment key={hobby.id}>
        <tr>
          <td className="hobby-name" style={{ paddingLeft: `${level * 20}px` }}>
            {editing === hobby.id ? (
              <input
                type="text"
                defaultValue={hobby.name}
                onBlur={(e) => handleEdit(hobby.id, e.target.value)}
                autoFocus
              />
            ) : (
              hobby.name
            )}
          </td>
          <td className="hobby-actions">
            <button className="edit-btn" onClick={() => setEditing(hobby.id)}>
              <i className="fa-solid fa-pen"></i>
            </button>
            <button className="delete-btn" onClick={() => handleDelete(hobby.id)}>
              <i className="fa-solid fa-trash"></i>
            </button>
          </td>
        </tr>
      </React.Fragment>
    ));
  };

  return (
    <div className="manage-hobbies">
      <MenuAdmin />
      <div className="top-bar">
        <Search />
        <button className="add-hobby-btn" onClick={() => navigate("/Hobby")}>
          + Ajouter un hobby
        </button>
      </div>
      <table className="hobbies-table">
        <thead>
          <tr>
            <th>Nom</th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>{renderRows(hobbies)}</tbody>
      </table>
    </div>
  );
};

export default ManageHobbies;
