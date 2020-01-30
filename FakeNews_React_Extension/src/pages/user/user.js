import React from 'react';

import { goBack } from 'react-chrome-extension-router';
import Header from "../../components/header/header";
import UserAvatar from "../../components/user_avatar/user_avatar";

const User = () => {
    return (
        <div>
            <Header name="User"/>
            <UserAvatar/>
        </div>
    );
};

export default User;
