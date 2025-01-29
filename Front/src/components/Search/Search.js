import React from 'react';
import './Search.css'; 

const Search = () => {
  return (
    <button className="search-button">
      <span className="search-icon">
  <i className="fa-solid fa-magnifying-glass"></i>
</span>
      <span className="search-text">Search</span>
    </button>
  );
};

export default Search;
