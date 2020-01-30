import React from 'react';
import {StyledFirebaseAuth} from "react-firebaseui";
import firebase from "firebase";

import "./signin_buttons.css";
import Home from "../../pages/home/home";

import { goTo } from 'react-chrome-extension-router';

// Configure Firebase.
const config = {
    apiKey: "AIzaSyAshfSe2tk-ym25NKqHedc5M5X_X19j-GM",
    authDomain: "aset2019-f3e29.firebaseapp.com",
    databaseURL: "https://aset2019-f3e29.firebaseio.com",
    projectId: "aset2019-f3e29",
    storageBucket: "aset2019-f3e29.appspot.com",
    messagingSenderId: "24325557928",
    appId: "1:24325557928:web:77b28a455d06a9244ffc9d",
    measurementId: "G-30K23FMDBT"
};
firebase.initializeApp(config);

const uiConfig = {
    signInFlow: 'redirect',
    callbacks: {
        signInSuccessWithAuthResult: (authResult, redirectUrl) => {
            goTo(Home);
            return true
        }
    },
    signInOptions: [
        firebase.auth.EmailAuthProvider.PROVIDER_ID,
        firebase.auth.FacebookAuthProvider.PROVIDER_ID,
        firebase.auth.TwitterAuthProvider.PROVIDER_ID,
        firebase.auth.GithubAuthProvider.PROVIDER_ID
    ]
};

const SigninButtons = () => {
    return (
        <div>
            <div className="signincontent">
                <StyledFirebaseAuth uiConfig={uiConfig} firebaseAuth={firebase.auth()} />
            </div>
        </div>
    );
};

export default SigninButtons;
