import './Darkener.css';
import React from 'react';



export default function Darkener(props) {
  return <div className={(props.dark ? "darkener dark" : "darkener translucent")+props.hidden} 
  onClick={props.onClick(0)}></div>
};
