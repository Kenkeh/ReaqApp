import { Button, IconButton, MenuItem, Select, TextField } from '@mui/material';
import AddIcon from '@mui/icons-material/Add';
import ClearIcon from '@mui/icons-material/Clear';
import AddTaskTwoToneIcon from '@mui/icons-material/AddTaskTwoTone';
import TaskTwoToneIcon from '@mui/icons-material/TaskTwoTone';
import TaskAltTwoToneIcon from '@mui/icons-material/TaskAltTwoTone';
import AssignmentTwoToneIcon from '@mui/icons-material/AssignmentTwoTone';
import PlaylistAddCheckTwoToneIcon from '@mui/icons-material/PlaylistAddCheckTwoTone';
import AssignmentIndTwoToneIcon from '@mui/icons-material/AssignmentIndTwoTone';
import AssignmentReturnedTwoToneIcon from '@mui/icons-material/AssignmentReturnedTwoTone';
import AssignmentTurnedInTwoToneIcon from '@mui/icons-material/AssignmentTurnedInTwoTone';
import AssignmentLateTwoToneIcon from '@mui/icons-material/AssignmentLateTwoTone';
import FormatListNumberedTwoToneIcon from '@mui/icons-material/FormatListNumberedTwoTone';
import ListAltTwoToneIcon from '@mui/icons-material/ListAltTwoTone';
import ErrorTwoToneIcon from '@mui/icons-material/ErrorTwoTone';
import BrokenImageTwoToneIcon from '@mui/icons-material/BrokenImageTwoTone';
import ThumbUpTwoToneIcon from '@mui/icons-material/ThumbUpTwoTone';
import ClassTwoToneIcon from '@mui/icons-material/ClassTwoTone';
import AccountTreeTwoToneIcon from '@mui/icons-material/AccountTreeTwoTone';
import AccessTimeTwoToneIcon from '@mui/icons-material/AccessTimeTwoTone';
import TimelapseTwoToneIcon from '@mui/icons-material/TimelapseTwoTone';
import SentimentVerySatisfiedTwoToneIcon from '@mui/icons-material/SentimentVerySatisfiedTwoTone';
import SentimentVeryDissatisfiedTwoToneIcon from '@mui/icons-material/SentimentVeryDissatisfiedTwoTone';
import AssuredWorkloadTwoToneIcon from '@mui/icons-material/AssuredWorkloadTwoTone';
import GroupWorkTwoToneIcon from '@mui/icons-material/GroupWorkTwoTone';
import WorkspacesTwoToneIcon from '@mui/icons-material/WorkspacesTwoTone';
import WorkspacePremiumTwoToneIcon from '@mui/icons-material/WorkspacePremiumTwoTone';
import WorkTwoToneIcon from '@mui/icons-material/WorkTwoTone';
import WorkOffTwoToneIcon from '@mui/icons-material/WorkOffTwoTone';
import { useEffect, useState } from 'react';
import { overTheme } from '../../../../overTheme';
import './ArtDefEdit.css';
import AttributeDef from '../../AttributeDef/AttributeDef';
import AttributeDefEdit from '../../AttributeDefEdit/AttributeDefEdit';
import { currentDate } from '../../../../AppConsts';


export default function ArtDefEdit (props) {

    const [currentArtDef, setCurrentArtDef] = useState({
        shape: 0,
        name: '',
        description: '',
        attributeDefinitions: []
    });

    const [currentArtDefNameError, setCurrentArtDefNameError] = useState('Artefact Definition name cannot be empty');

    const [selectedAttributeDef, setSelectedAttributeDef] = useState(-1);
    const [creatingAttribDef, setCreatingAttribDef] = useState(false);

    const newArtDefIconGridStyle = {
        width: '400px',
        height: '400px',
        display: 'grid',
        gridTemplateColumns: '1fr 1fr 1fr 1fr 1fr 1fr 1fr 1fr 1fr 1fr',
        gridTemplateRows: '1fr 1fr 1fr 1fr 1fr 1fr 1fr 1fr 1fr 1fr'
    };

    useEffect(() =>{
        if (props.artDefToEdit){
            setCurrentArtDef(props.artDefToEdit);
            setCurrentArtDefNameError('');
        }
    },[props.artDefToEdit]);

    const setArtDefInfo = (info, value) =>{
        switch (info){
            case 'name':
                if (value==''){
                    setCurrentArtDefNameError('Artefact Definition name cannot be empty');
                }else if (props.otherArtDefs.find(artDef => artDef.name == value)){
                    setCurrentArtDefNameError('Artefact Definition already exists');
                }else{
                    setCurrentArtDefNameError('');
                }
                setCurrentArtDef({...currentArtDef, name: value});
                break;
            case 'icon':
                setCurrentArtDef({...currentArtDef, shape: value});
                break;
            case 'description':
                setCurrentArtDef({...currentArtDef, description: value});
                break;
        }
    }

    const restartInfo = () =>{
        setCurrentArtDef({
            shape: 0,
            name: '',
            description: '',
            attributeDefinitions: []
        });
        setCurrentArtDefNameError('Artefact Definition name cannot be empty');
    }

    const deleteAttribute = () =>{
        var newAttribList = [...currentArtDef.attributeDefinitions];
        newAttribList.splice(selectedAttributeDef,1);
        setCurrentArtDef({...currentArtDef, attributeDefinitions: newAttribList});
        setSelectedAttributeDef(-1);
    }
    

    const cancelArtDefEdit = () =>{
        restartInfo();
        props.cancelArtDefEdition();
    }

    const validateArtDefEdit = () =>{
        if (props.artDefToEdit){
            const editionHistoryEntry = {
                type: 2,
                changeDate: currentDate(),
                changes: JSON.stringify({
                    oldArtefact: props.artDefToEdit,
                    newArtefact: currentArtDef
                })
            }
            setCurrentArtDef({...currentArtDef, 
                historyEntries: currentRelDef.historyEntries ? 
                    [...currentRelDef.historyEntries, editionHistoryEntry]
                    :
                    [editionHistoryEntry]});
            props.validateArtDefEdition(currentArtDef, props.artDefToEditIndex);
        }else{
            props.validateArtDefEdition(currentArtDef);
        }
        restartInfo();
        props.cancelArtDefEdition();
    }

    return (
        <div className='currentArtDefContainer' style={{backgroundColor: overTheme.palette.primary.dark}}>
            <div className='currentArtDefIconContainer'>
                <div className='currentArtDefTitle'>
                    Icon
                </div>
                <div className='currentArtDefValue'>
                    <Select value={currentArtDef.shape} 
                    MenuProps={{style: newArtDefIconGridStyle}} 
                    onChange={(event) => setArtDefInfo('icon', event.target.value)}>
                        <MenuItem value={0}><TaskTwoToneIcon/></MenuItem>
                        <MenuItem value={1}><AddTaskTwoToneIcon/></MenuItem>
                        <MenuItem value={2}><TaskAltTwoToneIcon/></MenuItem>
                        <MenuItem value={3}><AssignmentTwoToneIcon/></MenuItem>
                        <MenuItem value={4}><PlaylistAddCheckTwoToneIcon/></MenuItem>
                        <MenuItem value={5}><AssignmentIndTwoToneIcon/></MenuItem>
                        <MenuItem value={6}><AssignmentReturnedTwoToneIcon/></MenuItem>
                        <MenuItem value={7}><AssignmentTurnedInTwoToneIcon/></MenuItem>
                        <MenuItem value={8}><AssignmentLateTwoToneIcon/></MenuItem>
                        <MenuItem value={9}><FormatListNumberedTwoToneIcon/></MenuItem>
                        <MenuItem value={10}><ListAltTwoToneIcon/></MenuItem>
                        <MenuItem value={11}><ErrorTwoToneIcon/></MenuItem>
                        <MenuItem value={12}><BrokenImageTwoToneIcon/></MenuItem>
                        <MenuItem value={13}><ThumbUpTwoToneIcon/></MenuItem>
                        <MenuItem value={14}><ClassTwoToneIcon/></MenuItem>
                        <MenuItem value={15}><AccountTreeTwoToneIcon/></MenuItem>
                        <MenuItem value={16}><AccessTimeTwoToneIcon/></MenuItem>
                        <MenuItem value={17}><TimelapseTwoToneIcon/></MenuItem>
                        <MenuItem value={18}><SentimentVerySatisfiedTwoToneIcon/></MenuItem>
                        <MenuItem value={19}><SentimentVeryDissatisfiedTwoToneIcon/></MenuItem>
                        <MenuItem value={20}><AssuredWorkloadTwoToneIcon/></MenuItem>
                        <MenuItem value={21}><GroupWorkTwoToneIcon/></MenuItem>
                        <MenuItem value={22}><WorkspacesTwoToneIcon/></MenuItem>
                        <MenuItem value={23}><WorkspacePremiumTwoToneIcon/></MenuItem>
                        <MenuItem value={24}><WorkTwoToneIcon/></MenuItem>
                        <MenuItem value={25}><WorkOffTwoToneIcon/></MenuItem>
                        <MenuItem value={26}><GroupWorkTwoToneIcon/></MenuItem>
                    </Select>
                </div>
            </div>
            <div className='currentArtDefNameContainer'>
                <div className='currentArtDefTitle'>
                    Name
                </div>
                <div className='currentArtDefValue'>
                    <TextField 
                    variant="outlined"
                    value={currentArtDef.name}
                    onChange={(event) => setArtDefInfo('name', event.target.value)}
                    error={currentArtDefNameError != ''}
                    />
                </div>
            </div>
            <div className='currentArtDefDescContainer'>
                <div className='currentArtDefTitle'>
                    Description
                </div>
                <div className='currentArtDefValue'>
                    <TextField 
                    variant="outlined"
                    value={currentArtDef.description}
                    onChange={(event) => setArtDefInfo('description', event.target.value)}
                    />
                </div>
            </div>
            <div className='currentArtDefAttrListContainer'>
                <div className='currentArtDefAttrListTitle'>
                    Attributes
                </div>
                <div className='currentArtDefAttrDel'>
                    <IconButton disabled={selectedAttributeDef==-1} onClick={deleteAttribute}>
                        <ClearIcon style={selectedAttributeDef==-1 ? {color: 'grey'} : {color: overTheme.palette.primary.light}}/>
                    </IconButton>
                </div>
                <div className='currentArtDefAttrAdd'>
                    <IconButton disabled={creatingAttribDef} onClick={()=> setCreatingAttribDef(true)}>
                        <AddIcon style={creatingAttribDef ? {color: 'white'} : {color: overTheme.palette.primary.light}}/>
                    </IconButton>
                </div>
                <div className='currentArtDefAttrList'>
                    {creatingAttribDef ?
                    <AttributeDefEdit
                    cancelAttribDefEdition={() => setCreatingAttribDef(false)}
                    currentAttribDefs={currentArtDef.attributeDefinitions}
                    validateAttribDefEdition={(newAttrib) => 
                        setCurrentArtDef({...currentArtDef, attributeDefinitions: [...currentArtDef.attributeDefinitions, newAttrib]})}
                    />
                    : 
                    currentArtDef.attributeDefinitions.map((attrDef, index)=>{
                        return  <AttributeDef key={index}
                                ind={index} 
                                attribDef={attrDef}
                                select={setSelectedAttributeDef}
                                selected={selectedAttributeDef == index}
                                />
                    })}
                </div>
            </div>
            <div className='currentArtDefCancelContainer'>
                <Button
                color='secondary'
                variant='contained'
                disableElevation={true}
                onClick={cancelArtDefEdit}
                >
                    CANCEL
                </Button>
            </div>
            <div className='currentArtDefCreateContainer'>
                <Button
                color={currentArtDefNameError=='' ? 'secondary' : 'error'} 
                variant={currentArtDefNameError=='' ? 'contained' : 'outlined'}
                onClick={currentArtDefNameError=='' ? validateArtDefEdit : null}
                >
                    {currentArtDefNameError != '' ? currentArtDefNameError : 'CREATE'}
                </Button>
            </div>
        </div>
    );
    
}