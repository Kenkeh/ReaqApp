import { Routes, Route } from 'react-router-dom';
import React from 'react';
import AccIcon from '../Elements/images/green_valid.png';
import RejIcon from '../Elements/images/red_cancel.png';
import RedirectionPage from './RedirectionPage/RedirectionPage';
import LandingArea from './%/LandingArea/LandingArea';
import FirstLook from './%/FirstLook/FirstLook';



export default function MainSwitcher(props) {
  return (
    <Routes>
      {<Route path="/" element={
        props.user ? 
        <FirstLook user={props.user} loginSession={props.loginSession} setProject={props.setProject}/> :
        <LandingArea/>
      }
      />}
      {<Route path="/register-exp" element={
        <RedirectionPage 
          topBarHeight={props.topBarHeight} 
          message="Ups! It seems that link has expired..." 
          icon={RejIcon}/>
        } 
      />}
      {<Route path="/register-acc" element={
        <RedirectionPage 
          topBarHeight={props.topBarHeight} 
          message="Congratulations, your register is completed! You can now log in!"
          icon={AccIcon}
        />} 
      />}
    </Routes>
  );
};
