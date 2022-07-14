import { PinDropSharp } from '@mui/icons-material';
import './ArtefactElement.css';



export default function ArtefactElement(props) {


  return (
    <div className='artefactElemContainer' 
    onClick={() => props.focusOn(props.artefact.id)}
    style={props.selected ? {backgroundColor: 'rgba(255,255,255, 0.25)'} : {backgroundColor: 'transparent'}}
    >
      {props.artefact.name}
    </div>
  );
}
