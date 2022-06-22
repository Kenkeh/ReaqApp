import { Button, MenuItem, Select, Slider, TextField } from '@mui/material';
import { useState } from 'react';
import './AttributeDefEdit.css';
import { overTheme } from '../../../../overTheme';
import Centerer from '../../../../MiniTools/Centerer/Centerer';


export default function AttributeDefEdit (props) {

    const [attribDef, setAttribDef] = useState({
        type: 0,
        name: '',
        description: ''
    })


    const [attribDefNameError, setAttribDefNameError] = useState('Attribute name cannot be empty');

    const [currentEnumValue, setCurrentEnumValue] = useState('');
    const [currentEnumError, setCurrentEnumError] = useState(true);
    const [enumValues, setEnumValues] = useState([]);
    const [selectedEnumValue, setSelectedEnumValue] = useState(-1);

    const maxIntegerRange = [0,999];
    const [integerValues, setIntegerValues] = useState([maxIntegerRange[0],maxIntegerRange[1]]);

    const restartInfo = () =>{
        setAttribDef({
            type: 0,
            name: '',
            description: ''
        });
        setAttribDefNameError('Attribute name cannot be empty');
        setCurrentEnumValue('');
        setCurrentEnumError(true);
        setEnumValues([]);
        setSelectedEnumValue(-1);
        setIntegerValues([maxIntegerRange[0],maxIntegerRange[1]]);
    }
    const cancelAttribDefEdit = () =>{
        restartInfo();
        props.cancelAttribDefEdition();
    }
    const validateAttribDefEdit = () =>{
        switch (attribDef.type){
            case 0:
                props.validateAttribDefEdition({...attribDef, values: JSON.stringify(enumValues)});
                break;
            case 1:
                props.validateAttribDefEdition({...attribDef, values: JSON.stringify(integerValues)});
                break;
            default:
                props.validateAttribDefEdition({...attribDef, values: 'any string'});
                break;
        }
        cancelAttribDefEdit();
    }

    const checkEnumValue = (value) =>{
        if (value==''){
            setCurrentEnumError(true);
        }else{
            var found = false;
            for (var i=0; i<enumValues.length; i++){
                if (enumValues[i] == value){
                    setCurrentEnumError(true);
                    found =true;
                    break;
                }
            }
            if (!found) setCurrentEnumError(false);
        }
        setCurrentEnumValue(value);
    }
    const addEnumValue = () =>{
        setEnumValues([...enumValues, currentEnumValue]);
        setCurrentEnumError(true);
        setCurrentEnumValue('');
    }
    const toogleEnumValueSelection = (index) =>{
        setSelectedEnumValue( prev => prev == index ? -1 : index);
    }
    const removeEnumValue = () => {
        const newValues = [...enumValues];
        newValues.splice(selectedEnumValue,1);
        setSelectedEnumValue(-1);
        setEnumValues(newValues);
    }

    const checkIntegerRange = (event, newValue) =>{
        if (newValue[1]<=maxIntegerRange[0]){
            newValue[1]=maxIntegerRange[0]+1;
        }else if (newValue[0]>=maxIntegerRange[1]){
            newValue[0]=maxIntegerRange[1]-1;
        }
        setIntegerValues(newValue);
    }
    const checkIntegerValue = (value, first) =>{
        if (first){
            if (value>=integerValues[1]){
                setIntegerValues([integerValues[1]-1,integerValues[1]]);
            }else if (value<=maxIntegerRange[0]){
                setIntegerValues([maxIntegerRange[0],integerValues[1]]);
            }else{
                setIntegerValues([value,integerValues[1]]);
            }
        }else{
            if (value<=integerValues[0]){
                setIntegerValues([integerValues[0],integerValues[0]+1]);
            }else if (value>=maxIntegerRange[1]){
                setIntegerValues([integerValues[0],maxIntegerRange[1]]);
            }else{
                setIntegerValues([integerValues[0], value]);
            }
        }
    }

    const checkAttribName = (value) =>{
        if (value==''){
            setAttribDefNameError('Attribute name cannot be empty');
        }else{
            var found = false;
            for (var i=0; i<props.currentAttribDefs.length; i++){
                if (props.currentAttribDefs[i].name == value){
                    setAttribDefNameError('Attribute name already exists');
                    found = true;
                    break;
                }
            }
            if (!found) setAttribDefNameError('');
        }
        setAttribDef({...attribDef, name: value});
    }

    
    const attributeDefValues = (type) =>{
        switch (type){
            case 0:
                return  <div className='attribDefEditEnumValues'>
                            <div className='attribDefEditEnumControl'>
                                <div className='attribDefEditEnumInput'>
                                    <TextField 
                                    label='Value' 
                                    color='secondary'
                                    value={currentEnumValue}
                                    onChange={(event) => checkEnumValue(event.target.value)}
                                    />
                                </div>
                                <Button 
                                variant={currentEnumError ? 'outlined' : 'contained'} 
                                disableElevation
                                color={currentEnumError ? 'error' : 'secondary'}
                                onClick={currentEnumError ? null : addEnumValue}
                                >
                                    ADD
                                </Button>
                                <Button 
                                variant={selectedEnumValue>=0 ? 'contained' : 'outlined'} 
                                disableElevation
                                color={selectedEnumValue>=0 ? 'secondary' : 'error'}
                                onClick={selectedEnumValue<0 ? null : removeEnumValue}
                                >
                                    DELETE
                                </Button>
                            </div>
                            
                            <div className='attribDefEditEnumList'>
                                {enumValues.map((value, index) =>{
                                    return  <div key={index} className='attribDefEditEnumValue'
                                            style={selectedEnumValue == index ? 
                                            {backgroundColor: overTheme.palette.secondary.dark} : {backgroundColor: overTheme.palette.secondary.main}}
                                            onClick={ () => toogleEnumValueSelection(index)}
                                            >
                                                <Centerer>
                                                    {value}
                                                </Centerer>
                                            </div>
                                })}
                            </div>
                        </div>
            case 1:
                return  <div className='attribDefEditIntegerValues'>
                            <div className='adivIntegerElem'>
                                <Centerer>
                                    <Slider
                                    color='secondary'
                                    value={integerValues}
                                    valueLabelDisplay="auto"
                                    onChange={checkIntegerRange}
                                    min={maxIntegerRange[0]}
                                    max={maxIntegerRange[1]}
                                    />
                                </Centerer>
                            </div>
                            <div className='adivIntegerElem adivInputElem'>
                                <Centerer>
                                    Minimum
                                </Centerer>
                                <TextField
                                value={integerValues[0]}
                                style={{height: '45px'}}
                                type='number'
                                inputProps={{ inputMode: 'numeric', pattern: '[0-9]+' }}
                                onChange={(event) => checkIntegerValue(event.target.value, true)}
                                />
                            </div>
                            <div className='adivIntegerElem adivInputElem'>
                                <Centerer>
                                    Maximum
                                </Centerer>
                                <TextField
                                value={integerValues[1]}
                                style={{height: '45px'}}
                                type='number'
                                inputProps={{ inputMode: 'numeric', pattern: '[0-9]+' }}
                                onChange={(event) => checkIntegerValue(event.target.value, false)}
                                />
                            </div>
                        </div>
            default:
                return undefined;
        }
    }

    return (
        <div className='attribDefEditContainer'>
            <div className='attribDefEditName'>
                <TextField 
                label='Name' 
                color={attribDefNameError ? 'error' : 'secondary'}
                value={attribDef.name}
                onChange={(event) => checkAttribName(event.target.value)}
                />
            </div>
            <div className='attribDefEditDesc'>
                <TextField 
                label='Description' 
                color='secondary'
                value={attribDef.description}
                onChange={(event) => setAttribDef({...attribDef, description: event.target.value})}
                />
            </div>
            <div className='attribDefEditType'>
                <Select value={attribDef.type} color='secondary' onChange={(event)=>{setAttribDef({...attribDef, type: event.target.value})}}>
                    <MenuItem value={0}>ENUMERATE</MenuItem>
                    <MenuItem value={1}>INTEGER</MenuItem>
                    <MenuItem value={2}>STRING</MenuItem>
                </Select>
            </div>
            <div className='attribDefEditValues'>
                {attributeDefValues(attribDef.type)}
            </div>
            <div>
                <Button 
                variant='contained' 
                color='secondary' 
                disableElevation={true}
                onClick={() => {cancelAttribDefEdit()}}>
                    CANCEL
                </Button>
            </div>
            <div>
                <Button 
                variant={(attribDefNameError!='' || (attribDef.type==0 && enumValues.length==0)) ? 'outlined' : 'contained'}
                color={(attribDefNameError!='' || (attribDef.type==0 && enumValues.length==0)) ? 'error' : 'secondary'}
                disableElevation={true}
                onClick={(attribDefNameError!='' || (attribDef.type==0 && enumValues.length==0)) ? null : validateAttribDefEdit}>
                    {attribDefNameError!='' ? attribDefNameError 
                    : (attribDef.type==0 && enumValues.length==0) ? 'Enumerate values cannot be empty'
                    : 'CREATE'}
                </Button>
            </div>
        </div>
    );
    
}