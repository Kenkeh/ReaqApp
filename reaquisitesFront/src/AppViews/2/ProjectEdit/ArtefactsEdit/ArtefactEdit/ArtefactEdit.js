import { useEffect, useState } from 'react';
import { overTheme } from '../../../../../overTheme';
import './ArtefactEdit.css';
import { Button, TextField, Select, MenuItem } from '@mui/material';
import { ArtefactIcons } from '../../../../../AppConsts';
import Attribute from '../../Attribute/Attribute';
import { currentDate } from '../../../../../AppConsts';


export default function ArtefactEdit (props) {

    const [currentArtefact, setCurrentArtefact] = useState({
        id: 0,
        definition: undefined,
        name: '',
        description: '',
        attributes: []
    });


    const [currentArtefactError, setCurrentArtefactError] = useState('Artefact name cannot be empty');
    



    useEffect(() =>{
        if (props.artefactToEdit){
            setCurrentArtefact(props.artefactToEdit);
            setCurrentArtefactError('');
        }
    },[props.artefactToEdit]);

    

    const restartInfo = () =>{
        setCurrentArtefact({
            id: 0,
            definition: undefined,
            name: '',
            description: '',
            attributes: []
        });
        setCurrentArtefactError('Artefact name cannot be empty');
    }
    
    const defaultAttributeValue = (attribDef) =>{
        switch (attribDef.type){
            case 2:
                return '';
            default:
                return ''+JSON.parse(attribDef.values)[0];
            
        }
    }
    const setAttributeValue = (index, value) =>{
        var newAttributes = [...currentArtefact.attributes];
        newAttributes[index] = {...newAttributes[index],
            value: value
        };
        setCurrentArtefact({ ...currentArtefact,
            attributes: newAttributes
        })
    }

    const setArtefactInfo = (info, value) =>{
        switch (info){
            case 'name':
                if (value==''){
                    setCurrentArtefactError('Artefact name cannot be empty');
                }else if (!currentArtefact.definition){
                    setCurrentArtefactError('Artefact definition cannot be empty');
                }else if (props.otherArtefacts.find(artefact => artefact.name == value && 
                    (artefact.definition.name == currentArtefact.definition.name && artefact.definition.shape == currentArtefact.definition.shape))){
                    setCurrentArtefactError('Artefact already exists');
                }else {
                    setCurrentArtefactError('');
                }
                setCurrentArtefact({...currentArtefact, name: value});
                break;
            case 'definition':
                if (!value){
                    setCurrentArtefactError('Artefact definition cannot be empty');
                }else if (currentArtefact.name==''){
                    setCurrentArtefactError('Artefact name cannot be empty');
                }else if (props.otherArtefacts.find(artefact => artefact.name == currentArtefact.name && artefact.definition.id == value.id)){
                    setCurrentArtefactError('Artefact already exists');
                }else {
                    setCurrentArtefactError('');
                }
                if (currentArtefact.definition){
                    if (value.id != currentArtefact.definition.id){
                        var newArtefactDefaultAttributes = [];
                        value.attributeDefinitions.forEach((attribDef) => {
                            newArtefactDefaultAttributes.push({
                                definition: attribDef,
                                value: defaultAttributeValue(attribDef)
                            });
                        });
                        setCurrentArtefact({...currentArtefact, definition: value, attributes: newArtefactDefaultAttributes});
                    }
                }else{
                    var newArtefactDefaultAttributes = [];
                    value.attributeDefinitions.forEach((attribDef) => {
                        newArtefactDefaultAttributes.push({
                            definition: attribDef,
                            value: defaultAttributeValue(attribDef)
                        });
                    });
                    setCurrentArtefact({...currentArtefact, definition: value, attributes: newArtefactDefaultAttributes});
                }
                break;
            case 'description':
                setCurrentArtefact({...currentArtefact, description: value});
                break;
            case 'attributeValue':
                setCurrentArtefact({...currentArtefact, description: value});
                break;
        }
        
    }

    const cancelArtefactEdit = () =>{
        restartInfo();
        props.cancelArtefactEdition();
    }

    const validateArtefactEdit = () =>{
        if (props.artefactToEdit){
            const editionHistoryEntry = {
                elementType: 3,
                elementId: currentArtefact.id,
                changeType: 2,
                changeDate: currentDate(),
                changes: JSON.stringify({
                    old: props.artefactToEdit,
                    new: currentArtefact
                })
            }
            props.validateArtefactEdition(currentArtefact, props.artefactToEditIndex, editionHistoryEntry);
        }else{
            props.validateArtefactEdition(currentArtefact);
        }
        restartInfo();
        props.cancelArtefactEdition();
    }

    return (
        <div className='currentArtefactContainer' style={{backgroundColor: overTheme.palette.primary.dark}}>
            <div className='currentArtefactIconContainer'>
                <div className='currentArtefactTitle'>
                    Definition
                </div>
                <div className='currentArtefactValue'>
                    <Select 
                        value={currentArtefact.definition || {}/*blank value, to avoid error of changing from undefined to not null*/}
                        onChange={(event) => setArtefactInfo('definition', event.target.value)}
                        error={!currentArtefact.definition || currentArtefactError == 'Artefact already exists'}
                    >
                        {props.avaliableArtDefs.map((artDef, index) =>{
                            return  <MenuItem key={index} value={artDef}>
                                        <div className='currentArtefactDefinitionItem'>
                                            {ArtefactIcons[artDef.shape]}
                                            <div className='currentArtefactDefinitionItemName'>
                                                {artDef.name}
                                            </div>
                                        </div>
                                    </MenuItem>
                        })}
                    </Select>
                </div>
            </div>
            <div className='currentArtefactNameContainer'>
                <div className='currentArtefactTitle'>
                    Name
                </div>
                <div className='currentArtefactValue'>
                    <TextField 
                        variant="outlined"
                        value={currentArtefact.name}
                        onChange={(event) => setArtefactInfo('name', event.target.value)}
                        error={currentArtefactError == 'Artefact name cannot be empty' || currentArtefactError == 'Artefact already exists'}
                    />
                </div>
            </div>
            <div className='currentArtefactDescContainer'>
                <div className='currentArtefactTitle'>
                    Description
                </div>
                <div className='currentArtefactValue'>
                    <TextField 
                        variant="outlined"
                        value={currentArtefact.description}
                        onChange={(event) => setArtefactInfo('description', event.target.value)}
                    />
                </div>
            </div>
            <div className='currentArtefactAttrListContainer'>
                <div className='currentArtefactAttrListTitle'>
                    Attributes
                </div>
                <div className='currentArtefactAttrList'>
                    {currentArtefact.definition && currentArtefact.attributes.map((attribute, index) =>{
                        return  <Attribute 
                                    key={index}
                                    ind={index} 
                                    attrib={attribute}
                                    setAttribValue={setAttributeValue}
                                />
                    })}
                </div>
            </div>
            <div className='currentArtefactCancelContainer'>
                <Button
                color='secondary'
                variant='contained'
                disableElevation={true}
                onClick={cancelArtefactEdit}
                >
                    CANCEL
                </Button>
            </div>
            <div className='currentArtefactCreateContainer'>
                <Button
                color={currentArtefactError=='' ? 'secondary' : 'error'} 
                variant={currentArtefactError=='' ? 'contained' : 'outlined'}
                onClick={currentArtefactError=='' ? validateArtefactEdit : null}
                >
                    {currentArtefactError != '' ? currentArtefactError : props.artefactToEdit ? 'UPDATE' : 'CREATE'}
                </Button>
            </div>
        </div>
    );
    
}