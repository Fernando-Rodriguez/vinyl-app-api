import axios from 'axios';
import Responses from './apiResponse';

axios.defaults.baseURL = process.env.REACT_APP_API_BASE;
axios.defaults.headers = {
  Accept: 'application/json',
  'Content-Type': 'application/json',
};
axios.defaults.withCredentials = true;

const ApiProvider = {
  getAll: async (resource) => {
    try {
      const dbRes = await axios.get(`/${resource}`);
      return Responses.ResponseHandler(dbRes);
    } catch (e) {
      return Responses.ErrorHandler(e);
    }
  },
  getSingle: async (resource, id) => {
    try {
      const dbRes = await axios.get(`/${resource}/${id}`);
      return Responses.ResponseHandler(dbRes);
    } catch (e) {
      return Responses.ErrorHandler(e);
    }
  },
  post: async (resource, item) => {
    try {
      const dbRes = await axios.post(`/${resource}`, item);
      return Responses.ResponseHandler(dbRes);
    } catch (e) {
      return Responses.ErrorHandler(e);
    }
  },
  put: async (resource, item) => {
    try {
      const dbRes = await axios.put(`/${resource}`, item);
      return Responses.ResponseHandler(dbRes);
    } catch (e) {
      return Responses.ErrorHandler(e);
    }
  },
  patch: async (resource, item) => {
    try {
      const dbRes = await axios.patch(`/${resource}`, item);
      return Responses.ResponseHandler(dbRes);
    } catch (e) {
      return Responses.ErrorHandler(e);
    }
  },
  delete: async (resource, id) => {
    try {
      const dbRes = await axios.delete(`/${resource}`, id);
      return Responses.ResponseHandler(dbRes);
    } catch (e) {
      return Responses.ErrorHandler(e);
    }
  },
};

export default ApiProvider;
