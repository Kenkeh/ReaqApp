import { useEffect, useState } from 'react';
import { overTheme } from '../../../../../overTheme';
import './RelationshipEdit.css';
import { Button, TextField, Select, MenuItem } from '@mui/material';
import Attribute from '../../Attribute/Attribute';
import { ArtefactIcons, currentDate, cytoscapeArrowHeads } from '../../../../../AppConsts';
import CJSArrowShow from '../../RelDefsEdit/RelDefEdit/CJSArrowShow/CJSArrowShow';
import Centerer from '../../../../../MiniTools/Centerer/Centerer';


export default function RelationshipEdit (props) {

    const [currentRelationship, setCurrentRelationship] = useState({
        id: 0,
        definition: undefined,
        parent: undefined,
        child: undefined,
        description: '',
        attributes: []
    });


    const [currentRelationshipError, setCurrentRelationshipError] = useState('Relationship definition cannot be empty');
    



    useEffect(() =>{
        if (props.relationshipToEdit){
            setCurrentRelationship(props.relationshipToEdit);
            setCurrentRelationshipError('');
        }
    },[props.relationshipToEdit]);

    

    const restartInfo = () =>{
        setCurrentRelationship({
            id: 0,
            definition: undefined,
            parent: undefined,
            child: undefined,
            description: '',
            attributes: []
        });
        setCurrentRelationshipError('Relationship definition cannot be empty');
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
        var newAttributes = [...currentRelationship.attributes];
        newAttributes[index] = {...newAttributes[index],
            value: value
        };
        setCurrentRelationship({ ...currentRelationship,
            attributes: newAttributes
        })
    }

    const setRelationshipInfo = (info, value) =>{
        switch (info){
            case 'definition':
                if ((currentRelationship.parent && currentRelationship.child) &&
                    props.otherRelationships.find(relationship => (relationship.parent.id == currentRelationship.parent.id &&
                    relationship.child.id == currentRelationship.child.id) && relationship.definition.id == value.id)){
                    setCurrentRelationshipError('Relationship already exists');
                }else {
                    setCurrentRelationshipError('');
                }
                if (currentRelationship.definition){
                    if (value.id != currentRelationship.definition.id){
                        var newRelationshipDefaultAttributes = [];
                        value.attributeDefinitions.forEach((attribDef) => {
                            newRelationshipDefaultAttributes.push({
                                definition: attribDef,
                                value: defaultAttributeValue(attribDef)
                            });
                        });
                        setCurrentRelationship({...currentRelationship, definition: value, attributes: newRelationshipDefaultAttributes});
                    }
                }else{
                    var newRelationshipDefaultAttributes = [];
                    value.attributeDefinitions.forEach((attribDef) => {
                        newRelationshipDefaultAttributes.push({
                            definition: attribDef,
                            value: defaultAttributeValue(attribDef)
                        });
                    });
                    setCurrentRelationship({...currentRelationship, definition: value, attributes: newRelationshipDefaultAttributes});
                }
                break;
            case 'parent':
                if ((currentRelationship.definition && currentRelationship.child) &&
                    props.otherRelationships.find(relationship => (relationship.parent.id == value.id &&
                    relationship.child.id == currentRelationship.child.id) && relationship.definition.id == currentRelationship.definition.id)){
                    setCurrentRelationshipError('Relationship already exists');
                }else {
                    setCurrentRelationshipError('');
                }
                setCurrentRelationship({...currentRelationship, parent: value});
                break;
            case 'child':
                if ((currentRelationship.definition && currentRelationship.parent) &&
                    props.otherRelationships.find(relationship => (relationship.parent.id == currentRelationship.parent.id &&
                    relationship.child.id == value.id) && relationship.definition.id == currentRelationship.definition.id)){
                    setCurrentRelationshipError('Relationship already exists');
                }else {
                    setCurrentRelationshipError('');
                }
                setCurrentRelationship({...currentRelationship, child: value});
                break;
            case 'description':
                setCurrentRelationship({...currentRelationship, description: value});
                break;
        }
        
    }

    const cancelRelationshipEdit = () =>{
        restartInfo();
        props.cancelRelationshipEdition();
    }

    const validateRelationshipEdit = () =>{
        if (props.relationshipToEdit){
            const editionHistoryEntry = {
                elementType: 4,
                elementId: currentRelationship.id,
                changeType: 2,
                changeDate: currentDate(),
                changes: JSON.stringify({
                    old: props.relationshipToEdit,
                    new: currentRelationship
                })
            }
            props.validateRelationshipEdition(currentRelationship, props.relationshipToEditIndex, editionHistoryEntry);
        }else{
            props.validateRelationshipEdition(currentRelationship);
        }
        restartInfo();
        props.cancelRelationshipEdition();
    }

    return (
        <div className='currentRelationshipContainer' style={{backgroundColor: overTheme.palette.primary.dark}}>
            <div className='currentRelationshipIconContainer'>
                <div className='currentRelationshipTitle'>
                    Definition
                </div>
                <div className='currentRelationshipValue'>
                    <Select 
                        value={currentRelationship.definition || {}/*blank value, to avoid error of changing from undefined to not null*/}
                        onChange={(event) => setRelationshipInfo('definition', event.target.value)}
                        error={!currentRelationship.definition || currentRelationshipError == 'Relationship already exists'}
                    >
                        {props.avaliableRelDefs.map((relDef, index) => {
                            return  <MenuItem value={relDef} key={index}>
                                        <div className='currentRelationshipDefinitionItemContainer'>
                                            <CJSArrowShow 
                                                selected={currentRelationship.definition && currentRelationship.definition.id==relDef.id} 
                                                index={'RelDefEdit'+index} 
                                                arrowType={cytoscapeArrowHeads[Math.floor(relDef.shape/2)]} 
                                                fillType={relDef.shape%2==0 ? 'filled' : 'hollow'}
                                            />
                                            <div className='currentRelationshipDefinitionItemName'>
                                                <Centerer>
                                                    {relDef.name}
                                                </Centerer>
                                            </div>
                                        </div>
                                    </MenuItem>
                        })}
                    </Select>
                </div>
            </div>
            <div className='currentRelationshipParentContainer'>
                <div className='currentRelationshipTitle'>
                    Parent
                </div>
                <div className='currentRelationshipValue'>
                    <Select 
                        variant="outlined"
                        value={currentRelationship.parent|| {}/*blank value, to avoid error of changing from undefined to not null*/}
                        onChange={(event) => setRelationshipInfo('parent', event.target.value)}
                        error={!currentRelationship.parent || currentRelationshipError == 'Relationship already exists'}
                    >
                        {
                            !currentRelationship.child ?
                            props.avaliableArtefacts.map((artefact, index) => {
                                return  <MenuItem value={artefact} key={index}>
                                            <div className='currentRelationshipImplicateItemContainer'>
                                                {ArtefactIcons[artefact.definition.shape]}
                                                <div className='currentRelationshipImplicateItemName'>
                                                    {artefact.definition.name}
                                                </div>
                                                <div className='currentRelationshipImplicateItemName'>
                                                    {artefact.name}
                                                </div>
                                            </div>
                                        </MenuItem>
                            })
                            :
                            props.avaliableArtefacts.filter((artefact) => artefact.id != currentRelationship.child.id).map((artefact, index) => {
                                return  <MenuItem value={artefact} key={index}>
                                            <div className='currentRelationshipImplicateItemContainer'>
                                                {ArtefactIcons[artefact.definition.shape]}
                                                <div className='currentRelationshipImplicateItemName'>
                                                    {artefact.definition.name}
                                                </div>
                                                <div className='currentRelationshipImplicateItemName'>
                                                    {artefact.name}
                                                </div>
                                            </div>
                                        </MenuItem>
                            })
                        }
                    </Select>
                </div>
            </div>
            <div className='currentRelationshipChildContainer'>
                <div className='currentRelationshipTitle'>
                    Child
                </div>
                <div className='currentRelationshipValue'>
                    <Select 
                        variant="outlined"
                        value={currentRelationship.child || {}/*blank value, to avoid error of changing from undefined to not null*/}
                        onChange={(event) => setRelationshipInfo('child', event.target.value)}
                        error={!currentRelationship.child || currentRelationshipError == 'Relationship already exists'}
                    >
                    {
                        !currentRelationship.parent ?
                        props.avaliableArtefacts.map((artefact, index) => {
                            return  <MenuItem value={artefact} key={index}>
                                        <div className='currentRelationshipImplicateItemContainer'>
                                            {ArtefactIcons[artefact.definition.shape]}
                                            <div className='currentRelationshipImplicateItemName'>
                                                {artefact.definition.name}
                                            </div>
                                            <div className='currentRelationshipImplicateItemName'>
                                                {artefact.name}
                                            </div>
                                        </div>
                                    </MenuItem>
                        })
                        :
                        props.avaliableArtefacts.filter((artefact) => artefact.id != currentRelationship.parent.id).map((artefact, index) => {
                            return  <MenuItem value={artefact} key={index}>
                                        <div className='currentRelationshipImplicateItemContainer'>
                                            {ArtefactIcons[artefact.definition.shape]}
                                            <div className='currentRelationshipImplicateItemName'>
                                                {artefact.definition.name}
                                            </div>
                                            <div className='currentRelationshipImplicateItemName'>
                                                {artefact.name}
                                            </div>
                                        </div>
                                    </MenuItem>
                        })
                    }

                    </Select>
                </div>
            </div>
            <div className='currentRelationshipDescContainer'>
                <div className='currentRelationshipTitle'>
                    Description
                </div>
                <div className='currentRelationshipValue'>
                    <TextField 
                        variant="outlined"
                        value={currentRelationship.description}
                        onChange={(event) => setRelationshipInfo('description', event.target.value)}
                    />
                </div>
            </div>
            <div className='currentRelationshipAttrListContainer'>
                <div className='currentRelationshipAttrListTitle'>
                    Attributes
                </div>
                <div className='currentRelationshipAttrList'>
                    {currentRelationship.definition && currentRelationship.attributes.map((attribute, index) =>{
                        return  <Attribute 
                                    key={index}
                                    ind={index} 
                                    attrib={attribute}
                                    setAttribValue={setAttributeValue}
                                />
                    })}
                </div>
            </div>
            <div className='currentRelationshipCancelContainer'>
                <Button
                color='secondary'
                variant='contained'
                disableElevation={true}
                onClick={cancelRelationshipEdit}
                >
                    CANCEL
                </Button>
            </div>
            <div className='currentRelationshipCreateContainer'>
                <Button
                color={(currentRelationshipError=='' && currentRelationship.child && currentRelationship.parent) ? 'secondary' : 'error'} 
                variant={(currentRelationshipError=='' && currentRelationship.child && currentRelationship.parent) ? 'contained' : 'outlined'}
                onClick={(currentRelationshipError=='' && currentRelationship.child && currentRelationship.parent) ? validateRelationshipEdit : null}
                >
                    {currentRelationshipError != '' ? currentRelationshipError :
                    !currentRelationship.parent ? 'Relationship parent cannot be empty' :
                    !currentRelationship.child ? 'Relationship child cannot be empty' :
                     props.relationshipToEdit ? 'UPDATE' : 'CREATE'}
                </Button>
            </div>
        </div>
    );
    
}