
import React from "react";
import MenuUser from "../../components/MenuUser/MenuUser";
import Search from "../../components/Search/Search";
import "./ViewEvents.css";


function ViewEvents() {
    return (
        <div className="view-events-page">
            <MenuUser />
            <div className="search-container">
                <Search />
            </div>
            <div className="create-event-button">
                <button>Create Event</button>
            </div>
            <div className="view-events-container">
                <div className="content">
                    <div className="events-list">
                        <div className="event-item">
                            <div className="event-header">
                                <span className="event-name">Event Name</span>
                                <span className="hobby-name">Hobby Name</span>
                            </div>
                            <div className="participate">
                                <i className="fa-solid fa-plus"></i>
                                <span className="participate-text">Participate</span>
                            </div>
                        </div>
                        <div className="event-item">
                            <div className="event-header">
                            <span className="event-name">Event Name</span>
                                <span className="hobby-name">Hobby Name</span>
                            </div>
                            <div className="participate">
                                <i className="fa-solid fa-plus"></i>
                                <span className="participate-text">Participate</span>
                            </div>
                        </div>
                        <div className="event-item">
                            <div className="event-header">
                            <span className="event-name">Event Name</span>
                                <span className="hobby-name">Hobby Name</span>
                            </div>
                            <div className="participate">
                                <i className="fa-solid fa-plus"></i>
                                <span className="participate-text">Participate</span>
                            </div>
                        </div>
                        <div className="event-item">
                            <div className="event-header">
                            <span className="event-name">Event Name</span>
                            <span className="hobby-name">Hobby Name</span>
                            </div>
                            <div className="participate">
                                <i className="fa-solid fa-plus"></i>
                                <span className="participate-text">Participate</span>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
}

export default ViewEvents;