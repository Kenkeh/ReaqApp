import { useEffect, useState } from 'react';
import './ProjectEdit.css';
import { getUserProject } from '../../../AppAPI';
import { overTheme } from '../../../overTheme';
import { Button } from '@mui/material';
import ArtDefsEdit from './ArtDefsEdit/ArtDefsEdit';
import RelDefsEdit from './RelDefsEdit/RelDefsEdit';


export default function ProjectEdit (props) {

    const [activeLabel, setActiveLabel] = useState(undefined);

    
    const elemsButtonsStyleHiglighted = {
        borderRadius: '5px',
        color: 'black', 
        height: '100%',
        minHeight: '0px',
        backgroundColor: overTheme.palette.primary.light
    }
    const elemsButtonsStyle = {
        borderRadius: '5px',
        color: 'white', 
        height: '100%',
        minHeight: '0px',
        backgroundColor: overTheme.palette.secondary.dark
    }


    const elementEditionPanel = (panel) =>{
        switch (panel){
            case 0:
                return  <ArtDefsEdit 
                            inactiveButtonStyle={elemsButtonsStyle} 
                            activeButtonStyle={elemsButtonsStyleHiglighted}
                            project={props.activeProject}
                            setProject={props.setActiveProject}
                            setProjectModified={props.setActiveProjectModified}
                        />
            case 1:
                return  <RelDefsEdit
                            inactiveButtonStyle={elemsButtonsStyle} 
                            activeButtonStyle={elemsButtonsStyleHiglighted}
                            project={props.activeProject}
                            setProject={props.setActiveProject}
                            setProjectModified={props.setActiveProjectModified}
                        />
            default:
                return <></>
        }
    }


    return (
        <div className='projectEditorBackgroud'>
            <div className='projectEditionBackground' style={{backgroundColor: overTheme.palette.primary.light, borderColor: overTheme.palette.primary.light}}>
                <div className='projectEditionLabels'>
                    <Button className='projectEditionLabel' 
                        onClick={() => setActiveLabel(0)}
                        style={activeLabel==0 ? elemsButtonsStyleHiglighted : elemsButtonsStyle }
                        disableElevation={true}
                        >
                            Artefact Definition
                    </Button>
                    <Button className='projectEditionLabel' 
                        onClick={() => setActiveLabel(1)}
                        style={activeLabel==1 ? elemsButtonsStyleHiglighted : elemsButtonsStyle}
                        disableElevation={true}
                        >
                            Relationship Definition
                    </Button>
                    <Button className='projectEditionLabel'
                        onClick={() => setActiveLabel(2)}
                        style={activeLabel==2 ? elemsButtonsStyleHiglighted : elemsButtonsStyle}
                        disableElevation={true}
                        >
                            Artefact
                    </Button>
                    <Button className='projectEditionLabel' 
                        onClick={() => setActiveLabel(3)}
                        style={activeLabel==3 ? elemsButtonsStyleHiglighted : elemsButtonsStyle}
                        disableElevation={true}
                        >
                            Relationship
                    </Button>
                    <Button className='projectEditionLabel' 
                        onClick={() => setActiveLabel(4)}
                        style={activeLabel==4 ? elemsButtonsStyleHiglighted : elemsButtonsStyle}
                        disableElevation={true}
                        >
                            View
                    </Button>
                </div>
                <div className='projectElementsEditionPanel'>
                    {elementEditionPanel(activeLabel)}
                </div>
            </div>
        </div>
    );
    
}