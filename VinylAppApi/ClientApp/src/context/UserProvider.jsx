import React, { createContext, useState } from 'react';
import { useHistory, useLocation } from 'react-router-dom';
import tokenApi from '../services/ApiEndpoints/TokenApi.service';
import userApi from '../services/ApiEndpoints/UserApi.service';

export const UserContext = createContext();

// eslint-disable-next-line react/prop-types
const UserProvider = ({ children }) => {
  const [currentUser, setCurrentUser] = useState(null);
  const [isLoggedin, setIsLoggedIn] = useState(() => {
    const loginStatus = localStorage.getItem('isUserLoggedIn') ?? null;
    return loginStatus;
  });
  const history = useHistory();
  const location = useLocation();
  const { from } = location.state || { from: { pathname: '/' } };

  const GetCurrentUser = async () => {
    const user = await userApi.getCurrentUser();
    setCurrentUser(user);
  };

  const SignInHandler = async (email, password) => {
    await tokenApi.post({
      userName: email,
      userSecret: password,
    });
    await GetCurrentUser();
    localStorage.setItem('isUserLoggedIn', true);
    setIsLoggedIn(true);
    history.replace(from);
  };

  const SignOutHandler = async () => {
    await tokenApi.logout();
    history.replace('/login');
    localStorage.removeItem('isUserLoggedIn');
    setIsLoggedIn(false);
    setCurrentUser(null);
  };

  const UserCreation = async (email, password) => {
    await userApi.post({
      userName: email,
      userSecret: password,
    });
  };

  return (
    <UserContext.Provider value={{
      currentUser,
      SignInHandler,
      SignOutHandler,
      UserCreation,
      GetCurrentUser,
      isLoggedin,
    }}
    >
      {children}
    </UserContext.Provider>
  );
};

export default UserProvider;
