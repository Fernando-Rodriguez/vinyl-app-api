const Responses = {
  ResponseHandler: (res) => {
    if (res.results) {
      return res.results;
    }
    if (res.data) {
      return res.data;
    }
    return res;
  },
  ErrorHandler: (err) => {
    if (err.data) {
      return err.data;
    }
    return err;
  },
};

export default Responses;
