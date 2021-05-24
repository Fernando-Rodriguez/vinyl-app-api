/* eslint-disable no-console */
import React from 'react';
import ReactDOM from 'react-dom';

import App from './components/App/App';

import reportWebVitals from './reportWebVitals';

import './index.css';
import 'dotenv/config';

ReactDOM.render(
  <React.StrictMode>
    <App />
  </React.StrictMode>,
  document.getElementById('root'),
);

reportWebVitals(console.log);
