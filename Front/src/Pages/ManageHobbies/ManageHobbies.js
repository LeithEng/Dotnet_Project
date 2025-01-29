import React, { useState } from "react";
import MenuAdmin from "../../components/MenuAdmin/MenuAdmin";
import Search from "../../components/Search/Search";
import "./ManageHobbies.css";

const ManageHobbies = () => {
  const [hobbies, setHobbies] = useState([
    { id: 1, name: "New Hobby", subHobbies: [] },
  ]);
  const [editing, setEditing] = useState(null);

  const handleAddSubHobby = (id) => {
    const addSubHobby = (list) => {
      list.forEach((hobby) => {
        if (hobby.id === id) {
          const newId = Date.now();
          hobby.subHobbies.push({ id: newId, name: "New SubHobby", subHobbies: [] });
        } else {
          addSubHobby(hobby.subHobbies);
        }
      });
    };

    const newHobbies = [...hobbies];
    addSubHobby(newHobbies);
    setHobbies(newHobbies);
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

    const newHobbies = [...hobbies];
    updateHobbyName(newHobbies);
    setHobbies(newHobbies);
    setEditing(null);
  };

  const handleDelete = (id) => {
    const deleteHobby = (list) => {
      return list.filter((hobby) => {
        if (hobby.id === id) return false;
        hobby.subHobbies = deleteHobby(hobby.subHobbies);
        return true;
      });
    };

    const newHobbies = deleteHobby(hobbies);
    setHobbies(newHobbies);
  };

  const renderRows = (list, level = 0) => {
    return list.map((hobby) => (
      <React.Fragment key={hobby.id}>
        {/* Ligne principale */}
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
            {level < 2 && (
              <button
                className="add-btn"
                onClick={() => handleAddSubHobby(hobby.id)}
              >
                <i className="fa-solid fa-plus"></i>
              </button>
            )}
            <button
              className="edit-btn"
              onClick={() => setEditing(hobby.id)}
            >
              <i className="fa-solid fa-pen"></i>
            </button>
            <button
              className="delete-btn"
              onClick={() => handleDelete(hobby.id)}
            >
              <i className="fa-solid fa-trash"></i>
            </button>
          </td>
        </tr>
        {/* Sous-lignes */}
        {renderRows(hobby.subHobbies, level + 1)}
      </React.Fragment>
    ));
  };

  return (
    <div className="manage-hobbies">
      <MenuAdmin />
      <Search />

      <button
        className="add-hobby-btn"
        onClick={() =>
          setHobbies([
            ...hobbies,
            { id: Date.now(), name: "New Hobby", subHobbies: [] },
          ])
        }
      >
        Add a Hobby
      </button>

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
