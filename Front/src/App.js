
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
      </Routes>
    </Router>
  );
}

export default App;
