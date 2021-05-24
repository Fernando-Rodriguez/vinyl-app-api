/* eslint-disable no-unused-vars */
import './App.css';
import React from 'react';
import {
  BrowserRouter as Router,
  Switch,
  Route,
} from 'react-router-dom';
import HomeContainer from '../HomeView/HomeContainer/HomeContainer';
import UserProvider from '../../context/UserProvider';
import AlbumProvider from '../../context/AlbumProvider';
import AlbumMethodProvider from '../../context/AlbumMethodProvider';
import SearchProvider from '../../context/SearchContext';
import PrivateRoute from '../RouterRoutes/PrivateRoute';
import Login from '../LoginView/Login';

const App = () => (
  <div className="App">
    <Router>
      <UserProvider>
        <Switch>
          <Route exact path="/login">
            <Login />
          </Route>
          <PrivateRoute path="/">
            <SearchProvider>
              <AlbumProvider>
                <AlbumMethodProvider>
                  <HomeContainer />
                </AlbumMethodProvider>
              </AlbumProvider>
            </SearchProvider>
          </PrivateRoute>
        </Switch>
      </UserProvider>
    </Router>
  </div>
);

export default App;
