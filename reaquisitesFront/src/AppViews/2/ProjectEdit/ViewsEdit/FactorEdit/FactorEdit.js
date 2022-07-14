import { Button, MenuItem, Select, Slider, TextField } from '@mui/material';
import { useState } from 'react';
import './FactorEdit.css';
import { overTheme } from '../../../../../overTheme';
import Centerer from '../../../../../MiniTools/Centerer/Centerer';
import { cytoscapeArrowHeads, ArtefactIcons } from '../../../../../AppConsts';
import CJSArrowShow from '../../RelDefsEdit/RelDefEdit/CJSArrowShow/CJSArrowShow';


export default function FactorEdit (props) {

    const [factor, setFactor] = useState({
        type: 0,
        elementType: 0,
        elementDefinition: {},
        attributeDefinition: {},
        interpolated: false,
        weight: 1,
        values: []
    });

    const [currentElementIndex, setCurrentElementIndex] = useState(-1);
    const [currentAttributeIndex, setCurrentAttributeIndex] = useState(0);
    const [factorError, setFactorError] = useState(false);
    const [factorValuesError, setFactorValuesError] = useState(false);

    const [currentEnumValue, setCurrentEnumValue] = useState(-1);
    const [currentStringValue, setCurrentStringValue] = useState('');
    const [currentRangeValue, setCurrentRangeValue] = useState([0,1]);


    const restartInfo = () =>{
        setFactor({
            type: 0,
            elementType: 0,
            elementDefinition: {},
            attributeDefinition: {},
            interpolated: false,
            weight: 1,
            values: []
        });
    }
    const cancelFactorEdit = () =>{
        restartInfo();
        props.cancelFactorEdition();
    }

    const checkFactor = (info, value) =>{
        switch (info){
            case 'type':
                setFactor({...factor, type: value, values: []});
                break;
            case 'element':
                setFactor({...factor, elementDefinition: value.elem, elementType: value.type, attributeDefinition: value.elem.attributeDefinitions[0], values: []});
                break;
            case 'attribute':
                setFactor({...factor, attributeDefinition: value, values: []});
                break;
        }
        setFactorValuesError(false);
        if (factor.elementDefinition.description && factor.attributeDefinition.name){
            if (factor.elementType==1){
                if (props.relationshipFactors.find((fact) => 
                fact.elementDefinition.id == factor.elementDefinition.id && fact.attribute.name == factor.attributeDefinition.name)){
                    setFactorError(true);
                }
            }else{
                if (props.artefactFactors.find((fact) => 
                fact.elementDefinition.id == factor.elementDefinition.id && fact.attribute.name == factor.attributeDefinition.name)){
                    setFactorError(true);
                }
            }
        }
    }

    const checkFactorValuesError = (newValues) =>{
        var errorFound = false;
        var valuesToSearch = factor.values;
        if (newValues) valuesToSearch = newValues;
        switch (factor.attributeDefinition.type){
            case 0:
                for (var iR=0; iR<valuesToSearch.length; iR++){
                    for (var jR=iR+1; jR<valuesToSearch.length; jR++){
                        if (valuesToSearch[iR].key == valuesToSearch[jR].key){
                            errorFound=true;
                            break;
                        }
                    }
                    if (errorFound) break;
                }
                break;
            case 1:
                for (var iR=0; iR<valuesToSearch.length; iR++){
                    for (var jR=iR+1; jR<valuesToSearch.length; jR++){
                        if ((valuesToSearch[iR].key[0] <= valuesToSearch[jR].key[0] && valuesToSearch[iR].key[1] >= valuesToSearch[jR].key[0])
                        || (valuesToSearch[iR].key[0] <= valuesToSearch[jR].key[1] && valuesToSearch[iR].key[1] >= valuesToSearch[jR].key[1])){
                            errorFound=true;
                            break;
                        }
                    }
                    if (errorFound) break;
                }
                break;
            case 2:
                for (var iR=0; iR<valuesToSearch.length; iR++){
                    for (var jR=iR+1; jR<valuesToSearch.length; jR++){
                        if (valuesToSearch[iR].key == valuesToSearch[jR].key){
                            errorFound=true;
                            break;
                        }
                    }
                    if (errorFound) break;
                }
                break;
        }
        setFactorValuesError(errorFound);
    }

    const addNewFactorValue = () =>{

        switch (factor.attributeDefinition.type){
            case 0:
                const keyValueEnum = JSON.parse(factor.attributeDefinition.values)[currentEnumValue];
                if (factor.values.find((value) => value.key == keyValueEnum)){
                    setFactorValuesError(true);
                }else{
                    checkFactorValuesError();
                }
                if (factor.type == 0){
                    setFactor({...factor, values: [...factor.values, 
                        { key: keyValueEnum, R: 255, G: 255, B: 255, A: 1}]});
                }else{
                    setFactor({...factor, values: [...factor.values, 
                        { key: keyValueEnum, size: 1}]});
                }
                break;
            case 1:
                const keyValueRange = [currentRangeValue[0], currentRangeValue[1]];
                if (factor.values.find((value) => 
                    (value.key[0] <= keyValueRange[0] && value.key[1] >= keyValueRange[0])
                    || (value.key[0] <= keyValueRange[1] && value.key[1] >= keyValueRange[1]))){
                    setFactorValuesError(true);
                }else{
                    checkFactorValuesError();
                }
                if (factor.type == 0){
                    setFactor({...factor, values: [...factor.values, 
                        { key: JSON.stringify(keyValueRange), R: 255, G: 255, B: 255, A: 1}]});
                }else{
                    setFactor({...factor, values: [...factor.values, 
                        { key: JSON.stringify(keyValueRange), size: 1}]});
                }
                break;
            case 2:
                if (factor.values.find((value) => value.key == currentStringValue)){
                    setFactorValuesError(true);
                }else{
                    checkFactorValuesError();
                }
                if (factor.type == 0){
                    setFactor({...factor, values: [...factor.values, 
                        { key: currentStringValue, R: 255, G: 255, B: 255, A: 1}]});
                }else{
                    setFactor({...factor, values: [...factor.values, 
                        { key: currentStringValue, size: 1}]});
                }
                break;

        }
    }

    const editFactorValue = (index, value, type, index2) =>{
        var newValues = [...factor.values];
        if (type==0){
            if (value>255) value = 255;
            else if (value<0) value = 0;
            switch(index2){
                case 0: 
                    newValues[index].R = value;
                    break;
                case 1: 
                    newValues[index].G = value;
                    break;
                case 2: 
                    newValues[index].B = value;
                    break;
                case 3: 
                    newValues[index].A = value;
                    break;
            }
        }else{
            if (value>100) value = 100;
            else if (value<1) value = 1;
            newValues[index].size = value;
        }
        setFactor({...factor, values: newValues});
    }

    const deleteFactorValue = (index) =>{
        var newValues = [...factor.values];
        newValues.splice(index, 1);
        if (newValues.length==0)
            setFactorError(0);
        setFactor({...factor, values: newValues});
        checkFactorValuesError(newValues);
    }

    return (
        <div className='factorEditContainer'>
            <div className='factorEditType'>
                <Select
                    value={factor.type}
                    onChange={(event) => checkFactor('type',event.target.value)}
                >
                    <MenuItem value={0}>
                        COLOR
                    </MenuItem>
                    <MenuItem value={1}>
                        SIZE
                    </MenuItem>
                </Select>
            </div>
            <div className='factorEditElement'>
                <Select
                    value={currentElementIndex}
                    onChange={(event) => {
                        const ind = event.target.value;
                        setCurrentElementIndex(ind);
                        setCurrentAttributeIndex(0);
                        if (ind>=props.avaliableArtDefs.filter((artDef) => artDef.attributeDefinitions.length>0).length){
                            checkFactor('element', {elem: props.avaliableRelDefs.filter(
                                (relDef) => relDef.attributeDefinitions.length>0)[ind-props.avaliableArtDefs.filter(
                                    (artDef) => artDef.attributeDefinitions.length>0).length], type: 1}
                            );
                        }else{
                            checkFactor('element',{elem: props.avaliableArtDefs.filter(
                                (artDef) => artDef.attributeDefinitions.length>0)[ind], type:0});
                        }
                    }}
                >
                    {props.avaliableArtDefs.filter((artDef) => artDef.attributeDefinitions.length>0).map((artDef, index) =>{
                        return  <MenuItem key={index} value={index}>
                                    <div className='currentArtefactDefinitionItem'>
                                        {ArtefactIcons[artDef.shape]}
                                        <div className='currentArtefactDefinitionItemName'>
                                            {artDef.name}
                                        </div>
                                    </div>
                                </MenuItem>
                    })}
                    {props.avaliableRelDefs.filter((relDef) => relDef.attributeDefinitions.length>0).map((relDef, index) => {
                        return  <MenuItem value={index + props.avaliableArtDefs.length} key={index + props.avaliableArtDefs.length}>
                                    <div className='currentRelationshipDefinitionItemContainer'>
                                        <CJSArrowShow 
                                            selected={currentElementIndex == index + props.avaliableArtDefs.length} 
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
            <div className='factorEditAttrib'>
                <Select
                    value={currentAttributeIndex}
                    onChange={(event) => {
                        const ind = event.target.value;
                        setCurrentAttributeIndex(ind);
                        checkFactor('attribute', factor.elementDefinition.attributeDefinitions[ind]);
                    }}
                >
                    {factor.elementDefinition.id &&
                    factor.elementDefinition.attributeDefinitions.map((attribDef, index) =>{
                        return  <MenuItem key={index} value={index}>
                                    <div className='currentArtefactDefinitionItem'>
                                        {attribDef.type == 0 ? 'ENUMERATE' :
                                        attribDef.type == 1 ? 'INTEGER' :
                                        'STRING'
                                        }
                                        <div className='currentArtefactDefinitionItemName'>
                                            {attribDef.name}
                                        </div>
                                    </div>
                                </MenuItem>
                    })}
                </Select>
            </div>
            <div className='factorEditValues'>
                {factor.attributeDefinition.name &&
                <>
                    <div className='factorEditValuesAdder'>
                        {factor.attributeDefinition.type == 0 ?
                            <Select
                                value={currentEnumValue}
                                onChange={(event) => setCurrentEnumValue(event.target.value)}
                            >
                                {JSON.parse(factor.attributeDefinition.values).map((value, index) =>{
                                    return  <MenuItem key={index} value={index}>
                                                <Centerer>
                                                    {value}
                                                </Centerer>
                                            </MenuItem>
                                })}
                            </Select>
                        : factor.attributeDefinition.type == 1 ?
                            <div className='factorEditValuesAdderInteger'>
                                <Centerer>
                                    <TextField
                                        value={currentRangeValue[0]}
                                        type='number'
                                        color='secondary'
                                        inputProps={{ inputMode: 'numeric', pattern: '[0-9]+' }}
                                        onChange={(event) => {
                                            const newValue = parseInt(event.target.value);
                                            if (newValue<parseInt(JSON.parse(factor.attributeDefinition.values)[0])){
                                                setCurrentRangeValue([JSON.parse(factor.attributeDefinition.values)[0], currentRangeValue[1]]);
                                            }else{
                                                setCurrentRangeValue([newValue, currentRangeValue[1]]);
                                            }
                                        }}
                                    />
                                </Centerer>
                                <Centerer>
                                    <Slider
                                    color='secondary'
                                    value={currentRangeValue}
                                    valueLabelDisplay="auto"
                                    onChange={(event) => setCurrentRangeValue(event.target.value)}
                                    min={parseInt(JSON.parse(factor.attributeDefinition.values)[0])}
                                    max={parseInt(JSON.parse(factor.attributeDefinition.values)[1])}
                                    />
                                </Centerer>
                                <Centerer>
                                    <TextField
                                        value={currentRangeValue[1]}
                                        type='number'
                                        color='secondary'
                                        inputProps={{ inputMode: 'numeric', pattern: '[0-9]+' }}
                                        onChange={(event) => {
                                            const newValue = parseInt(event.target.value);
                                            if (newValue>parseInt(JSON.parse(factor.attributeDefinition.values)[1])){
                                                setCurrentRangeValue([currentRangeValue[0], JSON.parse(factor.attributeDefinition.values)[1]]);
                                            }else{
                                                setCurrentRangeValue([currentRangeValue[0], newValue]);
                                            }
                                        }}
                                        />
                                </Centerer>
                            </div>
                        :
                            <TextField
                                variant="outlined"
                                color='secondary'
                                value={currentStringValue}
                                onChange={(event) => setCurrentStringValue(event.target.value)}
                            />
                        }
                        <Button
                        variant='contained' 
                        color='secondary' 
                        disableElevation={true}
                        onClick={addNewFactorValue}
                        >
                            ADD
                        </Button>
                    </div>
                    {factor.values.map((value, index)=>{
                        return  <div key={index} className='factorEditPointContainer' 
                                style={{backgroundColor: overTheme.palette.secondary.main}}>
                                    <Centerer>
                                        {value.key.length == 1 ? value.key : value.key.length==2 ? '['+value.key[0] + ', '+value.key[1]+']' : value.key}
                                    </Centerer>
                                    {factor.type == 0 ?
                                        <div className='factorEditPointColor'> 
                                            <Centerer>
                                                R:
                                            </Centerer>
                                            <Centerer>
                                                <TextField
                                                    variant="outlined"
                                                    color='secondary'
                                                    type='number'
                                                    inputProps={{ inputMode: 'numeric', pattern: '[0-9]+' }}
                                                    value={value.R}
                                                    onChange={(event) => editFactorValue(index, parseInt(event.target.value), 0, 0)}
                                                />
                                            </Centerer>
                                            <Centerer>
                                                G:
                                            </Centerer>
                                            <Centerer>
                                                <TextField
                                                    variant="outlined"
                                                    type='number'
                                                    color='secondary'
                                                    inputProps={{ inputMode: 'numeric', pattern: '[0-9]+' }}
                                                    value={value.G}
                                                    onChange={(event) => editFactorValue(index, parseInt(event.target.value), 0, 1)}
                                                />
                                            </Centerer>
                                            <Centerer>
                                                B:
                                            </Centerer>
                                            <Centerer>
                                                <TextField
                                                    variant="outlined"
                                                    type='number'
                                                    color='secondary'
                                                    inputProps={{ inputMode: 'numeric', pattern: '[0-9]+' }}
                                                    value={value.B}
                                                    onChange={(event) => editFactorValue(index, parseInt(event.target.value), 0, 2)}
                                                />
                                            </Centerer>
                                        </div>
                                        :
                                        <div className='factorEditPointSize'>
                                            <Centerer>
                                                Tamaño:
                                            </Centerer>
                                            <Centerer>
                                                <Slider 
                                                    color='primary'
                                                    value={value.value}
                                                    valueLabelDisplay={'auto'}
                                                    min={1}
                                                    max={100}
                                                    onChange={
                                                        (event) => editFactorValue(index, event.target.value, 1)
                                                    }
                                                />
                                            </Centerer>
                                            <Centerer>
                                                <TextField
                                                    variant="outlined"
                                                    type='number'
                                                    color='secondary'
                                                    inputProps={{ inputMode: 'numeric', pattern: '[0-9]+' }}
                                                    value={value.size}
                                                    onChange={(event) => editFactorValue(index, parseInt(event.target.value), 1)}
                                                />
                                            </Centerer>
                                        </div>
                                    }
                                    <Button
                                    variant='contained' 
                                    style={value.R !=undefined ? {color: 'black', backgroundColor: 'rgb('+value.R+', '+value.G+', '+value.B+')'}
                                    : {color: 'black', backgroundColor: overTheme.palette.error.main}}
                                    disableElevation={true}
                                    onClick={() => deleteFactorValue(index)}
                                    >
                                        DEL
                                    </Button>
                                </div> 
                    })}
                </>
                }
            </div>
            <div className='factorEditCancel'>
                <Button 
                variant='contained' 
                color='secondary' 
                disableElevation={true}
                onClick={() => {cancelFactorEdit()}}>
                    CANCEL
                </Button>
            </div>
            <div>
                <Button 
                variant={factor.values.length == 0 || factorValuesError || factorError ? 'outlined' : 'contained'}
                color={factor.values.length == 0 || factorValuesError || factorError ? 'error' : 'secondary'}
                disableElevation={true}
                onClick={factorError ? null : ()=>{
                    props.validateFactorEdition(factor);
                    cancelFactorEdit();
                }}>
                    {factor.values.length == 0 ? 'Need Info'
                    : factorValuesError ? 'Factor point duplicated' 
                    : factorError ? 'Factor already exits' : 'CREATE'}
                </Button>
            </div>
        </div>
    );
    
}