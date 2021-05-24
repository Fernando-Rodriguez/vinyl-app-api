import React, { useState, createContext } from 'react';

export const SelectedAlbumContext = createContext();

// eslint-disable-next-line react/prop-types
const SelectedAlbumProvider = ({ children }) => {
  const [selectedAlbum, setSelectedAlbum] = useState([]);

  return (
    <SelectedAlbumContext.Provider value={[
      selectedAlbum,
      setSelectedAlbum]}
    >
      {children}
    </SelectedAlbumContext.Provider>
  );
};

export default SelectedAlbumProvider;
