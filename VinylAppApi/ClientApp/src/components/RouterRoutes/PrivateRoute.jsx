/* eslint-disable arrow-body-style */
/* eslint-disable no-unused-vars */
import React, { useContext } from 'react';
import { Route, Redirect } from 'react-router-dom';
import { UserContext } from '../../context/UserProvider';

const PrivateRoute = ({ children, ...rest }) => {
  const {
    currentUser,
    isLoggedin,
  } = useContext(UserContext);

  return (
    <Route
      // eslint-disable-next-line react/jsx-props-no-spreading
      {...rest}
      render={({ location }) => (isLoggedin ? (children) : (
        <Redirect
          to={{
            pathname: '/login',
            state: { from: location },
          }}
        />
      ))}
    />
  );
};

export default PrivateRoute;
