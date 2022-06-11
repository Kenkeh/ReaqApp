import { Button } from '@mui/material';
import AddIcon from '@mui/icons-material/Add';
import { useEffect, useState } from 'react';
import ArtDefEdit from './ArtDefEdit/ArtDefEdit';
import Centerer from '../../../MiniTools/Centerer/Centerer';
import { overTheme } from '../../../overTheme';
import './ArtDefsEdit.css';


export default function ArtDefsEdit (props) {

    const [currentArtDefPanel, setCurrentArtDefPanel] = useState(false);
    
    const newArtDefIconGridStyle = {
        width: '400px',
        height: '400px',
        display: 'grid',
        gridTemplateColumns: '1fr 1fr 1fr 1fr 1fr 1fr 1fr 1fr 1fr 1fr',
        gridTemplateRows: '1fr 1fr 1fr 1fr 1fr 1fr 1fr 1fr 1fr 1fr'
    };
    

    return (
        <div className='artDefEditorContainer'>
            <div className={currentArtDefPanel ? 'animHeight pePanelClosed' : 'animHeight artDefAddContainer'}>
                <Button onClick={()=>{ setCurrentArtDefPanel( prev => !prev) }}
                style={currentArtDefPanel ? 
                    {...props.activeButtonStyle, width: '100%', display: 'none'} : {...props.inactiveButtonStyle, width: '100%'}}
                >
                    <AddIcon className={ currentArtDefPanel ? 'dpa_open' : 'dpa_closed'}
                        style={{transition: 'transform 0.5s'}}/>
                    ADD ARTEFACT DEFINITION
                </Button>
            </div>
            <div className={currentArtDefPanel ? 'artDefEditContainer animHeight cadcOpen' : 'artDefEditContainer animHeight pePanelClosed'}
                style={{backgroundColor: overTheme.palette.primary.dark}}>
                <ArtDefEdit 
                projectArtDefs={props.projectArtDefs}
                cancelArtDefEdition = { ()=>{ setCurrentArtDefPanel( prev => !prev) }}
                />
            </div>
            <div className={currentArtDefPanel ? 'animHeight pePanelClosed' : 'animHeight adlcUp'}>
                <div className='artDefListTitleContainer'>
                    <Centerer>
                        <div className='artDefListTitle'>
                            Artefact Definitions
                        </div>
                    </Centerer>
                </div>
                <div className='artDefList' style={{backgroundColor: overTheme.palette.primary.dark}}>
                </div>
            </div>
        </div>
    );
    
}