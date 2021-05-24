/* eslint-disable no-console */
import React, {
  createContext,
  useContext,
  useEffect,
  useState,
} from 'react';
import { SearchContext } from './SearchContext';
import groupApi from '../services/ApiEndpoints/GroupApi.service';
import albumApi from '../services/ApiEndpoints/AlbumApi.service';

export const AlbumContext = createContext();

// eslint-disable-next-line react/prop-types
const AlbumProvider = ({ children }) => {
  const [albums, setAlbums] = useState([]);
  const [coreAlbums, setCoreAlbums] = useState([]);
  const [groups, setGroups] = useState([]);
  const [currentGroup, setCurrentGroup] = useState('');
  const [refreshKey, setRefreshKey] = useState(false);
  const [search] = useContext(SearchContext);

  useEffect(() => {
    const RefreshAlbums = async () => {
      try {
        const dbAlbums = await albumApi.getAll();
        const dbGroup = await groupApi.getAll();
        setAlbums(dbAlbums.owned_Albums);
        setCoreAlbums(dbAlbums.owned_Albums);
        setGroups(dbGroup);
      } catch (err) {
        console.log(err.toString());
      }
    };
    RefreshAlbums();
  }, [refreshKey]);

  useEffect(() => {
    try {
      if (currentGroup.groupId === 'all') {
        setAlbums(coreAlbums);
      } else {
        const filteredGroup = groups.filter((group) => group.groupId === currentGroup.groupId);
        const addGroupAlbums = filteredGroup[0].groupAlbums;
        const newAlbumList = [...coreAlbums, ...addGroupAlbums];
        setAlbums(newAlbumList);
      }
    } catch (err) {
      console.log(err.toString());
    }
  }, [currentGroup]);

  useEffect(() => {
    if (search !== '') {
      const list = albums.filter((album) => {
        const inputToLower = search.toLowerCase();
        return (
          album.artist.toLowerCase().includes(inputToLower)
          || album.album.toLowerCase().includes(inputToLower)
        );
      });
      setAlbums(list);
    }
    return () => setAlbums(coreAlbums);
  }, [search]);

  return (
    <AlbumContext.Provider
      value={{
        albums,
        setAlbums,
        groups,
        setCurrentGroup,
        currentGroup,
        setRefreshKey,
        refreshKey,
      }}
    >
      {children}
    </AlbumContext.Provider>
  );
};

export default AlbumProvider;
