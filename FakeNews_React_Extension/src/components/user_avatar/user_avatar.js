import React from 'react';
import "./user_avatar.css";
import logo from '../../assets/personal-user-illustration-@2x.png';
import User from "../../pages/user/user";
import { Link } from 'react-chrome-extension-router';

const aop = require("node-aop");

const obj = {
    name:"",
    get : function(key){ return this["key"]; },
    set : function(key, value){ this["key"] = value; }
};

aop.before(obj, "set", function(key, value){
    value =  " Hello, " + value;
    return [key, value]; // modify the parameters,
});

obj.set("name", "test@gmail.com");

const UserAvatar = () => {
    return (
        <div>
            <h4 className="gr-title-1">USER</h4>
            <Link className="user-link" component={User}>
            <img src={logo} alt="Logo" className="user-img" height="100" width="90" />
            <div className="user-content">
                <p className="user-name">{ obj.get("name")  }</p>
            </div>
            </Link>
        </div>
    );
};

export default UserAvatar;
