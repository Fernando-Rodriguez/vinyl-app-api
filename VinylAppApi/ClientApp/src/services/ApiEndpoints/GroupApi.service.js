import ApiCore from '../ApiBase/apiCore';

const url = 'group';

const groupApi = new ApiCore({
  getAll: true,
  getSingle: true,
  post: true,
  put: true,
  patch: true,
  delete: true,
  url,
});

export default groupApi;
