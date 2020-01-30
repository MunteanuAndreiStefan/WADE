import React, {useEffect, useState} from 'react';
import Header from "../../components/header/header";
import UserAvatar from "../../components/user_avatar/user_avatar";
import Chart_1 from "../../components/chart_1/chart_1";
import Chart_2 from "../../components/chart_2/chart_2";
import User from "../user/user";
import firebase from "firebase";
import {goTo} from "react-chrome-extension-router";
import Signin from "../signin/signin";
import "./home.css";

const Home = () => {

    const [signOut, setSignOut] = useState(0);

    useEffect(() => {
        if (signOut >= 1) { setSignOut(0); }
        if (signOut === 1) {
            goTo(Signin);
            setSignOut(0);
        }
    });

    return (
        <div className="home">
            <Header name="Fake News"/>
            <UserAvatar/>
            <button className="signout" onClick={() => setSignOut(signOut + 1)}>
                Sign Out
            </button>
            <Chart_1/>
        </div>
    );
};

export default Home;
