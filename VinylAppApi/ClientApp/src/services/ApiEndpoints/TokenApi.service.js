import ApiCore from '../ApiBase/apiCore';
import ApiProvider from '../ApiBase/apiProvider';

const url = 'token';
const urlRefresh = 'token/refresh';
const urlLogout = 'token/logout';

const tokenApi = new ApiCore({
  getAll: false,
  getSingle: false,
  post: true,
  put: false,
  patch: false,
  delete: false,
  url,
});

tokenApi.refreshToken = async () => {
  await ApiProvider.post(urlRefresh);
};

tokenApi.logout = async () => {
  await ApiProvider.getAll(urlLogout);
};

export default tokenApi;
