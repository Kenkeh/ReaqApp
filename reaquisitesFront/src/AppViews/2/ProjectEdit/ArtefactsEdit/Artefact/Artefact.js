import { overTheme } from '../../../../../overTheme';
import './Artefact.css';
import Centerer from '../../../../../MiniTools/Centerer/Centerer';
import { ArtefactIcons } from '../../../../../AppConsts';


export default function Artefact (props) {
    

    const artefactAttributeValuesParse = () =>{
        var finalString = '{ ';
        for (var i=0; i<props.artefact.attributeDefinitions.length-1; i++){
            finalString+=props.artefact.attributeDefinitions[i].name+', '
        }
        finalString += props.artefact.attributeDefinitions[props.artefact.attributeDefinitions.length-1].name +' }';
        return finalString;
    }


    return (
        <div className={props.selected ? 'artefactContainer adcSelected' : 'artefactContainer adcUnselected'}
            style={props.selected ? {backgroundColor: overTheme.palette.primary.light} : {backgroundColor: overTheme.palette.secondary.dark}}>
            <div className={props.selected ? 'artefactHeadersContainer adhcSelected' : 'artefactHeadersContainer adhcUnselected'}>
                <div className='artefactShowInfo'
                onClick={()=>{
                    if (props.selected){
                        props.select(-1);
                    }else{
                        props.select(props.ind);
                    }
                }}>
                    <div className='artefactShape'>
                        <Centerer>
                            {ArtefactIcons[props.artefact.shape]}
                        </Centerer>
                    </div>
                    <div className='artefactName'>
                        <Centerer>
                            {props.artefact.name}
                        </Centerer>
                    </div>
                </div>
                
                <div className='artefactHeaders'>
                    <div className='artefactValuesHeader'>
                        <Centerer>
                            Attributes
                        </Centerer>
                    </div>
                    <div className='artefactDescHeader'>
                        <Centerer>
                            Description
                        </Centerer>
                    </div>
                </div>
            </div>
            <div className='artefactExtraInfo'>
                <div className='artefactAttributesInfo'>
                    <Centerer>
                        {props.artefact.attributeDefinitions.length ? artefactAttributeValuesParse() : 'NO ATTRIBUTES'}
                    </Centerer>
                </div>
                <div className='artefactDescInfo'>
                    <Centerer>
                        {props.artefact.description=='' ? 'NO DESCRIPTION' : props.artefact.description}
                    </Centerer>
                </div>
            </div>
        </div>
    );
    
}