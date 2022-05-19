import './ReaqLogo.css';
import logo from '../images/logo192.png';
import React from 'react';
import Centerer from '../../MiniTools/Centerer/Centerer';
import {Link} from 'react-router-dom';



export default function ReaqLogo(props){
  return (
    <div id={props.id} className="reaq_logo_container" style={props.style}>
      <img className="reaq_logo_img" src={logo} alt="Here it was the logo."></img>
      <Centerer style={{position: "absolute",  top: "0px", width: "100%", height: "100%" }}> <Link to="../"><b>ReactQuisites</b></Link></Centerer>
    </div>
  );
};
