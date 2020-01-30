import React from 'react';
import {goBack} from "react-chrome-extension-router";

const UserProfile = () => {
    return (
        <div className="user_profile">
            <button onClick={() => goBack()}> Back </button>
            <p> Hello User </p>
        </div>
    );
};

export default UserProfile;
