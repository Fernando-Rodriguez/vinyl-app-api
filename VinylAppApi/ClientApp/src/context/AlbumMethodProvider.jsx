import React, { createContext, useContext } from 'react';
import { AlbumContext } from './AlbumProvider';
// import ApiService from '../../../Depreciated/api.service';
import albumApi from '../services/ApiEndpoints/AlbumApi.service';

export const AlbumMethodContext = createContext();

// eslint-disable-next-line react/prop-types
const AlbumMethodProvider = ({ children }) => {
  const {
    albums,
    setAlbums,
    setRefreshKey,
    refreshKey,
  } = useContext(AlbumContext);

  const addAlbumHandler = async (album) => {
    if (album !== null) {
      try {
        await albumApi.post(album);
        // await ApiService.postDataAsync(album);
        setRefreshKey(!refreshKey);
      } catch (err) {
        // eslint-disable-next-line no-console
        console.log(err);
      }
    }
  };
  const deleteAlbumHandler = async (id) => {
    if (id !== null) {
      try {
        // await ApiService.deleteDataAsync(id);
        const filteredAlbums = albums.filter((album) => {
          if (album.idString !== id) {
            return album;
          }
          return null;
        });
        setAlbums(filteredAlbums);
      } catch (err) {
        // eslint-disable-next-line no-console
        console.log(err.ToString());
      }
    }
  };
  const updateAlbumHandler = async (changes) => {
    /// await ApiService.updateDataAsync(userId, id, changes);
    await albumApi.put(changes);
    const newList = albums.map((album) => {
      if (album.idString === changes.id) {
        const newAlbum = album;
        newAlbum.album = changes.album;
        newAlbum.artist = changes.artist;
        newAlbum.rating = changes.rating;
        return newAlbum;
      }
      return album;
    });
    setAlbums(newList);
  };

  return (
    <AlbumMethodContext.Provider value={{
      addAlbumHandler,
      deleteAlbumHandler,
      updateAlbumHandler,
    }}
    >
      {children}
    </AlbumMethodContext.Provider>
  );
};

export default AlbumMethodProvider;
