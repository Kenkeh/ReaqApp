import { Select, TextField, Slider, MenuItem } from '@mui/material';
import { useEffect, useState } from 'react';
import Centerer from '../../../../MiniTools/Centerer/Centerer';
import { overTheme } from '../../../../overTheme';
import './Attribute.css';


export default function Attribute (props) {
    
    const [attributeValues, setAttributeValues] = useState([]);

    useEffect(()=>{
        if (props.attrib.definition.type==2){
            setAttributeValues('');
        }else{
            setAttributeValues(JSON.parse(props.attrib.definition.values));
        }
    },[]);
    
    const parseAttributeType = () =>{
        switch(props.attrib.definition.type){
            case 0:
                return 'ENUM'
            case 1:
                return 'INTEGER'
            case 2:
                return 'STRING'
        }
    }

    const checkIntegerValue = (number) =>{
        if (number<attributeValues[0]){
            props.setAttribValue(props.ind, ''+attributeValues[0]);
        }else if (number>attributeValues[1]){
            props.setAttribValue(props.ind, ''+attributeValues[1]);
        }else{
            props.setAttribValue(props.ind, parseInt(number));
        }
    }

    const valuePicker = () =>{
        switch (props.attrib.definition.type){
            case 0:
                return  <Select 
                            color='secondary'
                            value={props.attrib.value}
                            onChange={(event) => props.setAttribValue(props.ind, event.target.value)}
                        >
                            {attributeValues.map((attrValue, index) =>{
                                return  <MenuItem key={index} value={attrValue}>
                                            {attrValue}
                                        </MenuItem>
                            })}
                        </Select>
            case 1:
                return  <div className='integerAttribValuePicker'>
                            <Centerer>
                                <Slider 
                                    color='secondary'
                                    value={parseInt(props.attrib.value)}
                                    valueLabelDisplay={'auto'}
                                    min={attributeValues[0]}
                                    max={attributeValues[1]}
                                    onChange={(event) => props.setAttribValue(props.ind, ''+event.target.value)}
                                />
                            </Centerer>
                            <Centerer>
                                <TextField
                                    value={parseInt(props.attrib.value)}
                                    type='number'
                                    inputProps={{ inputMode: 'numeric', pattern: '[0-9]+' }}
                                    onChange={(event) => checkIntegerValue(event.target.value)}
                                />
                            </Centerer>
                        </div>
                        
            default:
                return  <TextField
                            color='secondary'
                            value={props.attrib.value}
                            onChange={(event) => props.setAttribValue(props.ind, event.target.value)}
                        />
        }
    }

    return (
        <div className='attributeContainer' style={{backgroundColor: overTheme.palette.primary.main}}>
            <Centerer>
                <div className='attributeName'>
                    {props.attrib.definition.name}
                </div>
            </Centerer>
            <Centerer>
                {parseAttributeType()}
            </Centerer>
            <div className='attributeValuePicker'>
                {valuePicker()}
            </div>
        </div>
    );
    
}