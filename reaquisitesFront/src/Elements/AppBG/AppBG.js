import React from "react";
import './AppBG.css';
import Centerer from '../../MiniTools/Centerer/Centerer';


export default function AppBG(props){
    return (
        <div className="reaqBackgroundContainer" style={{height: 'calc(100% - '+props.topBarHeight+'px)'}}>
            <Centerer>
                <div className="reaqBGGradient">
                    {props.children}
                </div>
            </Centerer>
        </div>
    );
}