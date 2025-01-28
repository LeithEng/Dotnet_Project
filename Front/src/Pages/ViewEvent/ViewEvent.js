import React from "react";
import "./ViewEvent.css";
import ViewEvent from "../../components/ViewEvent/ViewEvent";
import MenuUser from "../../components/MenuUser/MenuUser";

const ViewEventCard = () => {
  return (
    <div className="event-page">
      <MenuUser />
      <div className="event-content">
        <ViewEvent />
      </div>
    </div>
  );
};

export default ViewEventCard;
