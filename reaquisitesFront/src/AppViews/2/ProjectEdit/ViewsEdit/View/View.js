import { overTheme } from '../../../../../overTheme';
import './View.css';
import Centerer from '../../../../../MiniTools/Centerer/Centerer';
import { ArtefactIcons } from '../../../../../AppConsts';
import CJSArrowShow from '../../RelDefsEdit/RelDefEdit/CJSArrowShow/CJSArrowShow';
import { cytoscapeArrowHeads } from '../../../../../AppConsts';

export default function View (props) {
    

    const relationshipAttributeValuesParse = () =>{
        var finalString = '{ ';
        for (var i=0; i<props.visualization.definition.attributeDefinitions.length-1; i++){
            finalString+=props.visualization.attributes[i].definition.name+': '+props.visualization.attributes[i].value+',\n'
        }
        finalString += props.visualization.attributes[i].definition.name+': '+props.visualization.attributes[i].value +' }';
        return finalString;
    }


    return (
        <div className={props.selected ? 'visualizationContainer vcSelected' : 'visualizationContainer vcUnselected'}
            style={props.selected ? {backgroundColor: overTheme.palette.primary.light} : {backgroundColor: overTheme.palette.secondary.dark}}>
            <div className={props.selected ? 'visualizationHeadersContainer vhcSelected' : 'visualizationHeadersContainer vhcUnselected'}>
                <div className='visualizationShowInfo'
                onClick={()=>{
                    if (props.selected){
                        props.select(-1);
                    }else{
                        props.select(props.ind);
                    }
                }}>
                    <div className='visualizationDefinition'>
                        <Centerer>
                            {props.visualization.name}
                        </Centerer>
                    </div>
                    <div className='visualizationDescription'>
                        <Centerer>
                            {props.visualization.description!='' ? props.visualization.description : 'NO DESCRIPTION'}
                        </Centerer>
                    </div>
                </div>
                
                <div className='visualizationHeaders'>
                    <div className='visualizationValuesHeader'>
                        <Centerer>
                            Attributes
                        </Centerer>
                    </div>
                    <div className='visualizationDescHeader'>
                        <Centerer>
                            Values
                        </Centerer>
                    </div>
                </div>
            </div>
            <div className='visualizationExtraInfo'>
                <div className='visualizationAttributesInfo'>
                    {props.visualization.artefactColorFactors.map((factor, index)=>{
                        return  <div key={index} className='visualizationRelateds'>{factor.elementDefinition.name+' COLOR: '+factor.attributeDefinition.name}</div>
                    })}
                    {props.visualization.artefactSizeFactors.map((factor, index)=>{
                        return  <div key={index} className='visualizationRelateds'>{factor.elementDefinition.name+' SIZE: '+factor.attributeDefinition.name}</div>
                    })}
                    {props.visualization.relationshipColorFactors.map((factor, index)=>{
                        return  <div key={index} className='visualizationRelateds'>{factor.elementDefinition.name+' COLOR: '+factor.attributeDefinition.name}</div>
                    })}
                    {props.visualization.relationshipSizeFactors.map((factor, index)=>{
                        return  <div key={index} className='visualizationRelateds'>{factor.elementDefinition.name+' SIZE: '+factor.attributeDefinition.name}</div>
                    })}
                </div>
                <div className='visualizationDescInfo'>
                    {props.visualization.artefactColorFactors.map((factor, index)=>{
                        return  <div key={index} className='visualizationRelateds'>{factor.values.map((value, index2)=>{
                                    return <div key={index2} style={{display: 'inline', backgroundColor: 'rgb('+ value.r +', '+value.g+', '+value.b+')'}}>
                                                {'{point: '+value.key+', value: '+ value.r +' - '+value.g+' - '+value.b+'}'}
                                            </div>
                                })}</div>
                    })}
                    {props.visualization.artefactSizeFactors.map((factor, index)=>{
                        return  <div key={index} className='visualizationRelateds'>{factor.values.map((value, index2)=>{
                                    return <div key={index2} style={{display: 'inline'}}>{'{point: '+value.key+', value: '+value.size+'}'}</div>
                                })}</div>
                    })}
                    {props.visualization.relationshipColorFactors.map((factor, index)=>{
                        return  <div key={index} className='visualizationRelateds'>{factor.values.map((value, index2)=>{
                                    return <div key={index2} style={{display: 'inline', backgroundColor: 'rgb('+ value.r +', '+value.g+', '+value.b+')'}}>
                                        {'{point: '+value.key+', value: '+ value.r +' - '+value.g+' - '+value.b+'}'}
                                    </div>
                                })}</div>
                    })}
                    {props.visualization.relationshipSizeFactors.map((factor, index)=>{
                        return  <div key={index} className='visualizationRelateds'>{factor.values.map((value, index2)=>{
                                    return <div key={index2} style={{display: 'inline'}}>{'{point: '+value.key+', value: '+value.size+'}'}</div>
                                })}</div>
                    })}
                </div>
            </div>
        </div>
    );
    
}