/* eslint-disable no-unused-vars */
import React, { useContext, useState } from 'react';
import { AlbumMethodContext } from '../../../../context/AlbumMethodProvider';
import { UserContext } from '../../../../context/UserProvider';
import CustomButton from '../../SharedComponents/CustomButton';
import EditOptions from '../EditOptions/EditOptions';
import './TableRowItem.css';

const TableRowItem = ({ album }) => {
  const { deleteAlbumHandler } = useContext(AlbumMethodContext);
  const { currentUser } = useContext(UserContext);

  const [isOpen, setIsOpen] = useState(false);

  const openHandler = () => {
    setIsOpen(!isOpen);
  };

  const editOptionsButtons = (
    <>
      <p className="row-item">
        <CustomButton type="button" clickHandler={openHandler}>Edit</CustomButton>
      </p>
      <p className="row-item">
        <CustomButton
          type="button"
          clickHandler={() => deleteAlbumHandler(album.idString)}
        >
          Delete
        </CustomButton>
      </p>
    </>
  );

  return (
    <div className="row-container-main">
      <div className={isOpen ? 'row-container open' : 'row-container'}>
        <p className="row-item">{album.artist}</p>
        <p className="row-item">{album.album}</p>
        <p className="row-item">{album.rating}</p>
        <p className="row-item">{album.user}</p>
        {currentUser.userName === album.user ? editOptionsButtons : <p className="row-item" />}
      </div>
      {isOpen && (
        <div className="edit-container-row">
          <EditOptions
            album={album}
            openHandler={openHandler}
          />
        </div>
      )}
    </div>
  );
};

export default TableRowItem;
