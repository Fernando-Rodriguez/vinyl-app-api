import React from 'react';
import Stars from '../../Star/Stars';
import './MoreInfo.css';

const MoreInfo = ({ album }) => (
  <div className="info-container">
    <div className="info-item-header">
      <div className="info-item">{ album.artist }</div>
      <div className="info-item">{ album.album }</div>
      <div className="info-item">{album.user}</div>
      <div className="info-item info-star-rating">
        <Stars numStars={album.rating} />
      </div>
    </div>
  </div>
);

export default MoreInfo;
