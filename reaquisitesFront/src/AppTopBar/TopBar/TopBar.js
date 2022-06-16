import './TopBar.css';
import React from 'react';
import Centerer from '../../MiniTools/Centerer/Centerer';
import ReaqLogo from '../../Elements/ReaqLogo/ReaqLogo';
import { Button } from '@mui/material';



export default function TopBar(props){

  return(
    <div className='topbar'>

      <Button 
      variant='contained' 
      style={{position: "absolute", left: 5, top: 5, bottom: 5, width: "70px", fontSize: "8px", fontWeight: "bold"}}
      color="secondary"
      disableElevation
      >
        {props.project ? 'OPEN' : 'EXPLORE'}
      </Button>

      { props.project &&
        <Button 
        variant={props.projectEdited ? 'contained' : 'outlined'}
        style={{position: "absolute", left: 80, top: 5, bottom: 5, width: "70px", fontSize: "8px", fontWeight: "bold"}}
        color={props.projectEdited ? 'secondary' : 'error'}
        disableElevation
        >
          SAVE
        </Button>
      }

      <Centerer>
        <ReaqLogo style={{height: "30px"}}/>
      </Centerer>

      { !props.user ?
        <>
          <Button 
            variant='contained' 
            style={{position: "absolute", right: 80, top: 5, bottom: 5, width: "70px", fontSize: "8px", fontWeight: "bold"}}
            color="secondary"
            disableElevation
            onClick={props.tooglePanel(2)}
            >
            SIGN IN
          </Button>
          <Button 
            variant='contained' 
            style={{position: "absolute", right: 5, top: 5, bottom: 5, width: "70px", fontSize: "8px", fontWeight: "bold"}}
            color="secondary"
            disableElevation
            onClick={props.tooglePanel(1)}
            >
            SIGN UP
          </Button>
        </>
        :
        <>
          <Button 
            variant='contained' 
            style={{position: "absolute", right: 5, top: 5, bottom: 5, width: "70px", fontSize: "10px", fontWeight: "bold"}}
            color="secondary"
            disableElevation
            onClick={props.tooglePanel(3)}
            >
            USER
          </Button>
          <div style={{position: "absolute", right: 80, top: 5, bottom: 5, width: "250px", textAlign: "right"}}>
            <span style={{display: "inline-block", verticalAlign: "middle"}}>
              Welcome {props.user.nick}!
            </span>
          </div>
          <Button 
            variant='contained' 
            style={{position: "absolute", right: 5, top: 5, bottom: 5, width: "70px", fontSize: "10px", fontWeight: "bold"}}
            color="secondary"
            disableElevation
            onClick={props.tooglePanel(3)}
            >
            USER
          </Button>
        </>
      }
      
    </div>
  );
};
