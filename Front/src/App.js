
import './App.css';
import React from "react";
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import MenuAdmin from './components/MenuAdmin/MenuAdmin';
import RegisterForm from './components/RegisterForm/RegisterForm';
import LoginForm from './components/LoginForm/LoginForm';
import Post from './components/post/post';
import CreatePost from './Pages/CreatePost/CreatePost';
function App() {
  return (
    <Router>
      <Routes>
        <Route path="/" element={<CreatePost />} />
      </Routes>
    </Router>
  );
}

export default App;
