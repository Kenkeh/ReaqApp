import { Button, IconButton, MenuItem, Select, TextField } from '@mui/material';
import AddIcon from '@mui/icons-material/Add';
import ClearIcon from '@mui/icons-material/Clear';
import { useEffect, useState, useCallback } from 'react';
import { overTheme } from '../../../../overTheme';
import './RelDefEdit.css';
import AttributeDef from '../../AttributeDef/AttributeDef';
import AttributeDefEdit from '../../AttributeDefEdit/AttributeDefEdit';
import { currentDate, cytoscapeArrowHeads } from '../../../../AppConsts';
import CJSArrowShow from './CJSArrowShow/CJSArrowShow';


export default function RelDefEdit (props) {

    const [currentRelDef, setCurrentRelDef] = useState({
        id: 0,
        shape: 0,
        name: '',
        description: '',
        attributeDefinitions: []
    });

    const [currentRelDefNameError, setCurrentRelDefNameError] = useState('Relationship Definition name cannot be empty');

    const [selectedAttributeDef, setSelectedAttributeDef] = useState(-1);
    const [creatingAttribDef, setCreatingAttribDef] = useState(false);

    const newRelDefIconGridStyle = {
        width: '400px',
        height: '400px',
        display: 'grid',
        gridTemplateColumns: '1fr 1fr 1fr 1fr 1fr 1fr 1fr 1fr 1fr 1fr',
        gridTemplateRows: '1fr 1fr 1fr 1fr 1fr 1fr 1fr 1fr 1fr 1fr'
    };


    useEffect(() =>{
        if (props.relDefToEdit){
            setCurrentRelDef(props.relDefToEdit);
            setCurrentRelDefNameError('');
        }
    },[props.relDefToEdit]);

    const setRelDefInfo = (info, value) =>{
        switch (info){
            case 'name':
                if (value==''){
                    setCurrentRelDefNameError('Relationship Definition name cannot be empty');
                }else if (props.otherRelDefs.find(relDef => relDef.name == value)){
                    setCurrentRelDefNameError('Relationship Definition already exists');
                }else{
                    setCurrentRelDefNameError('');
                }
                setCurrentRelDef({...currentRelDef, name: value});
                break;
            case "icon":
                setCurrentRelDef({...currentRelDef, shape: value});
                break;
            case 'description':
                setCurrentRelDef({...currentRelDef, description: value});
                break;
        }
    }

    const restartInfo = () =>{
        setCurrentRelDef({
            id:0,
            shape: 0,
            name: '',
            description: '',
            attributeDefinitions: []
        });
        setCurrentRelDefNameError('Relationship Definition name cannot be empty');
    }

    const deleteAttribute = () =>{
        var newAttribList = [...currentRelDef.attributeDefinitions];
        newAttribList.splice(selectedAttributeDef,1);
        setCurrentRelDef({...currentRelDef, attributeDefinitions: newAttribList});
        setSelectedAttributeDef(-1);
    }
    

    const cancelRelDefEdit = () =>{
        restartInfo();
        props.cancelRelDefEdition();
    }

    const validateRelDefEdit = () =>{
        if (props.relDefToEdit){
            const editionHistoryEntry = {
                elementType: 2,
                elementId: currentRelDef.id,
                changeType: 2,
                changeDate: currentDate(),
                changes: JSON.stringify({
                    old: props.relDefToEdit,
                    new: currentRelDef
                })
            }

            props.validateRelDefEdition(currentRelDef, props.relDefToEditIndex, editionHistoryEntry);
        }else{
            props.validateRelDefEdition(currentRelDef);
        }
        restartInfo();
        props.cancelRelDefEdition();
    }


    return (
        <div className='currentRelDefContainer' style={{backgroundColor: overTheme.palette.primary.dark}}>
            <div className='currentRelDefIconContainer'>
                <div className='currentRelDefTitle'>
                    Icon
                </div>
                <div className='currentRelDefValue'>
                    <Select value={currentRelDef.shape}
                    onChange={(event) => setRelDefInfo('icon', event.target.value)}
                    MenuProps={{MenuListProps: { style:{backgroundColor: 'black'}}}}>
                        {cytoscapeArrowHeads.map((arrowType, index) => {

                            return ['filled', 'hollow'].map((fillType, index2) =>{
                                return  <MenuItem value={index*2+index2}>
                                            <CJSArrowShow 
                                                selected={currentRelDef.shape==index*2+index2} 
                                                index={index*2+index2} 
                                                arrowType={arrowType} 
                                                fillType={fillType}
                                            />
                                        </MenuItem>
                            });
                            
                        })}
                    </Select>
                </div>
            </div>
            <div className='currentRelDefNameContainer'>
                <div className='currentRelDefTitle'>
                    Name
                </div>
                <div className='currentRelDefValue'>
                    <TextField 
                    variant="outlined"
                    value={currentRelDef.name}
                    onChange={(event) => setRelDefInfo('name', event.target.value)}
                    error={currentRelDefNameError != ''}
                    />
                </div>
            </div>
            <div className='currentRelDefDescContainer'>
                <div className='currentRelDefTitle'>
                    Description
                </div>
                <div className='currentRelDefValue'>
                    <TextField 
                    variant="outlined"
                    value={currentRelDef.description}
                    onChange={(event) => setRelDefInfo('description', event.target.value)}
                    />
                </div>
            </div>
            <div className='currentRelDefAttrListContainer'>
                <div className='currentRelDefAttrListTitle'>
                    Attributes
                </div>
                <div className='currentRelDefAttrDel'>
                    <IconButton disabled={selectedAttributeDef==-1} onClick={deleteAttribute}>
                        <ClearIcon style={selectedAttributeDef==-1 ? {color: 'grey'} : {color: overTheme.palette.primary.light}}/>
                    </IconButton>
                </div>
                <div className='currentRelDefAttrAdd'>
                    <IconButton disabled={creatingAttribDef} onClick={()=> setCreatingAttribDef(true)}>
                        <AddIcon style={creatingAttribDef ? {color: 'white'} : {color: overTheme.palette.primary.light}}/>
                    </IconButton>
                </div>
                <div className='currentRelDefAttrList'>
                    {creatingAttribDef ?
                    <AttributeDefEdit
                        cancelAttribDefEdition={() => setCreatingAttribDef(false)}
                        currentAttribDefs={currentRelDef.attributeDefinitions}
                        validateAttribDefEdition={(newAttrib) => 
                            setCurrentRelDef({...currentRelDef, attributeDefinitions: [...currentRelDef.attributeDefinitions, newAttrib]})}
                    />
                    : 
                    currentRelDef.attributeDefinitions.map((attrDef, index)=>{
                        return  <AttributeDef key={index}
                                ind={index} 
                                attribDef={attrDef}
                                select={setSelectedAttributeDef}
                                selected={selectedAttributeDef == index}
                                />
                    })}
                </div>
            </div>
            <div className='currentRelDefCancelContainer'>
                <Button
                color='secondary'
                variant='contained'
                disableElevation={true}
                onClick={cancelRelDefEdit}
                >
                    CANCEL
                </Button>
            </div>
            <div className='currentRelDefCreateContainer'>
                <Button
                color={currentRelDefNameError=='' ? 'secondary' : 'error'} 
                variant={currentRelDefNameError=='' ? 'contained' : 'outlined'}
                onClick={currentRelDefNameError=='' ? validateRelDefEdit : null}
                >
                    {currentRelDefNameError != '' ? currentRelDefNameError : props.relDefToEdit ? 'UPDATE' : 'CREATE'}
                </Button>
            </div>
        </div>
    );
    
}