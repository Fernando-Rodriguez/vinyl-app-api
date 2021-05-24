/* eslint-disable no-unused-vars */
/* eslint-disable no-console */
import React, {
  useState,
  useEffect,
  useRef,
  useContext,
} from 'react';
import { AlbumContext } from '../../../context/AlbumProvider';
import CustomButton from '../SharedComponents/CustomButton';
import './DropDown.css';

const DropDown = ({ dropDownListArray }) => {
  const dropdownReference = useRef(null);
  const [isActive, setIsActive] = useState(false);
  const [dropDownSelection, setDropDownSelection] = useState({});
  const {
    setCurrentGroup,
  } = useContext(AlbumContext);

  useEffect(() => {
    const pageClickEvent = (e) => {
      if (dropdownReference.current !== null && !dropdownReference.current.contains(e.target)) {
        setIsActive(!isActive);
      }
      console.log(e);
    };

    if (isActive) {
      window.addEventListener('click', pageClickEvent);
    }
    return () => {
      window.removeEventListener('click', pageClickEvent);
    };
  }, [isActive]);

  const dropDownSelectionHandler = (group) => {
    setDropDownSelection({
      groupId: group.groupId,
      groupName: group.groupName,
    });
    setCurrentGroup(group);
    setIsActive(!isActive);
  };

  return (
    <div
      ref={dropdownReference}
      className="dropdown-container"
    >
      <CustomButton
        clickHandler={() => setIsActive(!isActive)}
        size="large"
      >
        {dropDownSelection.groupName ? dropDownSelection.groupName : 'Select Group!'}
      </CustomButton>
      <ul className={isActive ? 'dropdown-list active' : 'dropdown-list inactive'}>
        <li
          className="dropdown-list-item"
        >
          <span
            role="button"
            onClick={() => dropDownSelectionHandler({
              groupId: 'all',
              groupName: 'My Albums',
            })}
            onKeyDown={() => true}
            tabIndex="0"
          >
            My Albums!
          </span>
        </li>
        {dropDownListArray.map((group) => (
          <li
            className="dropdown-list-item"
            key={group.groupId}
          >
            <span
              role="button"
              onClick={() => dropDownSelectionHandler(group)}
              onKeyDown={() => true}
              tabIndex="0"
            >
              {group.groupName}
            </span>
          </li>
        ))}
      </ul>
      {isActive}
    </div>
  );
};

export default DropDown;
