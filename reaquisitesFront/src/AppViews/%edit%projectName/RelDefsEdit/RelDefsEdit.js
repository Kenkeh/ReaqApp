import { Button, IconButton } from '@mui/material';
import AddIcon from '@mui/icons-material/Add';
import EditIcon from '@mui/icons-material/Edit';
import ClearIcon from '@mui/icons-material/Clear';
import { useEffect, useState } from 'react';
import RelDefEdit from './RelDefEdit/RelDefEdit';
import Centerer from '../../../MiniTools/Centerer/Centerer';
import { overTheme } from '../../../overTheme';
import './RelDefsEdit.css';
import RelDef from './RelDef/RelDef';
import { currentDate } from '../../../AppConsts';


export default function RelDefsEdit (props) {

    const [currentRelDefPanel, setCurrentRelDefPanel] = useState(false);
    const [selectedRelDef, setSelectedRelDef] = useState(-1);
    const [editingRelDef, setEditingRelDef] = useState(undefined);
    

    const addRelDef = (newRelDef) => {
        var creationHistoryEntry = {
            type: 1,
            changeDate: currentDate(),
            changes: JSON.stringify({
                elementType: 1,
                newRelationshipDef: newRelDef
            })
        }
        props.setProject({...props.project, 
            relationshipDefs: [...props.project.relationshipDefs, newRelDef], 
            historyEntries: [...props.project.historyEntries, creationHistoryEntry]
        });
        props.setProjectModified(true);
    }

    const deleteRelDef = () =>{
        var relDefs = [...props.project.relationshipDefs];
        const removedRelDef = relDefs.splice(selectedRelDef,1);
        var deletionHistoryEntry = {
            type: 3,
            changeDate: currentDate(),
            changes: JSON.stringify({
                elementType: 1,
                deletedRelefatcDef: removedRelDef
            })
        }
        props.setProject({...props.project, 
            relationshipDefs: relDefs,
            historyEntries: [...props.project.historyEntries, deletionHistoryEntry]
        });
        setSelectedRelDef(-1);
        props.setProjectModified(true);
    }

    const startEditingRelDef = () =>{
        setEditingRelDef(props.project.relationshipDefs[selectedRelDef]);
        setCurrentRelDefPanel(true);
    }

    const editRelDef = (editedRelDef, index) =>{
        var newRelDefs = [...props.project.relationshipDefs];
        newRelDefs[index] = editedRelDef;
        props.setProject({...props.project, relationshipDefs: newRelDefs});
        props.setProjectModified(true);
    }

    const cancelRelDefAdd = () =>{
        setCurrentRelDefPanel(false);
    }

    const cancelRelDefEdit = () =>{
        setCurrentRelDefPanel(false);
        setEditingRelDef(undefined);
    }

    return (
        <div className='relDefEditorContainer'>
            <div className={currentRelDefPanel ? 'animHeight pePanelClosed' : 'animHeight relDefAddContainer'}>
                <Button onClick={()=>{ setCurrentRelDefPanel(true) }}
                style={currentRelDefPanel ? 
                    {...props.activeButtonStyle, width: '100%', display: 'none'} : {...props.inactiveButtonStyle, width: '100%'}}
                >
                    <AddIcon className={ currentRelDefPanel ? 'dpa_open' : 'dpa_closed'}
                        style={{transition: 'transform 0.5s'}}/>
                    ADD RELATIONSHIP DEFINITION
                </Button>
            </div>
            <div className={currentRelDefPanel ? 'relDefEditContainer animHeight cadcOpen' : 'relDefEditContainer animHeight pePanelClosed'}
                style={{backgroundColor: overTheme.palette.primary.dark}}>
                <RelDefEdit 
                otherRelDefs={editingRelDef ? props.project.relationshipDefs.filter(relDef => relDef != editingRelDef) : props.project.relationshipDefs}
                cancelRelDefEdition = { editingRelDef ? cancelRelDefEdit : cancelRelDefAdd}
                validateRelDefEdition = { editingRelDef ? editRelDef : addRelDef }
                relDefToEdit={editingRelDef}
                relDefToEditIndex={selectedRelDef}
                />
            </div>
            <div className={currentRelDefPanel ? 'animHeight pePanelClosed' : 'animHeight adlcUp'}>
                <div className='relDefListTitleContainer'>
                    <IconButton disabled={selectedRelDef==-1} onClick={startEditingRelDef}>
                        <EditIcon style={ selectedRelDef==-1 ? {color: 'gray'} : {color: 'white'}}/>
                    </IconButton>
                    <Centerer>
                        <div className='relDefListTitle'>
                            Relationship Definitions
                        </div>
                    </Centerer>
                    <IconButton disabled={selectedRelDef==-1} onClick={deleteRelDef}>
                        <ClearIcon style={ selectedRelDef==-1 ? {color: 'gray'} : {color: 'white'}}/>
                    </IconButton>
                </div>
                <div className='relDefList' style={{backgroundColor: overTheme.palette.primary.dark}}>
                    {props.project.relationshipDefs.map((relDef, index) =>{
                        return  <RelDef 
                                key={index} 
                                relDef={relDef} 
                                selected={selectedRelDef == index} 
                                ind={index}
                                select={setSelectedRelDef}/>
                    })}
                </div>
            </div>
        </div>
    );
    
}