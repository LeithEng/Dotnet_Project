import { useState } from "react";
import { useNavigate } from "react-router-dom"; // Import pour la redirection
import "./CreateHobby.css";

export default function HobbyPage() {
  const [hobbies, setHobbies] = useState([
    { level: 1, name: "", description: "", parentId: null }
  ]);
  const navigate = useNavigate(); // Hook pour la navigation

  const addHobbyLevel = () => {
    if (hobbies.length < 3) {
      const newLevel = hobbies.length + 1;
      const parentId = hobbies[hobbies.length - 1]?.id || null; // Le parentId est l'ID du hobby précédent
      setHobbies([
        ...hobbies,
        { level: newLevel, name: "", description: "", parentId }
      ]);
    }
  };

  const handleChange = (index, field, value) => {
    const newHobbies = [...hobbies];
    newHobbies[index][field] = value;
    setHobbies(newHobbies);
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      for (const hobby of hobbies) {
        const formData = new FormData();
        formData.append("Name", hobby.name);
        formData.append("Description", hobby.description);
        formData.append("Level", hobby.level);
        if (hobby.parentId) formData.append("ParentHobbyId", hobby.parentId);

        // L'icône n'est pas incluse ici, car elle n'est pas nécessaire
        const response = await fetch("http://localhost:5073/api/Hobby/CreateHobby", {
          method: "POST",
          body: formData,
        });

        if (!response.ok) {
          throw new Error("Erreur lors de l'enregistrement");
        }

        const data = await response.json();
        hobby.id = data.id; // Assurez-vous que l'ID est mis à jour après l'enregistrement
      }
      alert("Hobbies enregistrés avec succès");
    navigate("/ManageHobbies");

    } catch (error) {
      console.error("Erreur lors de l'enregistrement", error);
    }
  };

  return (
    <div className="hobby-container">
      <h1 className="hobby-title">Gestion des Hobbies</h1>
      <h2 className="hobby-subtitle">Ajouter un Hobby</h2>
      <form onSubmit={handleSubmit} className="hobby-form">
        {hobbies.map((hobby, index) => (
          <div key={index} className="hobby-card">
            <h3 className="hobby-level">Hobby Niveau {hobby.level}</h3>
            <input
              type="text"
              placeholder="Nom du hobby"
              value={hobby.name}
              onChange={(e) => handleChange(index, "name", e.target.value)}
              required
              className="hobby-input"
            />
            <textarea
              placeholder="Description"
              value={hobby.description}
              onChange={(e) => handleChange(index, "description", e.target.value)}
              required
              className="hobby-textarea"
            ></textarea>
            {hobby.parentId && <p className="hobby-parent">Parent ID: {hobby.parentId}</p>}
          </div>
        ))}
        {hobbies.length < 3 && (
          <button type="button" onClick={addHobbyLevel} className="hobby-add-button">
            Ajouter Niveau +
          </button>
        )}
        <button type="submit" className="hobby-submit-button">Enregistrer</button>
      </form>
    </div>
  );
}