import { Button, IconButton } from '@mui/material';
import AddIcon from '@mui/icons-material/Add';
import EditIcon from '@mui/icons-material/Edit';
import ClearIcon from '@mui/icons-material/Clear';
import { useEffect, useState } from 'react';
import ArtefactEdit from './ArtefactEdit/ArtefactEdit';
import Centerer from '../../../../MiniTools/Centerer/Centerer';
import { overTheme } from '../../../../overTheme';
import './ArtefactsEdit.css';
import Artefact from './Artefact/Artefact';
import { currentDate } from '../../../../AppConsts';


export default function ArtefactsEdit (props) {

    
    const [currentArtefactPanel, setCurrentArtefactPanel] = useState(false);
    const [selectedArtefact, setSelectedArtefact] = useState(-1);
    const [editingArtefact, setEditingArtefact] = useState(undefined);
    

    const addArtefact = (newArtefact) => {
        const creationHistoryEntry = {
            elementType: 3,
            changeType: 1,
            changeDate: currentDate(),
            changes: JSON.stringify(newArtefact)
        }
        props.setProject({...props.project, 
            artefacts: [...props.project.artefacts, newArtefact], 
            historyEntries: [...props.project.historyEntries, creationHistoryEntry]
        });
        props.setProjectModified(true);
    }

    const deleteArtefact = () =>{
        var artefacts = [...props.project.artefacts];
        const removedArtefact = artefacts.splice(selectedArtefact,1)[0];
        const deletionHistoryEntry = {
            elementType: 1,
            elementId: removedArtefact.id,
            changeType: 3,
            changeDate: currentDate(),
            changes: JSON.stringify(removedArtefact)
        }
        props.setProject({...props.project, 
            artefacts: artefacts,
            historyEntries: [...props.project.historyEntries, deletionHistoryEntry]
        });
        setSelectedArtefact(-1);
        props.setProjectModified(true);
    }

    const startEditingArtefact = () =>{
        setEditingArtefact(props.project.artefacts[selectedArtefact]);
        setCurrentArtefactPanel(true);
    }

    const editArtefact = (editedArtefact, index, editionHistoryEntry) =>{
        var newArtefacts = [...props.project.artefacts];
        newArtefacts[index] = editedArtefact;
        props.setProject({...props.project, 
            artefacts: newArtefacts,
            historyEntries: [...props.project.historyEntries, editionHistoryEntry]
        });
        props.setProjectModified(true);
    }

    const cancelArtefactAdd = () =>{
        setCurrentArtefactPanel(false);
    }

    const cancelArtefactEdit = () =>{
        setCurrentArtefactPanel(false);
        setEditingArtefact(undefined);
    }

    return (
        <div className='artefactEditorContainer'>
            <div className={currentArtefactPanel ? 'animHeight pePanelClosed' : 'animHeight artefactAddContainer'}>
                <Button onClick={()=>{ setCurrentArtefactPanel(true) }}
                style={currentArtefactPanel ? 
                    {...props.activeButtonStyle, width: '100%', display: 'none'} : {...props.inactiveButtonStyle, width: '100%'}}
                >
                    <AddIcon className={ currentArtefactPanel ? 'dpa_open' : 'dpa_closed'}
                        style={{transition: 'transform 0.5s'}}/>
                    ADD ARTEFACT
                </Button>
            </div>
            <div className={currentArtefactPanel ? 'artefactEditContainer animHeight cadcOpen' : 'artefactEditContainer animHeight pePanelClosed'}
                style={{backgroundColor: overTheme.palette.primary.dark}}>
                <ArtefactEdit 
                    otherArtefacts={editingArtefact ? props.project.artefactDefs.filter(artefact => artefact != editingArtefact) : props.project.artefactDefs}
                    cancelArtefactEdition = { editingArtefact ? cancelArtefactEdit : cancelArtefactAdd}
                    validateArtefactEdition = { editingArtefact ? editArtefact : addArtefact }
                    artefactToEdit={editingArtefact}
                    artefactToEditIndex={selectedArtefact}
                    avaliable
                />
            </div>
            <div className={currentArtefactPanel ? 'animHeight pePanelClosed' : 'animHeight adlcUp'}>
                <div className='artefactListTitleContainer'>
                    <IconButton disabled={selectedArtefact==-1} onClick={startEditingArtefact}>
                        <EditIcon style={ selectedArtefact==-1 ? {color: 'gray'} : {color: 'white'}}/>
                    </IconButton>
                    <Centerer>
                        <div className='artefactListTitle'>
                            Artefacts
                        </div>
                    </Centerer>
                    <IconButton disabled={selectedArtefact==-1} onClick={deleteArtefact}>
                        <ClearIcon style={ selectedArtefact==-1 ? {color: 'gray'} : {color: 'white'}}/>
                    </IconButton>
                </div>
                <div className='artefactList' style={{backgroundColor: overTheme.palette.primary.dark}}>
                    {props.project.artefacts.map((artefact, index) =>{
                        return  <Artefact 
                                    key={index} 
                                    artefact={artefact} 
                                    selected={selectedArtefact == index} 
                                    ind={index}
                                    select={setSelectedArtefact}
                                />
                    })}
                </div>
            </div>
        </div>
    );
    
}