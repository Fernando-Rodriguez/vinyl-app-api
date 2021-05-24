import ApiCore from '../ApiBase/apiCore';
import ApiProvider from '../ApiBase/apiProvider';

const url = 'users';

const userApi = new ApiCore({
  getAll: false,
  getSingle: true,
  post: false,
  put: false,
  patch: false,
  delete: false,
  url,
});

userApi.getCurrentUser = async () => {
  const user = await ApiProvider.getAll(url);
  return user;
};

export default userApi;
