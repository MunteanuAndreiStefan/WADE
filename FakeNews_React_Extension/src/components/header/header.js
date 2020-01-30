import React from 'react';
import "./header.css";

const Header = props => {
    return (
        <div className="waves">
            <div className='wave -one'></div>
            <div className='wave -two'></div>
            <div className='wave -three'></div>
            <div className='title'>{props.name}</div>
        </div>
    );
};

Header.propTypes = {

};

export default Header;
