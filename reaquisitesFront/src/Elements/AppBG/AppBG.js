import React from "react";
import './AppBG.css';
import Centerer from '../../MiniTools/Centerer/Centerer';


export default function AppBG(props){
    return (
        <div className="reaqBackgroundContainer" style={{width: props.width, height: props.height}}>
            <Centerer>
                <div className="reaqBGGradient">
                    {props.children}
                </div>
            </Centerer>
        </div>
    );
}