import React from "react";
import "./ViewEvent.css";

const EventCard = ({ eventData }) => {
    return (
      <div className="event-card">
        <button className="close-btn">✕</button>
        <div className="event-left">
          <h2 className="event-title">{eventData?.title ?? "Untitled Event"}</h2>
          <div className="event-details">
            <p><strong>Organizer:</strong> {eventData?.organizer ?? "Unknown"}</p>
            <p><strong>Hobby:</strong> {eventData?.hobby ?? "Not specified"}</p>
            <p><strong>Event type:</strong> <span className="highlight">{eventData?.type ?? "General"}</span></p>
            <p><strong>Date:</strong> {eventData?.date ?? "TBD"}</p>
            <p><strong>Duration:</strong> {eventData?.duration ?? "Not available"}</p>
          </div>
          <div className="event-buttons">
            <button className="share-btn">Share</button>
            <button className="participate-btn">Participate</button>
          </div>
        </div>
        <div className="event-right">
          <p>{eventData?.description ?? "No description available."}</p>
        </div>
      </div>
    );
  };
  
  export default EventCard;