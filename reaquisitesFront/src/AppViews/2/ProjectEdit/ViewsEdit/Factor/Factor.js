import { overTheme } from '../../../../../overTheme';
import Centerer from '../../../../../MiniTools/Centerer/Centerer';
import './Factor.css';
import { cytoscapeArrowHeads, ArtefactIcons } from '../../../../../AppConsts';
import CJSArrowShow from '../../RelDefsEdit/RelDefEdit/CJSArrowShow/CJSArrowShow';


export default function Factor (props) {
    
    const  factorElement = () => {
        if (props.elementType>0){
            return  <>
                        <div className='relDefShape'>
                            <Centerer>
                                <CJSArrowShow
                                    selected={true} 
                                    index={'RelDefFactor'+props.ind+cytoscapeArrowHeads.length*2+1} 
                                    arrowType={cytoscapeArrowHeads[Math.floor(props.factor.elementDefinition.shape/2)]} 
                                    fillType={props.factor.elementDefinition.shape%2 ? 'hollow' : 'filled'}
                                />
                            </Centerer>
                        </div>
                        <div className='relDefName'>
                            <Centerer>
                                {props.factor.elementDefinition.name}
                            </Centerer>
                        </div>
                    </>
        }else{
            return  <>
                        <div className='artDefShape'>
                            <Centerer>
                                {ArtefactIcons[props.factor.elementDefinition.shape]}
                            </Centerer>
                        </div>
                        <div className='artDefName'>
                            <Centerer>
                                {props.factor.elementDefinition.name}
                            </Centerer>
                        </div>
                    </>
        }
    }

    return (
        <div className={props.selected ? 'factorContainer fcSelected' : 'factorContainer fcUnselected'}
            style={props.selected ? {backgroundColor: overTheme.palette.primary.light} : {backgroundColor: overTheme.palette.secondary.dark}}>
            <div className={props.selected ? 'factorHeadersContainer fhcSelected' : 'factorHeadersContainer fhcUnselected'}>
                <div className='factorShowInfo'
                onClick={()=>{
                    if (props.selected){
                        props.select(-1);
                    }else{
                        props.select(props.ind);
                    }
                }}>
                    <div className='factorType'>
                        <Centerer>
                            { factorElement()}
                        </Centerer>
                    </div>
                    <div className='factorName'>
                        <Centerer>
                            {props.type == 0 ? 'COLOR' : 'SIZE'}
                        </Centerer>
                    </div>
                </div>
                
                <div className='factorHeaders'>
                    <div className='factorValuesHeader'>
                        <Centerer>
                            Attribute
                        </Centerer>
                    </div>
                    <div className='factorDescHeader'>
                        <Centerer>
                            Values
                        </Centerer>
                    </div>
                </div>
            </div>
            <div className='factorExtraInfo'>
                <div className='factorValuesInfo'>
                    <Centerer>
                        {props.factor.attributeDefinition.name}
                    </Centerer>
                </div>
                <div className='factorDescInfo'>
                    <Centerer>
                        {props.factor.values.map((value, index) =>{
                            return  <div key={index} style={{display: 'flex', flexDirection: 'row'}}>
                                        {value.key+': '}
                                        <div style={props.type==0 ? {backgroundColor: 'rgb('+value.R+', '+value.G+', '+value.B+')'} : {}}>
                                            {props.type==0 ? value.R + ' - '+value.G+' - '+value.B : value.size}
                                        </div>
                                    </div>
                        })}
                    </Centerer>
                </div>
            </div>
        </div>
    );
    
}