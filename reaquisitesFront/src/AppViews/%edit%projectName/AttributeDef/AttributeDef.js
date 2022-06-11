import { style } from '@mui/system';
import { useEffect, useState } from 'react';
import Centerer from '../../../MiniTools/Centerer/Centerer';
import { overTheme } from '../../../overTheme';
import './AttributeDef.css';


export default function AttributeDef (props) {
    
    const attribDefType = () => {
        switch (props.attribDef.type){
            case 0:
                return 'ENUMERATE';
            case 1:
                return 'INTEGER';
            default:
                return 'STRING';
        }
    }

    const attribDefValues = () =>{
        switch (props.attribDef.type){
            case 0:
                const values = props.attribDef.values;
                var finalString = '{ ';
                for (var i=0; i<values.length-1; i++){
                    finalString += values[i] +', ';
                }
                finalString += values[values.length-1] + ' }';
                return finalString;
            case 1:
                return 'Range ['+props.attribDef.values[0]+' - '+props.attribDef.values[1]+']';
            default:
                return 'Any string';
        }
    }

    return (
        <div className={props.selected ? 'attrDefContainer adcSelected' : 'attrDefContainer adcUnselected'}
            style={props.selected ? {backgroundColor: overTheme.palette.primary.light} : {backgroundColor: overTheme.palette.secondary.dark}}>
            <div className={props.selected ? 'attrDefHeadersContainer adhcSelected' : 'attrDefHeadersContainer adhcUnselected'}>
                <div className='attrDefShowInfo'
                onClick={()=>{
                    if (props.selected){
                        props.select(-1);
                    }else{
                        props.select(props.ind);
                    }
                }}>
                    <div className='attrDefType'>
                        <Centerer>
                            {attribDefType()}
                        </Centerer>
                    </div>
                    <div className='attrDefName'>
                        <Centerer>
                            {props.attribDef.name}
                        </Centerer>
                    </div>
                </div>
                
                <div className='attrDefHeaders'>
                    <div className='attrDefValuesHeader'>
                        <Centerer>
                            Values
                        </Centerer>
                    </div>
                    <div className='attrDefDescHeader'>
                        <Centerer>
                            Description
                        </Centerer>
                    </div>
                </div>
            </div>
            <div className='attrDefExtraInfo'>
                <div className='attrDefValuesInfo'>
                    <Centerer>
                        {attribDefValues()}
                    </Centerer>
                </div>
                <div className='attrDefDescInfo'>
                    <Centerer>
                        {props.attribDef.description}
                    </Centerer>
                </div>
            </div>
        </div>
    );
    
}