import { Button, IconButton } from '@mui/material';
import AddIcon from '@mui/icons-material/Add';
import EditIcon from '@mui/icons-material/Edit';
import ClearIcon from '@mui/icons-material/Clear';
import { useEffect, useState } from 'react';
import RelationshipEdit from './RelationshipEdit/RelationshipEdit';
import Centerer from '../../../../MiniTools/Centerer/Centerer';
import { overTheme } from '../../../../overTheme';
import './RelationshipsEdit.css';
import Relationship from './Relationship/Relationship';
import { currentDate } from '../../../../AppConsts';


export default function RelationshipsEdit (props) {

    
    const [currentRelationshipPanel, setCurrentRelationshipPanel] = useState(false);
    const [selectedRelationship, setSelectedRelationship] = useState(-1);
    const [editingRelationship, setEditingRelationship] = useState(undefined);
    

    const addRelationship = (newRelationship) => {
        var newRelationshipId = 0;
        props.project.relationships.forEach((relationship)=>{
            if (relationship.id>newRelationshipId) newRelationshipId=relationship.id; 
        });
        var newRelationshipWithRef = {...newRelationship,
            id: newRelationshipId+1
        }
        const creationHistoryEntry = {
            elementType: 4,
            changeType: 1,
            changeDate: currentDate(),
            changes: JSON.stringify(newRelationshipWithRef)
        }
        props.setProject({...props.project, 
            relationships: [...props.project.relationships, newRelationshipWithRef], 
            historyEntries: [...props.project.historyEntries, creationHistoryEntry]
        });
        props.setProjectModified(true);
    }

    const deleteRelationship = () =>{
        var relationships = [...props.project.relationships];
        const removedRelationship = relationships.splice(selectedRelationship,1)[0];
        const deletionHistoryEntry = {
            elementType: 4,
            elementId: removedRelationship.id,
            changeType: 3,
            changeDate: currentDate(),
            changes: JSON.stringify(removedRelationship)
        }
        props.setProject({...props.project, 
            relationships: relationships,
            historyEntries: [...props.project.historyEntries, deletionHistoryEntry]
        });
        setSelectedRelationship(-1);
        props.setProjectModified(true);
    }

    const startEditingRelationship = () =>{
        setEditingRelationship(props.project.relationships[selectedRelationship]);
        setCurrentRelationshipPanel(true);
    }

    const editRelationship = (editedRelationship, index, editionHistoryEntry) =>{
        var newRelationships = [...props.project.relationships];
        newRelationships[index] = editedRelationship;
        props.setProject({...props.project, 
            relationships: newRelationships,
            historyEntries: [...props.project.historyEntries, editionHistoryEntry]
        });
        props.setProjectModified(true);
    }

    const cancelRelationshipAdd = () =>{
        setCurrentRelationshipPanel(false);
    }

    const cancelRelationshipEdit = () =>{
        setCurrentRelationshipPanel(false);
        setEditingRelationship(undefined);
    }

    return (
        <div className='relationshipEditorContainer'>
            <div className={currentRelationshipPanel ? 'animHeight pePanelClosed' : 'animHeight relationshipAddContainer'}>
                <Button onClick={()=>{ setCurrentRelationshipPanel(true) }}
                style={currentRelationshipPanel ? 
                    {...props.activeButtonStyle, width: '100%', display: 'none'} : {...props.inactiveButtonStyle, width: '100%'}}
                >
                    <AddIcon className={ currentRelationshipPanel ? 'dpa_open' : 'dpa_closed'}
                        style={{transition: 'transform 0.5s'}}/>
                    ADD RELATIONSHIP
                </Button>
            </div>
            <div className={currentRelationshipPanel ? 'relationshipEditContainer animHeight crcOpen' : 'relationshipEditContainer animHeight pePanelClosed'}
                style={{backgroundColor: overTheme.palette.primary.dark}}>
                <RelationshipEdit 
                    otherRelationships={editingRelationship ? props.project.relationships.filter(relationship => relationship != editingRelationship) : props.project.relationships}
                    cancelRelationshipEdition = { editingRelationship ? cancelRelationshipEdit : cancelRelationshipAdd}
                    validateRelationshipEdition = { editingRelationship ? editRelationship : addRelationship }
                    relationshipToEdit={editingRelationship}
                    relationshipToEditIndex={selectedRelationship}
                    avaliableRelDefs={props.project.relationshipDefs}
                    avaliableArtefacts={props.project.artefacts}
                />
            </div>
            <div className={currentRelationshipPanel ? 'animHeight pePanelClosed' : 'animHeight rlcUp'}>
                <div className='relationshipListTitleContainer'>
                    <IconButton disabled={selectedRelationship==-1} onClick={startEditingRelationship}>
                        <EditIcon style={ selectedRelationship==-1 ? {color: 'gray'} : {color: 'white'}}/>
                    </IconButton>
                    <Centerer>
                        <div className='relationshipListTitle'>
                            Relationships
                        </div>
                    </Centerer>
                    <IconButton disabled={selectedRelationship==-1} onClick={deleteRelationship}>
                        <ClearIcon style={ selectedRelationship==-1 ? {color: 'gray'} : {color: 'white'}}/>
                    </IconButton>
                </div>
                <div className='relationshipList' style={{backgroundColor: overTheme.palette.primary.dark}}>
                    {props.project.relationships.map((relationship, index) =>{
                        return  <Relationship 
                                    key={index} 
                                    relation={relationship} 
                                    selected={selectedRelationship == index} 
                                    ind={index}
                                    select={setSelectedRelationship}
                                />
                    })}
                </div>
            </div>
        </div>
    );
    
}