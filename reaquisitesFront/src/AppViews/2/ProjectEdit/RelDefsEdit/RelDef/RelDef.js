import { overTheme } from '../../../../../overTheme';
import './RelDef.css';
import Centerer from '../../../../../MiniTools/Centerer/Centerer';
import { cytoscapeArrowHeads } from '../../../../../AppConsts';
import CJSArrowShow from '../RelDefEdit/CJSArrowShow/CJSArrowShow';


export default function RelDef (props) {
    

    const relDefAttributesParse = () =>{
        var finalString = '{ ';
        for (var i=0; i<props.relDef.attributeDefinitions.length-1; i++){
            finalString+=props.relDef.attributeDefinitions[i].name+', '
        }
        finalString += props.relDef.attributeDefinitions[props.relDef.attributeDefinitions.length-1].name +' }';
        return finalString;
    }


    return (
        <div className={props.selected ? 'relDefContainer adcSelected' : 'relDefContainer adcUnselected'}
            style={props.selected ? {backgroundColor: overTheme.palette.primary.light} : {backgroundColor: overTheme.palette.secondary.dark}}>
            <div className={props.selected ? 'relDefHeadersContainer adhcSelected' : 'relDefHeadersContainer adhcUnselected'}>
                <div className='relDefShowInfo'
                onClick={()=>{
                    if (props.selected){
                        props.select(-1);
                    }else{
                        props.select(props.ind);
                    }
                }}>
                    <div className='relDefShape'>
                        <Centerer>
                            <CJSArrowShow
                                selected={true} 
                                index={props.ind+cytoscapeArrowHeads.length*2+1} 
                                arrowType={cytoscapeArrowHeads[Math.floor(props.relDef.shape/2)]} 
                                fillType={props.relDef.shape%2 ? 'hollow' : 'filled'}
                            />
                        </Centerer>
                    </div>
                    <div className='relDefName'>
                        <Centerer>
                            {props.relDef.name}
                        </Centerer>
                    </div>
                </div>
                
                <div className='relDefHeaders'>
                    <div className='relDefValuesHeader'>
                        <Centerer>
                            Attributes
                        </Centerer>
                    </div>
                    <div className='relDefDescHeader'>
                        <Centerer>
                            Description
                        </Centerer>
                    </div>
                </div>
            </div>
            <div className='relDefExtraInfo'>
                <div className='relDefAttributesInfo'>
                    <Centerer>
                        {props.relDef.attributeDefinitions.length ? relDefAttributesParse() : 'NO ATTRIBUTES'}
                    </Centerer>
                </div>
                <div className='relDefDescInfo'>
                    <Centerer>
                        {props.relDef.description=='' ? 'NO DESCRIPTION' : props.relDef.description}
                    </Centerer>
                </div>
            </div>
        </div>
    );
    
}