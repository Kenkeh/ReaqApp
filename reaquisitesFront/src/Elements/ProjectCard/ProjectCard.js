import React from "react";
import './ProjectCard.css';
import { IconButton } from '@mui/material';
import { Visibility, Edit } from '@mui/icons-material';
import Centerer from '../../MiniTools/Centerer/Centerer';


export default function ProjectCard(props){
    return (
        <div className="projectCardContainer" style={props.style}>
            <div className={"projectCardTitle " + (props.textSize == 0 ? "titleSmall" : "titleBig")}>
                {props.project.name}
            </div>
            <div className={"projectCardVersion " + (props.textSize == 0 ? "textSmall" : "titleBig")}>
                {"Version: "+props.project.version}
            </div>
            <div className={"projectCardPublished " + (props.textSize == 0 ? "textSmall" : "titleBig")}>
                {props.project.isPublished ? "Público" : "Privado"}
            </div>
            <div className={"projectCardDescription " + (props.textSize == 0 ? "descSmall" : "titleBig")}>
                {props.project.description}
            </div>
            <div className={"projectCardLastModified " + (props.textSize == 0 ? "dateSmall" : "titleBig")}>
                {props.project.lastModified}
            </div>
            <div className="projectCardEditButton">
                <Centerer>
                    <IconButton sx={{color: 'white'}} >
                        <Edit/>
                    </IconButton>
                </Centerer>
            </div>
            <div className="projectCardVisualizeButton">
                <Centerer>
                    <IconButton sx={{color: 'white'}}>
                        <Visibility/>
                    </IconButton>
                </Centerer>
            </div>
        </div>
    );
}