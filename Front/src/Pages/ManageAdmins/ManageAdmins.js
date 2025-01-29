import React from "react";
import MenuAdmin from "../../components/MenuAdmin/MenuAdmin";
import Search from "../../components/Search/Search";
import "./ManageAdmins.css";

function ManageAdmins() {

    const handleDelete = (adminName) => {
        console.log(`${adminName} supprimé`);
        
    };

    return (
        <div className="manage-admins-page">
            
            <div className="menu-admin-container">
                <MenuAdmin />
            </div>

            <div className="search-container">
                <Search className="search" />
            </div>

            <div className="admin-list-container">
                <div className="admin-item">
                    <span>Admin 1</span>
                    <span 
                        className="delete-button" 
                     
                    >
                        &times; {/* Le symbole de la croix */}
                    </span>
                </div>
                <div className="admin-item">
                    <span>Admin 2</span>
                    <span 
                        className="delete-button" 
                       
                    >
                        &times;
                    </span>
                </div>
                <div className="admin-item">
                    <span>Admin 3</span>
                    <span 
                        className="delete-button" 
                      
                    >
                        &times;
                    </span>
                </div>
            </div>
        </div>
    );
}

export default ManageAdmins;
