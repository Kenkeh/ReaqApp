import './TopBar.css';
import React from 'react';
import Centerer from '../../MiniTools/Centerer/Centerer';
import ReaqLogo from '../../Elements/ReaqLogo/ReaqLogo';
import { Button } from '@mui/material';
import { saveProject } from '../../AppAPI';



export default function TopBar(props){
  
  const currentTopBarContent = (view) => {
    switch (view){
      case 0:
        return  <>
                  <Button 
                    variant='contained' 
                    style={{position: "absolute", left: 5, top: 5, bottom: 5, width: "70px", fontSize: "8px", fontWeight: "bold"}}
                    color="secondary"
                    disableElevation
                  >
                    EXPLORE
                  </Button>
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
                </>;
      case 1:
        return  <>
                  <Button 
                    variant='contained' 
                    style={{position: "absolute", left: 5, top: 5, bottom: 5, width: "70px", fontSize: "8px", fontWeight: "bold"}}
                    color="secondary"
                    disableElevation
                  >
                    EXPLORE
                  </Button>
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
                </>;
      case 2:
        return  <>
                  <Button 
                    variant='contained' 
                    style={{position: "absolute", left: 5, top: 5, bottom: 5, width: "70px", fontSize: "8px", fontWeight: "bold"}}
                    color="secondary"
                    disableElevation
                  >
                    OPEN
                  </Button>
                  <Button 
                  variant={props.projectEdited ? 'contained' : 'outlined'}
                  style={{position: "absolute", left: 80, top: 5, bottom: 5, width: "70px", fontSize: "8px", fontWeight: "bold"}}
                  color={props.projectEdited ? 'secondary' : 'error'}
                  onClick={props.projectEdited ? () => props.saveProject() : undefined}
                  disableElevation
                  >
                    SAVE
                  </Button>
                  <Button 
                    variant='contained' 
                    style={{position: "absolute", left: 155, top: 5, bottom: 5, width: "140px", fontSize: "8px", fontWeight: "bold",
                      display: 'block', whiteSpace: 'nowrap', overflow: 'hidden', textOverflow: 'ellipsis'}}
                    color="secondary"
                    disableElevation
                  >
                    {props.project ? props.project.name : 'Loading...'}
                  </Button>
                  <Button 
                    variant='contained' 
                    style={{position: "absolute", right: 5, top: 5, bottom: 5, width: "70px", fontSize: "10px", fontWeight: "bold"}}
                    color="secondary"
                    disableElevation
                    onClick={props.tooglePanel(3)}
                    >
                    USER
                  </Button>
                  <Button 
                    variant='contained' 
                    style={{position: "absolute", right: 80, top: 5, bottom: 5, width: "70px", fontSize: "10px", fontWeight: "bold"}}
                    color="secondary"
                    disableElevation
                    onClick={() => props.setCurrentView(1)}
                    >
                    BACK
                  </Button>
                </>;
      default:
        return <></>;
    }
  }

  return(
    <div className='topbar'>
      {currentTopBarContent(props.currentView)}
      <Centerer>
        <ReaqLogo style={{height: "30px"}}/>
      </Centerer>
    </div>
  );
};
