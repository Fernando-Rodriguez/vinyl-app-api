/* eslint-disable no-unused-vars */
import React, { useContext, useState } from 'react';
import { UserContext } from '../../context/UserProvider';
import CustomButton from '../HomeView/SharedComponents/CustomButton';
import './Login.css';

const Login = () => {
  const {
    SignInHandler,
    UserCreation,
  } = useContext(UserContext);

  const [userInput, setUserInput] = useState('');
  const [passInput, setPassInput] = useState('');
  const [isLoading, setIsLoading] = useState(false);

  const LoginUser = async () => {
    setIsLoading(true);
    await SignInHandler(userInput, passInput);
  };

  const onUserChangeHandler = (value) => {
    setUserInput(value);
  };

  const onPassChange = (value) => {
    setPassInput(value);
  };

  const onCreateAccount = async () => {
    if (userInput && passInput) {
      setIsLoading(true);
      await UserCreation(userInput, passInput);
      setIsLoading(false);
    }
  };

  const LoginSection = () => (
    <div className="input-items-container">
      <div className="input-items">
        <h1>Welcome to the Vinyl App</h1>
        <h2>Sign In Here</h2>
        <div className="input-field-container">
          <input
            className="login-input"
            onChange={(e) => onUserChangeHandler(e.target.value)}
            placeholder="enter username"
          />
        </div>
        <div className="input-field-container">
          <input
            className="login-input"
            onChange={(e) => onPassChange(e.target.value)}
            placeholder="enter password"
            type="password"
          />
        </div>
        <CustomButton
          clickHandler={LoginUser}
          theme="light"
        >
          Sign In
        </CustomButton>
      </div>
      <div
        className="input-create-account"
        onClick={onCreateAccount}
        role="button"
        tabIndex="0"
        onKeyDown={() => true}
      >
        Create Account
      </div>
    </div>
  );

  const LoadingSection = () => (
    <div className="input-items">
      <h1>Loading...</h1>
    </div>
  );

  return (
    <div className="login-container">
      <div className="side-bar-container">
        <img src="" alt="" />
      </div>
      <div className="login-input-container">
        {isLoading ? LoadingSection() : LoginSection()}
      </div>
    </div>
  );
};

export default Login;
