import ArtefactElement from './ArtefactElement/ArtefactElement';
import './ElementsList.css';
import Artefact from '../../2/ProjectEdit/ArtefactsEdit/Artefact/Artefact'



export default function ElementsList(props) {


  return (
    <div className='elementListContainer'>
      <div>

      </div>
      <div className='elementList'>
        {props.artefacts && props.artefacts.map((artefact,index) =>{
          return  <Artefact 
                    key={index} 
                    artefact={artefact} 
                    selected={ props.selectedArtefact == artefact.id} 
                    ind={index}
                    select={props.focusOnArtefact}
                  />
        })}
      </div>
    </div>
  );
}
