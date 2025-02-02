import React, { useState, useEffect } from "react";
import { useParams, useNavigate } from "react-router-dom";
import Cookies from "js-cookie";
import "./EditHobby.css";

const API_BASE_URL = "http://localhost:5073/api/Hobby";

const EditHobby = () => {
  const { id } = useParams();
  const navigate = useNavigate();
  const [hobby, setHobby] = useState({
    name: "",
    description: "",
    level: "",
    parentHobbyId: "",
  });
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    fetchHobby();
  }, []);

  const fetchHobby = async () => {
    try {
      const response = await fetch(`${API_BASE_URL}/details/${id}`);
      if (!response.ok) throw new Error("Failed to fetch hobby");
      const data = await response.json();
      setHobby(data);
      setLoading(false);
    } catch (error) {
      setError(error.message);
      setLoading(false);
    }
  };

  const handleChange = (e) => {
    setHobby({ ...hobby, [e.target.name]: e.target.value });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    if (!hobby.level) {
      setError("Level is required");
      return;
    }

    const formData = new FormData();
    formData.append("Id", hobby.id);
    formData.append("Name", hobby.name);
    formData.append("Description", hobby.description);
    formData.append("Level", hobby.level);
    formData.append("ParentHobbyId", hobby.parentHobbyId || "");

    // Si tu veux envoyer une image, ajoute :
    // formData.append("IconPicture", fileInput.files[0]);

    try {
      const response = await fetch(`${API_BASE_URL}/update/${hobby.id}`, {
        method: "PUT",
        headers: {
          "Accept": "application/json",
        },
        body: formData,
      });

      if (!response.ok) {
        const errorText = await response.text();
        throw new Error(`Failed to update hobby: ${errorText}`);
      }

      navigate("/ManageHobbies");
    } catch (error) {
      console.error("Error updating hobby:", error);
      setError(error.message);
    }
  };

  if (loading) return <p>Loading...</p>;
  if (error) return <p>Error: {error}</p>;

  return (
    <div className="edit-hobby">
      <h2>Edit Hobby</h2>
      <form onSubmit={handleSubmit}>
        <label>Nom:</label>
        <input
          type="text"
          name="name"
          value={hobby.name}
          onChange={handleChange}
          required
        />
        <br />

        <label>Description:</label>
        <textarea
          name="description"
          value={hobby.description}
          onChange={handleChange}
          required
        />
        <br />

        <label>Niveau:</label>
        <select
          name="level"
          value={hobby.level}
          onChange={handleChange}
          required
        >
          <option value="">Select Level</option>
          <option value="1">1</option>
          <option value="2">2</option>
          <option value="3">3</option>
        </select>
        <br />

        <label>Parent Hobby:</label>
        <input
          type="text"
          name="parentHobbyId"
          value={hobby.parentHobbyId}
          onChange={handleChange}
        />
        <br />

        <button type="submit">Mettre à jour</button>
        <button type="button" onClick={() => navigate("/ManageHobbies")}>Cancel</button>
      </form>
    </div>
  );
};

export default EditHobby;
