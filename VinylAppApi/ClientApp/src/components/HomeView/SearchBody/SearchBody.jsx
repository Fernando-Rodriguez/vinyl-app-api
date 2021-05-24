import React from 'react';
import { useHistory } from 'react-router-dom';
import AlbumCard from './AlbumCard/AlbumCard';
import './SearchBody.css';

const SearchBody = ({ album }) => {
  const history = useHistory();

  const clickHandler = (id) => {
    history.push(`/album/${id}`);
  };

  if (album === undefined || album === null) {
    return (
      <div>No albums to show...</div>
    );
  }

  const albumArray = (userAlbums) => (
    <div className="searchbody-container">
      {userAlbums.map((dBalbum) => (
        <AlbumCard
          className="searchbody-cell"
          album={dBalbum}
          key={`${dBalbum.idString}-${dBalbum.user}`}
          clickHandler={clickHandler}
        />
      ))}
    </div>
  );

  return (
    albumArray(album)
  );
};

export default SearchBody;
