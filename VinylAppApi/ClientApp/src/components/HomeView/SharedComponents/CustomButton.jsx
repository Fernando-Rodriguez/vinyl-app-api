import React from 'react';
import './CustomButton.css';

const CustomButton = (
  {
    clickHandler,
    children,
    theme,
    customStyle,
    size,
  },
) => (
  <button
    type="button"
    className={`${size} ${customStyle} ${theme} button-container`}
    onClick={clickHandler}
  >
    {children}
  </button>
);

export default CustomButton;
