import './AppTopBar.css';
import {useEffect, useRef, useState} from 'react';
import Register from './Register/Register';
import Login from './Login/Login';
import TopBar from './TopBar/TopBar';
import Darkener from './Darkener/Darkener';
import UserProfile from './UserProfile/UserProfile';



export default function AppTopBar (props) {

    const [showPanel, setShowPanel] = useState(0);
    const [hideDarkener, setHideDarkener] = useState(" hidden");
    const loginResetRef = useRef(null);
    const registerResetRef = useRef(null);
    const [actualTimeoutID, setActualTimeoutID] = useState(0);
    
    useEffect(() => {
        return function cleanup(){
          clearInterval(actualTimeoutID);
        }
    },[]);

    
    const logOutUser = () =>{
        props.setUser(undefined);
        tooglePanel(3).apply();
        //We have to use .apply: tooglePanel is defined as a function that returns a function 
        //(1 that takes panel as argument and other takes event for clicking)
        // for being able to use it whit click event
    }

    const tooglePanel = (panel) => (event) =>{
        var lastPanel = showPanel;
        if (showPanel===panel || panel===0){
            setShowPanel(0);
            var animationTimeout = 500;
            setActualTimeoutID(setTimeout(()=>{
                setHideDarkener(" hidden");
                switch (lastPanel){
                    case 1:
                        registerResetRef.current();
                        break;
                    case 2:
                        loginResetRef.current();
                        break;
                    default:
                        break;
                }
            },animationTimeout));
        }else{
            setHideDarkener("");
            clearTimeout(actualTimeoutID);
            setActualTimeoutID(setTimeout(()=>setShowPanel(panel),100));
        }
    }

    return (
        <div className='app_topbar'>
            <TopBar tooglePanel={tooglePanel} user={props.user}/>
            <Darkener dark={showPanel!==0} hidden={hideDarkener} onClick={tooglePanel}/>
            {/*No renderizamos condicional, sería feo esconder la pantalla de login justo después de loggear */}
            <Register resetRef={registerResetRef} className={ "center_panel " + (showPanel===1 ? "panel_below_topbar " : "center_panel_up ")}/>
            <Login resetRef={loginResetRef} className={"center_panel " + (showPanel===2 ? "panel_below_topbar " : "center_panel_up ")} setUser={props.setUser}/>
            <UserProfile user={props.user} logOutUser={logOutUser} className={ "user_panel " + (showPanel===3 ? "panel_below_topbar " : "user_panel_up ")}/>
            
        </div>
    );
}