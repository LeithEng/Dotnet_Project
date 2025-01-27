import React, { useState } from "react";
import "./CreateEvent.css";

const CreateEvent = () => {
  const [formData, setFormData] = useState({
    title: "",
    hobby: "",
    eventType: "",
    date: "",
    duration: "",
    description: "",
  });

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData({ ...formData, [name]: value });
  };

  const handleSubmit = () => {
    console.log("Event Created:", formData);
  };

  return (
    <div className="modal-container">
      <div className="modal-card">
        <div className="modal-header">
          <h2>Create new event</h2>
          <button className="close-button">&times;</button>
        </div>
        <p className="modal-description">Add the details of your event</p>
        <form className="modal-form">
          <input
            type="text"
            name="title"
            placeholder="Title"
            className="input-field"
            value={formData.title}
            onChange={handleChange}
          />
          <input
            type="text"
            name="hobby"
            placeholder="Hobby"
            className="input-field"
            value={formData.hobby}
            onChange={handleChange}
          />
          <input
            type="text"
            name="eventType"
            placeholder="Event type"
            className="input-field"
            value={formData.eventType}
            onChange={handleChange}
          />
          <input
            type="date"
            name="date"
            className="input-field"
            value={formData.date}
            onChange={handleChange}
          />
          <input
            type="text"
            name="duration"
            placeholder="Duration"
            className="input-field"
            value={formData.duration}
            onChange={handleChange}
          />
          <textarea
            name="description"
            placeholder="Description"
            className="textarea-field"
            value={formData.description}
            onChange={handleChange}
          ></textarea>
          <div className="button-group">
            <button type="button" className="cancel-button" onClick={() => setFormData({})}>
              Cancel
            </button>
            <button type="button" className="create-button" onClick={handleSubmit}>
              Create
            </button>
          </div>
        </form>
      </div>
    </div>
  );
};

export default CreateEvent;