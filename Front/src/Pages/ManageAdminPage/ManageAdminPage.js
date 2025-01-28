import React from "react";
import ManageAdmin from "../../components/ManageAdmin/ManageAdmin";
import MenuAdmin from "../../components/MenuAdmin/MenuAdmin";
import "./ManageAdminPage.css";

function CreateEventPage() {

    return (
        <div className="admin-container">
            <MenuAdmin />
           <div class="admin">
           <ManageAdmin />
            </div> 
        </div>
    );
    }

export default CreateEventPage;