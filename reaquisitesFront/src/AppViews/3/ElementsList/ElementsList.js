import ArtefactElement from './ArtefactElement/ArtefactElement';
import './ElementsList.css';



export default function ElementsList(props) {


  return (
    <div className='elementListContainer'>
      <div>

      </div>
      <div className='elementList'>
        {props.artefacts && props.artefacts.map((artefact,index) =>{
          return  <ArtefactElement
                    key={index} 
                    artefact={artefact} 
                    selected={ props.selectedArtefact == artefact.id} 
                    ind={index}
                    focusOn={props.focusOnArtefact}
                  />
        })}
      </div>
    </div>
  );
}
