/* eslint-disable react/jsx-one-expression-per-line */
/* eslint-disable jsx-a11y/click-events-have-key-events */
/* eslint-disable no-unused-vars */
import React, { useContext } from 'react';
import { Link } from 'react-router-dom';
import { UserContext } from '../../../context/UserProvider';
import './NavBar.css';

const NavBar = () => {
  const {
    currentUser,
    SignOutHandler,
  } = useContext(UserContext);

  const imageIURL = 'https://images.pexels.com/photos/1616470/pexels-photo-1616470.jpeg?auto=compress&cs=tinysrgb&dpr=2&h=750&w=1260';

  const clickHandler = async () => {
    await SignOutHandler();
  };

  return (
    <div className="navbar-container">
      <div className="navbar-img">
        <img
          className="navbar-image-item"
          src={imageIURL}
          alt="Album Artwork"
        />
      </div>
      <h3>
        Welcome, {currentUser ? currentUser.userName : 'no one'}!
      </h3>
      <div className="navbar-links">
        <Link to="/" className="navbar-links-item">
          <i className="fas fa-home iconItem" />
          Home
        </Link>
        <Link to="/add-album" className="navbar-links-item">
          <i className="far fa-plus-square iconItem" />
          Add
        </Link>
        <div
          role="button"
          tabIndex="0"
          className="navbar-links-item"
          onClick={clickHandler}
        >
          <i className="fas fa-sign-out-alt iconItem" />
          Logout
        </div>
      </div>
      <div className="navbar-bar" />
    </div>
  );
};

export default NavBar;
