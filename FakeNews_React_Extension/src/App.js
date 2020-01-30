import React from 'react';
import './App.css';
import Header from "./components/header/header";
import UserAvatar from "./components/user_avatar/user_avatar";
import Chart_1 from "./components/chart_1/chart_1";
import Chart_2 from "./components/chart_2/chart_2";

import {
    Router,
    Link,
    getCurrent,
    goBack,
    goTo,
    popToTop,
} from 'react-chrome-extension-router';


import Home from "./pages/home/home";
import User from "./pages/user/user";
import Signin from "./pages/signin/signin";



function App() {
  return (

    <div className="App">
        <Router>
            <Signin/>
        </Router>
    </div>

  );
}

export default App;
