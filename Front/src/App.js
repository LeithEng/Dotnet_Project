
import './App.css';
import React from "react";
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import MenuAdmin from './components/MenuAdmin/MenuAdmin';
import RegisterForm from './components/RegisterForm/RegisterForm';
import LoginForm from './components/LoginForm/LoginForm';
import Post from './components/post/post';
import CreatePost from './Pages/CreatePost/CreatePost';
import HomePage from './Pages/HomePage/HomePage';
function App() {
  return (
    <Router>
      <Routes>
        <Route path="/home" element={<HomePage/>} />
        <Route path="/Post" element={<CreatePost/>} />
      </Routes>
    </Router>
  );
}

export default App;
