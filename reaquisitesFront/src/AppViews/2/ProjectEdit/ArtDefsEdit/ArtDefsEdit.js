import { Button, IconButton } from '@mui/material';
import AddIcon from '@mui/icons-material/Add';
import EditIcon from '@mui/icons-material/Edit';
import ClearIcon from '@mui/icons-material/Clear';
import { useEffect, useState } from 'react';
import ArtDefEdit from './ArtDefEdit/ArtDefEdit';
import Centerer from '../../../../MiniTools/Centerer/Centerer';
import { overTheme } from '../../../../overTheme';
import './ArtDefsEdit.css';
import ArtDef from './ArtDef/ArtDef';
import { currentDate } from '../../../../AppConsts';


export default function ArtDefsEdit (props) {

    
    const [currentArtDefPanel, setCurrentArtDefPanel] = useState(false);
    const [selectedArtDef, setSelectedArtDef] = useState(-1);
    const [editingArtDef, setEditingArtDef] = useState(undefined);
    

    const addArtDef = (newArtDef) => {
        var newArtDefId = 0;
        props.project.artefactDefs.forEach((artDef)=>{
            if (artDef.id>newArtDefId) newArtDefId=artDef.id; 
        });
        var newArtDefWithRef = {...newArtDef,
            id: newArtDefId+1
        }
        const creationHistoryEntry = {
            elementType: 1,
            changeType: 1,
            changeDate: currentDate(),
            changes: JSON.stringify(newArtDefWithRef)
        }
        props.setProject({...props.project, 
            artefactDefs: [...props.project.artefactDefs, newArtDefWithRef], 
            historyEntries: [...props.project.historyEntries, creationHistoryEntry]
        });
        props.setProjectModified(true);
    }

    const deleteArtDef = () =>{
        var artDefs = [...props.project.artefactDefs];
        const removedArtDef = artDefs.splice(selectedArtDef,1)[0];
        const deletionHistoryEntry = {
            elementType: 1,
            elementId: removedArtDef.id,
            changeType: 3,
            changeDate: currentDate(),
            changes: JSON.stringify(removedArtDef)
        }
        props.setProject({...props.project, 
            artefactDefs: artDefs,
            historyEntries: [...props.project.historyEntries, deletionHistoryEntry]
        });
        setSelectedArtDef(-1);
        props.setProjectModified(true);
    }

    const startEditingArtDef = () =>{
        setEditingArtDef(props.project.artefactDefs[selectedArtDef]);
        setCurrentArtDefPanel(true);
    }

    const editArtDef = (editedArtDef, index, editionHistoryEntry) =>{
        var newArtDefs = [...props.project.artefactDefs];
        newArtDefs[index] = editedArtDef;
        props.setProject({...props.project, 
            artefactDefs: newArtDefs,
            historyEntries: [...props.project.historyEntries, editionHistoryEntry]
        });
        props.setProjectModified(true);
    }

    const cancelArtDefAdd = () =>{
        setCurrentArtDefPanel(false);
    }

    const cancelArtDefEdit = () =>{
        setCurrentArtDefPanel(false);
        setEditingArtDef(undefined);
    }

    return (
        <div className='artDefEditorContainer'>
            <div className={currentArtDefPanel ? 'animHeight pePanelClosed' : 'animHeight artDefAddContainer'}>
                <Button onClick={()=>{ setCurrentArtDefPanel(true) }}
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
                otherArtDefs={editingArtDef ? props.project.artefactDefs.filter(artDef => artDef != editingArtDef) : props.project.artefactDefs}
                cancelArtDefEdition = { editingArtDef ? cancelArtDefEdit : cancelArtDefAdd}
                validateArtDefEdition = { editingArtDef ? editArtDef : addArtDef }
                artDefToEdit={editingArtDef}
                artDefToEditIndex={selectedArtDef}
                />
            </div>
            <div className={currentArtDefPanel ? 'animHeight pePanelClosed' : 'animHeight adlcUp'}>
                <div className='artDefListTitleContainer'>
                    <IconButton disabled={selectedArtDef==-1} onClick={startEditingArtDef}>
                        <EditIcon style={ selectedArtDef==-1 ? {color: 'gray'} : {color: 'white'}}/>
                    </IconButton>
                    <Centerer>
                        <div className='artDefListTitle'>
                            Artefact Definitions
                        </div>
                    </Centerer>
                    <IconButton disabled={selectedArtDef==-1} onClick={deleteArtDef}>
                        <ClearIcon style={ selectedArtDef==-1 ? {color: 'gray'} : {color: 'white'}}/>
                    </IconButton>
                </div>
                <div className='artDefListHeadersContainer'>
                    <Centerer>
                        <div className='artDefListHeader'>
                            Shape
                        </div>
                    </Centerer>
                    <Centerer>
                        <div className='artDefListHeader'>
                            Name
                        </div>
                    </Centerer>
                </div>
                <div className='artDefList' style={{backgroundColor: overTheme.palette.primary.dark}}>
                    {props.project.artefactDefs.map((artDef, index) =>{
                        return  <ArtDef 
                                key={index} 
                                artDef={artDef} 
                                selected={selectedArtDef == index} 
                                ind={index}
                                select={setSelectedArtDef}/>
                    })}
                </div>
            </div>
        </div>
    );
    
}