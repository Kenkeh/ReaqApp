import './UserProfile.css';
import React from 'react';
import Centerer from '../../MiniTools/Centerer/Centerer';
import ReaqLogo from '../../Elements/ReaqLogo/ReaqLogo';
import { Button } from '@mui/material'
import  {overTheme} from '../../overTheme';



export default function UserProfile(props){


  return(
    <div className={props.className}>
      <div className="user_profile_container" style={{backgroundColor: overTheme.palette.primary.light}}>
        { props.user ?
          <>
            <Centerer>
              <span><b>Account Name:</b> {props.user.account}</span>
            </Centerer>
            <Centerer>
              <span><b>Nick Name:</b> {props.user.nick}</span>
            </Centerer>
            <Centerer>
              <span><b>E-Mail:</b> {props.user.eMail}</span>
            </Centerer>
            <Centerer>
              <span><b>Register Date:</b> {(new Date(props.user.registerDate)).toLocaleString()}</span>
            </Centerer>
            <Centerer>
              <Button variant="contained" onClick={props.logOutUser}>
                LOG OUT
              </Button>
            </Centerer>
          </>
          :
          <>
          <Centerer>
            <span><b>Account Name:</b></span>
          </Centerer>
          <Centerer>
            <span><b>Nick Name:</b></span>
          </Centerer>
          <Centerer>
            <span><b>E-Mail:</b></span>
          </Centerer>
          <Centerer>
            <span><b>Register Date:</b></span>
          </Centerer>
          </>
        }
        
      </div>
    </div>
  );
};
