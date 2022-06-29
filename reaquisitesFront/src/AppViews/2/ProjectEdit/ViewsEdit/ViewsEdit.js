import { Button, IconButton } from '@mui/material';
import AddIcon from '@mui/icons-material/Add';
import EditIcon from '@mui/icons-material/Edit';
import ClearIcon from '@mui/icons-material/Clear';
import VisibilityIcon from '@mui/icons-material/Visibility';
import VisibilityOffIcon from '@mui/icons-material/VisibilityOff';
import { useEffect, useState } from 'react';
import ViewEdit from './ViewEdit/ViewEdit';
import Centerer from '../../../../MiniTools/Centerer/Centerer';
import { overTheme } from '../../../../overTheme';
import './ViewsEdit.css';
import View from './View/View';
import { currentDate } from '../../../../AppConsts';


export default function ViewsEdit (props) {

    
    const [currentViewPanel, setCurrentViewPanel] = useState(false);
    const [selectedView, setSelectedView] = useState(-1);
    const [editingView, setEditingView] = useState(undefined);
    

    const addView = (newView) => {
        var newViewId = 0;
        props.project.visualizations.forEach((visualization)=>{
            if (visualization.id>newViewId) newViewId=visualization.id; 
        });
        var newRelationshipWithRef = {...newView,
            id: newViewId+1
        }
        const creationHistoryEntry = {
            elementType: 4,
            changeType: 1,
            changeDate: currentDate(),
            changes: JSON.stringify(newRelationshipWithRef)
        }
        props.setProject({...props.project, 
            visualizations: [...props.project.visualizations, newRelationshipWithRef], 
            historyEntries: [...props.project.historyEntries, creationHistoryEntry]
        });
        props.setProjectModified(true);
    }

    const deleteView = () =>{
        var visualizations = [...props.project.visualizations];
        const removedRelationship = visualizations.splice(selectedView,1)[0];
        const deletionHistoryEntry = {
            elementType: 4,
            elementId: removedRelationship.id,
            changeType: 3,
            changeDate: currentDate(),
            changes: JSON.stringify(removedRelationship)
        }
        props.setProject({...props.project, 
            visualizations: visualizations,
            historyEntries: [...props.project.historyEntries, deletionHistoryEntry]
        });
        setSelectedView(-1);
        props.setProjectModified(true);
    }

    const startEditingView = () =>{
        setEditingView(props.project.visualizations[selectedView]);
        setCurrentViewPanel(true);
    }

    const editVisualization = (editedVisualization, index, editionHistoryEntry) =>{
        var newVisualizations = [...props.project.visualizations];
        newVisualizations[index] = editedVisualization;
        props.setProject({...props.project, 
            visualizations: newVisualizations,
            historyEntries: [...props.project.historyEntries, editionHistoryEntry]
        });
        props.setProjectModified(true);
    }

    const cancelViewAdd = () =>{
        setCurrentViewPanel(false);
    }

    const cancelViewEdit = () =>{
        setCurrentViewPanel(false);
        setEditingView(undefined);
    }


    return (
        <div className='viewEditorContainer'>
            <div className={currentViewPanel ? 'animHeight pePanelClosed' : 'animHeight viewAddContainer'}>
                <Button onClick={()=>{ setCurrentViewPanel(true) }}
                style={currentViewPanel ? 
                    {...props.activeButtonStyle, width: '100%', display: 'none'} : {...props.inactiveButtonStyle, width: '100%'}}
                >
                    <AddIcon className={ currentViewPanel ? 'dpa_open' : 'dpa_closed'}
                        style={{transition: 'transform 0.5s'}}/>
                    ADD VISUALIZATION
                </Button>
            </div>
            <div className={currentViewPanel ? 'viewEditContainer animHeight cvcOpen' : 'viewEditContainer animHeight pePanelClosed'}
                style={{backgroundColor: overTheme.palette.primary.dark}}>
                <ViewEdit 
                    otherVisualizations={editingView ? props.project.visualizations.filter(visualization => visualization.name != editingView.name) : props.project.visualizations}
                    cancelVisualizationEdition = { editingView ? cancelViewEdit : cancelViewAdd}
                    validateVisualizationEdition = { editingView ? editVisualization : addView }
                    visualizationToEdit={editingView}
                    visualizationToEditIndex={selectedView}
                    avaliableRelDefs={props.project.relationshipDefs}
                    avaliableArtDefs={props.project.artefactDefs}
                />
            </div>
            <div className={currentViewPanel ? 'animHeight pePanelClosed' : 'animHeight vlcUp'}>
                <div className='viewListTitleContainer'>
                    <IconButton disabled={selectedView==-1} onClick={startEditingView}>
                        <EditIcon style={ selectedView==-1 ? {color: 'gray'} : {color: 'white'}}/>
                    </IconButton>
                    <IconButton disabled={selectedView==-1} onClick={()=>{props.setActiveVisualization(selectedView)}}>
                        <VisibilityIcon style={ selectedView==-1 ? {color: 'gray'} : {color: 'white'}}/>
                    </IconButton>
                    <Centerer>
                        <div className='viewListTitle'>
                            Visualizations
                        </div>
                    </Centerer>
                    <IconButton disabled={props.activeVisualization==-1} onClick={()=>{props.setActiveVisualization(-1)}}>
                        <VisibilityOffIcon style={ props.activeVisualization==-1 ? {color: 'gray'} : {color: 'white'}}/>
                    </IconButton>
                    <IconButton disabled={selectedView==-1} onClick={deleteView}>
                        <ClearIcon style={ selectedView==-1 ? {color: 'gray'} : {color: 'white'}}/>
                    </IconButton>
                </div>
                <div className='viewListHeadersContainer'>
                    <Centerer>
                        <div className='viewListHeader'>
                            Name
                        </div>
                    </Centerer>
                    <Centerer>
                        <div className='viewListHeader'>
                            Description
                        </div>
                    </Centerer>
                </div>
                <div className='viewList' style={{backgroundColor: overTheme.palette.primary.dark}}>
                    {props.project.visualizations.map((visualization, index) =>{
                        return  <View 
                                    key={index} 
                                    visualization={visualization} 
                                    selected={selectedView == index} 
                                    ind={index}
                                    select={setSelectedView}
                                />
                    })}
                </div>
            </div>
        </div>
    );
    
}