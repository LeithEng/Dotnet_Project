
import './App.css';
import React from "react";
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import MenuAdmin from './components/MenuAdmin/MenuAdmin';
import RegisterForm from './components/RegisterForm/RegisterForm';
import LoginForm from './components/LoginForm/LoginForm';
import Post from './components/post/post';
import CreatePost from './Pages/CreatePost/CreatePost';
import HomePage from './Pages/HomePage/HomePage';
import Login from './Pages/Login/Login';
import Register from './Pages/Register/Register';
import CreateEventPage from './Pages/CreateEvent/CreateEvent';
function App() {
  return (
    <Router>
      <Routes>
        <Route path="/home" element={<HomePage/>} />
        <Route path="/Post" element={<CreatePost/>} />
        <Route path="/Login" element={<Login/>} />
        <Route path="/Register" element={<Register/>} />
        <Route path="/CreateEvent" element={<CreateEventPage/>} />
      </Routes>
    </Router>
  );
}

export default App;
