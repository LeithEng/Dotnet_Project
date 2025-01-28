import React from "react";
import CreateEvent from "../../components/CreateEvent/CreateEvent";
import MenuUser from "../../components/MenuUser/MenuUser";
import "./CreateEvent.css";

function CreateEventPage() {

    return (
        <div className="create-event-container">
            <MenuUser />
           <div class="event">
           <CreateEvent />
            </div> 
        </div>
    );
    }

export default CreateEventPage;