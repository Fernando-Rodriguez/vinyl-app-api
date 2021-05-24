import ApiProvider from './apiProvider';

class ApiCore {
  constructor(options) {
    if (options.getAll) {
      this.getAll = () => ApiProvider.getAll(options.url);
    }

    if (options.getSingle) {
      this.getSingle = (id) => ApiProvider.getSingle(options.url, id);
    }

    if (options.post) {
      this.post = (item) => ApiProvider.post(options.url, item);
    }

    if (options.put) {
      this.put = (item) => ApiProvider.put(options.url, item);
    }

    if (options.patch) {
      this.patch = (item) => ApiProvider.patch(options.url, item);
    }

    if (options.remove) {
      this.remove = (id) => ApiProvider.remove(options.url, id);
    }
  }
}

export default ApiCore;
