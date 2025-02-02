import React, { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom"; // Import pour la redirection
import Cookies from "js-cookie";
import MenuAdmin from "../../components/MenuAdmin/MenuAdmin";
import Search from "../../components/Search/Search";
import "./ManageHobbies.css";

const API_BASE_URL = "http://localhost:5073/api/Hobby";

const ManageHobbies = () => {
  const [hobbies, setHobbies] = useState([]);
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

  const renderRows = (list, level = 0) => {
    return list.map((hobby) => (
      <React.Fragment key={hobby.id}>
        <tr>
          <td className="hobby-name" style={{ paddingLeft: `${level * 20}px` }}>
            {hobby.name}
          </td>
          <td className="hobby-actions">
            <button className="edit-btn" onClick={() => navigate(`/EditHobby/${hobby.id}`)}>
              <i className="fa-solid fa-pen"></i>
            </button>
            <button className="delete-btn" onClick={() => handleDelete(hobby.id)}>
              <i className="fa-solid fa-trash"></i>
            </button>
          </td>
        </tr>
        {/* Ajouter ici un appel pour récupérer les enfants d'un hobby */}
        <ChildrenHobbies hobbyId={hobby.id} level={level + 1} />
      </React.Fragment>
    ));
  };
  
  // Composant pour récupérer et afficher les enfants d'un hobby donné
  const ChildrenHobbies = ({ hobbyId, level }) => {
    const [children, setChildren] = useState([]);
  
    useEffect(() => {
      const fetchChildren = async () => {
        const response = await fetch(`${API_BASE_URL}/children/${hobbyId}`);
        const data = await response.json();
        setChildren(data);
      };
  
      fetchChildren();
    }, [hobbyId]);
  
    return renderRows(children, level);
  };

  return (
    <div className="manage-hobbies">
      <MenuAdmin />
      <div className="top-bar">
        <Search />
        <button className="add-hobby-btn" onClick={() => navigate("/Hobby")}>+ Ajouter un hobby</button>
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
