import React from 'react';

const Popup = ({ popupHandler }) => (
  <div className="PopUp">
    <button
      className="popup-x"
      onClick={() => popupHandler(false)}
      type="button"
    >
      X
    </button>
    <div className="pu-content-container">
      <img className="pu-img" src="/" alt="bone" />
      <h1>Add more bones?</h1>
    </div>
    <div className="pu-button-container">
      <button
        onClick={() => popupHandler(false)}
        type="button"
      >
        MORE BONES!
      </button>
      <button
        onClick={() => popupHandler(false)}
        type="button"
      >
        No, thank you.
      </button>
    </div>
  </div>
);

export default Popup;
