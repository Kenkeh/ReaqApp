import { overTheme } from '../../../../../overTheme';
import './Relationship.css';
import Centerer from '../../../../../MiniTools/Centerer/Centerer';
import { ArtefactIcons } from '../../../../../AppConsts';
import CJSArrowShow from '../../RelDefsEdit/RelDefEdit/CJSArrowShow/CJSArrowShow';
import { cytoscapeArrowHeads } from '../../../../../AppConsts';

export default function Relationship (props) {
    

    const relationshipAttributeValuesParse = () =>{
        var finalString = '{ ';
        for (var i=0; i<props.relation.definition.attributeDefinitions.length-1; i++){
            finalString+=props.relation.attributes[i].definition.name+': '+props.relation.attributes[i].value+',\n'
        }
        finalString += props.relation.attributes[i].definition.name+': '+props.relation.attributes[i].value +' }';
        return finalString;
    }


    return (
        <div className={props.selected ? 'relationshipContainer acSelected' : 'relationshipContainer acUnselected'}
            style={props.selected ? {backgroundColor: overTheme.palette.primary.light} : {backgroundColor: overTheme.palette.secondary.dark}}>
            <div className={props.selected ? 'relationshipHeadersContainer adhcSelected' : 'relationshipHeadersContainer adhcUnselected'}>
                <div className='relationshipShowInfo'
                onClick={()=>{
                    if (props.selected){
                        props.select(-1);
                    }else{
                        props.select(props.ind);
                    }
                }}>
                    <div className='relationshipDefinition'>
                        <CJSArrowShow 
                            selected={true} 
                            index={props.ind} 
                            arrowType={cytoscapeArrowHeads[Math.floor(props.relation.definition.shape/2)]} 
                            fillType={props.relation.definition.shape%2==0 ? 'filled' : 'hollow'}
                        />
                        <Centerer>
                            {props.relation.definition.name}
                        </Centerer>
                    </div>
                    <div className='relationshipImplicates'>
                        <div className='relationshipArtefact'>
                            <div className='relationshipArtefactDefinition'>
                                <Centerer>
                                    {ArtefactIcons[props.relation.parent.definition.shape]}
                                </Centerer>
                                <Centerer>
                                    {props.relation.parent.definition.name}
                                </Centerer>
                            </div>
                            <div className='relationshipArtefactName'>
                                <Centerer>
                                    {props.relation.parent.name}
                                </Centerer>
                            </div>
                        </div>
                        <div className='relationshipArtefact'>
                            <div className='relationshipArtefactDefinition'>
                                <Centerer>
                                    {ArtefactIcons[props.relation.child.definition.shape]}
                                </Centerer>
                                <Centerer>
                                    {props.relation.child.definition.name}
                                </Centerer>
                            </div>
                            <div className='relationshipArtefactName'>
                                <Centerer>
                                    {props.relation.child.name}
                                </Centerer>
                            </div>
                        </div>
                    </div>
                </div>
                
                <div className='relationshipHeaders'>
                    <div className='relationshipValuesHeader'>
                        <Centerer>
                            Attributes
                        </Centerer>
                    </div>
                    <div className='relationshipDescHeader'>
                        <Centerer>
                            Description
                        </Centerer>
                    </div>
                </div>
            </div>
            <div className='relationshipExtraInfo'>
                <div className='relationshipAttributesInfo'>
                    <Centerer>
                        {props.relation.attributes.length ? relationshipAttributeValuesParse() : 'NO ATTRIBUTES'}
                    </Centerer>
                </div>
                <div className='relationshipDescInfo'>
                    <Centerer>
                        {props.relation.description=='' ? 'NO DESCRIPTION' : props.relation.description}
                    </Centerer>
                </div>
            </div>
        </div>
    );
    
}