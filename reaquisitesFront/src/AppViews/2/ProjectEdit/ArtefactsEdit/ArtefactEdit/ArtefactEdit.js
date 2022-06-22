import { useEffect, useState } from 'react';
import { overTheme } from '../../../../../overTheme';
import './ArtefactEdit.css';
import { Button, TextField, Select } from '@mui/material';


export default function ArtefactEdit (props) {

    const [currentArtefact, setCurrentArtefact] = useState({
        id: 0,
        definition: undefined,
        name: '',
        description: '',
        attributes: []
    });

    const [currentArtefactNameError, setCurrentArtefactNameError] = useState('Artefact name cannot be empty');

    useEffect(() =>{
        if (props.artefactToEdit){
            setCurrentArtefact(props.artefactToEdit);
            setCurrentArtefactNameError('');
        }
    },[props.artefactToEdit]);

    const setArtefactInfo = (info, value) =>{
        switch (info){
            case 'name':
                if (value==''){
                    setCurrentArtefactNameError('Artefact name cannot be empty');
                }else if (props.otherArtefacts.find(artefact => artefact.name == value && 
                    (artefact.definition.name == currentArtefact.definition.name && artefact.definition.shape == currentArtefact.definition.shape))){
                    setCurrentArtefactNameError('Artefact already exists');
                }else{
                    setCurrentArtefactNameError('');
                }
                setCurrentArtefact({...currentArtefact, name: value});
                break;
            case 'definition':
                setCurrentArtefact({...currentArtefact, definition: value});
                break;
            case 'description':
                setCurrentArtefact({...currentArtefact, description: value});
                break;
            case 'attributeValue':
                setCurrentArtefact({...currentArtefact, description: value});
                break;
        }
    }

    const restartInfo = () =>{
        setCurrentArtefact({
            id: 0,
            definition: undefined,
            name: '',
            description: '',
            attributes: []
        });
        setCurrentArtefactNameError('Artefact name cannot be empty');
    }

    return (
        <div className='currentArtefactContainer' style={{backgroundColor: overTheme.palette.primary.dark}}>
            <div className='currentArtefactIconContainer'>
                <div className='currentArtefactTitle'>
                    Definition
                </div>
                <div className='currentArtefactValue'>
                    <Select value={currentArtefact.definition}
                    onChange={(event) => setArtefactInfo('icon', event.target.value)}>
                        
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
                    error={currentArtefactNameError != ''}
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
                    LISTA ATRIBUTOS
                </div>
            </div>
            <div className='currentArtefactCancelContainer'>
                <Button
                color='secondary'
                variant='contained'
                disableElevation={true}
                onClick={null}
                >
                    CANCEL
                </Button>
            </div>
            <div className='currentArtefactCreateContainer'>
                <Button
                color={currentArtefactNameError=='' ? 'secondary' : 'error'} 
                variant={currentArtefactNameError=='' ? 'contained' : 'outlined'}
                onClick={currentArtefactNameError=='' ? null : null}
                >
                    {currentArtefactNameError != '' ? currentArtefactNameError : props.artefactToEdit ? 'UPDATE' : 'CREATE'}
                </Button>
            </div>
        </div>
    );
    
}