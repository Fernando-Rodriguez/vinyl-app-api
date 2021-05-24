import ApiCore from '../ApiBase/apiCore';

const url = 'ownedalbums';

const albumApi = new ApiCore({
  getAll: true,
  getSingle: true,
  post: true,
  put: true,
  patch: true,
  delete: true,
  url,
});

export default albumApi;
