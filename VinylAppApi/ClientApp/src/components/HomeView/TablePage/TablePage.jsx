/* eslint-disable no-unused-vars */
import React, { useContext, useState } from 'react';
import { AlbumContext } from '../../../context/AlbumProvider';
import AddAlbum from './AddAlbum/AddAlbum';
import TableRowItem from './TableRow/TableRowItem';
import CustomButton from '../SharedComponents/CustomButton';
import './TablePage.css';

const TablePage = () => {
  const {
    albums,
  } = useContext(AlbumContext);

  const [addAlbumBool, setAddAlbumBool] = useState(false);

  const closeHandler = () => {
    setAddAlbumBool(!addAlbumBool);
  };

  const TableRows = () => {
    if (albums !== undefined) {
      return (
        <div className="table-row-body">
          {albums.map((album) => (
            <TableRowItem key={album.idString} album={album} />
          ))}
        </div>
      );
    }

    return (
      <div>Loading...</div>
    );
  };

  return (
    <div className="table-container">
      <div className="add-albums-container">
        <CustomButton
          customStyle="med-button"
          clickHandler={() => setAddAlbumBool(!addAlbumBool)}
        >
          {!addAlbumBool ? 'Add Album' : 'Close' }
          <i className="margined far fa-plus-square" />
        </CustomButton>
        {addAlbumBool && <AddAlbum closeHandler={closeHandler} />}
      </div>
      <div className="table-whole">
        <div className="table-header-row">
          <p className="table-header-item">Artist</p>
          <p className="table-header-item">Album</p>
          <p className="table-header-item">Rating</p>
          <p className="table-header-item">User</p>
        </div>
        {TableRows()}
      </div>
    </div>
  );
};

export default TablePage;
