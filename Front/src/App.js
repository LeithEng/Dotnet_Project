
import './App.css';
import React from "react";
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import ViewPost from './components/ViewPost/ViewPost';
import CreatePost from './Pages/CreatePost/CreatePost';
import HomePage from './Pages/HomePage/HomePage';
import Login from './Pages/Login/Login';
import Register from './Pages/Register/Register';
import CreateEventPage from './Pages/CreateEvent/CreateEvent';
import AddAdmin from './Pages/AddAdmin/AddAdmin';
import ViewEvents from './Pages/ViewEvents/ViewEvents';
import ViewProfileAdmin from './Pages/ViewProfileAdmin/ViewProfileAdmin';
import EditProfile from './Pages/EditProfile/EditProfile';
import ViewEventCard from './Pages/ViewEvent/ViewEvent';
import ManageAdminPage from './Pages/ManageAdminPage/ManageAdminPage';
import ViewProfile from './Pages/ViewProfile/ViewProfile';
import ViewFriends from './Pages/ViewFriends/ViewFriends';
import ManageUsers from './Pages/ManageUsers/ManageUsers';
import ManageHobbies from './Pages/ManageHobbies/ManageHobbies';
import HobbyPage from './Pages/CreateHobby/CreateHobby';
import EditHobby from './Pages/EditHobby/EditHobby';
import ViewHobbies from './Pages/ViewHobbies/ViewHobbies';
import ManageAdmins from './Pages/ManageAdmins/ManageAdmins';
import AddFriends from './Pages/AddFriends/AddFriends';
function App() {
  return (
    <Router>
      <Routes>
        <Route path="/home" element={<HomePage/>} />
        <Route path="/Post" element={<CreatePost/>} />
        <Route path="/Login" element={<Login/>} />
        <Route path="/Register" element={<Register/>} />
        <Route path="/CreateEvent" element={<CreateEventPage/>} />
        <Route path="/ViewPost" element={<ViewPost/>} />
        <Route path="/AddAdmin" element={<AddAdmin/>} />
        <Route path="/ViewEvents" element={<ViewEvents/>} />
        <Route path="/AdminProfile" element={<ViewProfileAdmin/>} />
        <Route path="/EditProfile" element={<EditProfile/>} />
        <Route path="/ViewEvent" element={<ViewEventCard/>} />
        <Route path="/ManageAdmin" element={<ManageAdminPage/>} />
        <Route path="/Profile" element={<ViewProfile/>} />
        <Route path="/ViewFriends" element={<ViewFriends/>} />
        <Route path="/ManageUsers" element={<ManageUsers/>} />
        <Route path="/ManageHobbies" element={<ManageHobbies/>} />
        <Route path="/Hobby" element={<HobbyPage/>} />
        <Route path="/EditHobby/:id" element={<EditHobby/>} />
        <Route path="/ViewHobbies" element={<ViewHobbies/>} />
        <Route path="/ManageAdmins" element={<ManageAdmins/>} />
        <Route path="/AddFriends" element={<AddFriends/>} />

      </Routes>
    </Router>
  );
}

export default App;
