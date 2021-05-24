function debounce1(func, wait, immediate) {
  let timeout;

  return (...args) => {
    const context = this;

    const callNow = immediate && !timeout;
    clearTimeout(timeout);

    timeout = setTimeout(() => {
      timeout = null;
      if (!immediate) {
        func.apply(context, args);
      }
    }, wait);

    if (callNow) {
      func.apply(context, args);
    }
  };
}

export default debounce1;
