import { useEffect, useState } from 'react';
import { overTheme } from '../../../../../overTheme';
import './ViewEdit.css';
import { Button, TextField, IconButton, Select, MenuItem } from '@mui/material';
import { currentDate } from '../../../../../AppConsts';
import AddIcon from '@mui/icons-material/Add';
import ClearIcon from '@mui/icons-material/Clear';
import FactorEdit from '../FactorEdit/FactorEdit';
import Factor from '../Factor/Factor';



export default function ViewEdit (props) {

    const [currentView, setCurrentView] = useState({
        id: 0,
        name: '',
        description: '',
        graphTemplate: 0,
        artefactColorFactors: [],
        relationshipColorFactors: [],
        artefactSizeFactors: [],
        relationshipSizeFactors: []
    });


    const [currentViewError, setCurrentViewError] = useState('Visualization name cannot be empty');
    const [selectedFactor, setSelectedFactor] = useState(-1);
    const [creatingFactor, setCreatingFactor] = useState(false);



    useEffect(() =>{
        if (props.visualizationToEdit){
            setCurrentView(props.visualizationToEdit);
            setCurrentViewError('');
        }
    },[props.visualizationToEdit]);

    

    const restartInfo = () =>{
        setCurrentView({
            id: 0,
            name: '',
            description: '',
            graphTemplate: 0,
            artefactColorFactors: [],
            relationshipColorFactors: [],
            artefactSizeFactors: [],
            relationshipSizeFactors: []
        });
        setCurrentViewError('Visualization name cannot be empty');
    }
    

    const setVisualizationInfo = (info, value) =>{
        switch (info){
            case 'name':
                if (value==''){
                    setCurrentViewError('Visualization name cannot be empty');
                }else if (props.otherVisualizations.find(visualization => visualization.name == value)){
                    setCurrentViewError('Visualization already exists');
                }else {
                    setCurrentViewError('');
                }
                setCurrentView({...currentView, name: value});
                break;
            case 'description':
                setCurrentView({...currentView, description: value});
                break;
            case 'template':
                setCurrentView({...currentView, template: value});
                break;
                
        }
        
    }

    const cancelVisualizationEdit = () =>{
        restartInfo();
        props.cancelVisualizationEdition();
    }

    const validateVisualizationEdit = () =>{
        if (props.visualizationToEdit){
            const editionHistoryEntry = {
                elementType: 5,
                elementId: currentView.id,
                changeType: 2,
                changeDate: currentDate(),
                changes: JSON.stringify({
                    old: props.relationshipToEdit,
                    new: currentView
                })
            }
            props.validateVisualizationEdition(currentView, props.visualizationToEdit, editionHistoryEntry);
        }else{
            props.validateVisualizationEdition(currentView);
        }
        restartInfo();
        props.cancelVisualizationEdition();
    }

    const addFactor = (factor) =>{
        if (factor.element.type == 1){
            if (factor.type == 0){
                setCurrentView({...currentView,
                    relationshipColorFactors: [...currentView.relationshipColorFactors, {
                        interpolated: false,
                        element: factor.element,
                        definition: factor.attribute,
                        values: factor.values,
                        weight: 1
                    }]
                });
            }else{
                setCurrentView({...currentView,
                    relationshipSizeFactors: [...currentView.relationshipSizeFactors, {
                        interpolated: false,
                        element: factor.element,
                        definition: factor.attribute,
                        values: factor.values,
                        weight: 1
                    }]
                });

            }
        }else{
            if (factor.type == 0){
                setCurrentView({...currentView,
                    artefactColorFactors: [...currentView.artefactColorFactors, {
                        interpolated: false,
                        element: factor.element,
                        definition: factor.attribute,
                        values: factor.values,
                        weight: 1
                    }]
                });
            }else{
                setCurrentView({...currentView,
                    artefactSizeFactors: [...currentView.artefactSizeFactors, {
                        interpolated: false,
                        element: factor.element,
                        definition: factor.attribute,
                        values: factor.values,
                        weight: 1
                    }]
                });

            }
        }
    }

    const removeFactor = () =>{
        if (selectedFactor >= currentView.artefactColorFactors.length + currentView.artefactSizeFactors.length + currentView.relationshipColorFactors.length){
            var newRelSizeFactors = [...currentView.relationshipSizeFactors];
            newRelSizeFactors.splice(selectedFactor-currentView.artefactColorFactors.length + currentView.artefactSizeFactors.length + currentView.relationshipColorFactors.length,1);
            setCurrentView({...currentView, relationshipSizeFactors: newRelSizeFactors});
        }else if (selectedFactor >= currentView.artefactColorFactors.length + currentView.artefactSizeFactors.length){
            var newRelSizeFactors = [...currentView.relationshipColorFactors];
            newRelSizeFactors.splice(selectedFactor-currentView.artefactColorFactors.length + currentView.artefactSizeFactors.length,1);
            setCurrentView({...currentView, relationshipColorFactors: newRelSizeFactors});
        }else if (selectedFactor >= currentView.artefactColorFactors.length ){
            var newArtColorFactors = [...currentView.artefactSizeFactors];
            newArtColorFactors.splice(selectedFactor-currentView.artefactColorFactors.length,1);
            setCurrentView({...currentView, artefactSizeFactors: newArtColorFactors});
        }else{
            var newArtColorFactors = [...currentView.artefactColorFactors];
            newArtColorFactors.splice(selectedFactor,1);
            setCurrentView({...currentView, artefactColorFactors: newArtColorFactors});
        }
        setSelectedFactor(-1);
    }

    return (
        <div className='currentVisualizationContainer' style={{backgroundColor: overTheme.palette.primary.dark}}>
            <div className='currentVisualizationNameContainer'>
                <div className='currentVisualizationTitle'>
                    Name
                </div>
                <div className='currentVisualizationValue'>
                    <TextField 
                        variant="outlined"
                        value={currentView.name}
                        error={currentViewError != ''}
                        onChange={(event) => setVisualizationInfo('name', event.target.value)}
                    />
                </div>
            </div>
            <div className='currentVisualizationTemplateContainer'>
                <div className='currentVisualizationTitle'>
                    Layout
                </div>
                <div className='currentVisualizationValue'>
                    <Select 
                        variant="outlined"
                        value={currentView.graphTemplate}
                        onChange={(event) => setVisualizationInfo('template', event.target.value)}
                    >
                        <MenuItem value={0}>
                            breadthfirst
                        </MenuItem>
                        <MenuItem value={1}>
                            dagre
                        </MenuItem>
                        <MenuItem value={2}>
                            concentric
                        </MenuItem>
                    </Select>
                </div>
            </div>
            <div className='currentVisualizationDescContainer'>
                <div className='currentVisualizationTitle'>
                    Description
                </div>
                <div className='currentVisualizationValue'>
                    <TextField 
                        variant="outlined"
                        value={currentView.description}
                        onChange={(event) => setVisualizationInfo('description', event.target.value)}
                    />
                </div>
            </div>
            <div className='currentVisualizationAttrListContainer'>
                <div className='currentVisualizationAttrListTitle'>
                    Factors
                </div>
                <div className='currentVisualizationAttrDel'>
                    <IconButton disabled={selectedFactor==-1} onClick={()=> removeFactor()}>
                        <ClearIcon style={selectedFactor==-1 ? {color: 'grey'} : {color: overTheme.palette.primary.light}}/>
                    </IconButton>
                </div>
                <div className='currenVisualizationAttrAdd'>
                    <IconButton disabled={creatingFactor} onClick={()=> setCreatingFactor(true)}>
                        <AddIcon style={creatingFactor ? {color: 'white'} : {color: overTheme.palette.primary.light}}/>
                    </IconButton>
                </div>
                <div className='currentVisualizationAttrList'>
                    {creatingFactor ?
                    <FactorEdit
                    cancelFactorEdition={() => setCreatingFactor(false)}
                    avaliableArtDefs={props.avaliableArtDefs}
                    avaliableRelDefs={props.avaliableRelDefs}
                    artefactFactors={[...currentView.artefactColorFactors, ...currentView.artefactSizeFactors]}
                    relationshipFactors = {[...currentView.relationshipColorFactors, ...currentView.relationshipSizeFactors]}
                    validateFactorEdition={addFactor}
                    />
                    : 
                    <>
                        {currentView.artefactColorFactors.map((factor,index) =>{
                            return  <Factor key={index}
                                    ind={index} 
                                    factor={factor}
                                    type={0}
                                    select={setSelectedFactor}
                                    selected={selectedFactor == index}
                                    />
                        })}
                        {currentView.artefactSizeFactors.map((factor,index) =>{
                            return  <Factor key={index + currentView.artefactColorFactors.length}
                                    ind={index + currentView.artefactColorFactors.length} 
                                    factor={factor}
                                    type={1}
                                    select={setSelectedFactor}
                                    selected={selectedFactor == index + currentView.artefactColorFactors.length}
                                    />
                        })}
                        {currentView.relationshipColorFactors.map((factor,index) =>{
                            return  <Factor key={index + currentView.artefactColorFactors.length + currentView.artefactSizeFactors.length}
                                    ind={index + currentView.artefactColorFactors.length + currentView.artefactSizeFactors.length} 
                                    factor={factor}
                                    type={0}
                                    select={setSelectedFactor}
                                    selected={selectedFactor == index + currentView.artefactColorFactors.length + currentView.artefactSizeFactors.length}
                                    />
                        })}
                        {currentView.relationshipSizeFactors.map((factor,index) =>{
                            return  <Factor key={index 
                                        + currentView.artefactColorFactors.length + currentView.artefactSizeFactors.length + currentView.relationshipColorFactors.length}
                                    ind={index 
                                        + currentView.artefactColorFactors.length + currentView.artefactSizeFactors.length + currentView.relationshipColorFactors.length} 
                                    factor={factor}
                                    type={1}
                                    select={setSelectedFactor}
                                    selected={selectedFactor == index  
                                        + currentView.artefactColorFactors.length + currentView.artefactSizeFactors.length + currentView.relationshipColorFactors.length}
                                    />
                        })}
                    </>}
                </div>
            </div>
            <div className='currentVisualizationCancelContainer'>
                <Button
                color='secondary'
                variant='contained'
                disableElevation={true}
                onClick={cancelVisualizationEdit}
                >
                    CANCEL
                </Button>
            </div>
            <div className='currentVisualizationCreateContainer'>
                <Button
                color={currentViewError=='' ? 'secondary' : 'error'} 
                variant={currentViewError=='' ? 'contained' : 'outlined'}
                onClick={currentViewError=='' ? validateVisualizationEdit : null}
                >
                    {currentViewError != '' ? currentViewError :
                     props.visualizationToEdit ? 'UPDATE' : 'CREATE' }
                </Button>
            </div>
        </div>
    );
    
}