import { overTheme } from '../../../../overTheme';
import './ArtDef.css';
import Centerer from '../../../../MiniTools/Centerer/Centerer';
import { ArtefactIcons } from '../../../../AppConsts';


export default function ArtDef (props) {
    

    const artDefAttributesParse = () =>{
        var finalString = '{ ';
        for (var i=0; i<props.artDef.attributeDefinitions.length-1; i++){
            finalString+=props.artDef.attributeDefinitions[i].name+', '
        }
        finalString += props.artDef.attributeDefinitions[props.artDef.attributeDefinitions.length-1].name +' }';
        return finalString;
    }


    return (
        <div className={props.selected ? 'artDefContainer adcSelected' : 'artDefContainer adcUnselected'}
            style={props.selected ? {backgroundColor: overTheme.palette.primary.light} : {backgroundColor: overTheme.palette.secondary.dark}}>
            <div className={props.selected ? 'artDefHeadersContainer adhcSelected' : 'artDefHeadersContainer adhcUnselected'}>
                <div className='artDefShowInfo'
                onClick={()=>{
                    if (props.selected){
                        props.select(-1);
                    }else{
                        props.select(props.ind);
                    }
                }}>
                    <div className='artDefShape'>
                        <Centerer>
                            {ArtefactIcons[props.artDef.shape]}
                        </Centerer>
                    </div>
                    <div className='artDefName'>
                        <Centerer>
                            {props.artDef.name}
                        </Centerer>
                    </div>
                </div>
                
                <div className='artDefHeaders'>
                    <div className='artDefValuesHeader'>
                        <Centerer>
                            Attributes
                        </Centerer>
                    </div>
                    <div className='artDefDescHeader'>
                        <Centerer>
                            Description
                        </Centerer>
                    </div>
                </div>
            </div>
            <div className='artDefExtraInfo'>
                <div className='artDefAttributesInfo'>
                    <Centerer>
                        {props.artDef.attributeDefinitions.length ? artDefAttributesParse : 'NO ATTRIBUTES'}
                    </Centerer>
                </div>
                <div className='artDefDescInfo'>
                    <Centerer>
                        {props.artDef.description=='' ? 'NO DESCRIPTION' : props.artDef.description}
                    </Centerer>
                </div>
            </div>
        </div>
    );
    
}