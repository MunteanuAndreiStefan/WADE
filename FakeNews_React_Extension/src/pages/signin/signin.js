import React from 'react';
import Header from "../../components/header/header";
import SigninButtons from "../../components/signin_buttons/signin_buttons";


const Signin = () => {
    return (
        <div>
            <Header name="Sign In"/>
            <SigninButtons/>
        </div>
    );
};

export default Signin;
