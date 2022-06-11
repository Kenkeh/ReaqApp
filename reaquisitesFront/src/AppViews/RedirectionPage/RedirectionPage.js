import './RedirectionPage.css';
import { useState, useEffect } from 'react';
import Centerer from '../../MiniTools/Centerer/Centerer';
import logo from '../../Elements/images/logo192.png';
import Chip from '@mui/material/Chip';
import Spinner from '../../MiniTools/Spinner/Spinner';
import { Navigate } from 'react-router-dom';
import { overTheme } from '../../overTheme';



export default function RedirectionPage(props) {

  
  var timeoutIntervalID = 0;

  const [redirectionTimeout,setRedirectionTimeout] = useState(10);

  const redirectIntervalTimer = () => {
    setRedirectionTimeout(redirectionTimeout-1);
  }
  useEffect(() => {
    timeoutIntervalID = setInterval(() => {
      setRedirectionTimeout(prevValue => prevValue-1);
    }, 1000);
    return function cleanup(){
      clearInterval(timeoutIntervalID);
    }
  },[]);
  return (
    <div className="redirection_page_container" style={{backgroundColor: overTheme.palette.primary.dark}}>
      <div className="redirection_page_img_container">
        <Spinner msCycle="10000ms" left>
          <Centerer>
            <img className="register_acc_img" src={logo} alt="Here it was the logo."></img>
          </Centerer>
        </Spinner>
      </div>
      <div className="redirection_page_message_container">
        <Centerer>
          <Chip label={props.message}/>
        </Centerer>
      </div>
      <div className="redirection_page_message_container">
        <Centerer >
          You will be redirected to the homepage in {redirectionTimeout} seconds...
        </Centerer>
        {redirectionTimeout>0 ? null : <Navigate to="/"/>}
      </div >
      <div className="redirection_page_message_container">
        <Centerer >
            <img className="redirection_page_img" src={props.icon} alt="Here it was the accept icon"></img>
        </Centerer>
      </div>
      <div className="redirection_page_img_container">
        <Spinner msCycle="10000ms">
          <Centerer>
            <img className="register_acc_img" src={logo} alt="Here it was the logo."></img>
          </Centerer>
        </Spinner>
      </div>
    </div>
    );
};
