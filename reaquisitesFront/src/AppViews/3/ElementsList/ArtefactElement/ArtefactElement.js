import { PinDropSharp } from '@mui/icons-material';
import './ArtefactElement.css';



export default function ArtefactElement(props) {


  return (
    <div className='artefactElemContainer' onClick={() => props.focusOn(props.artefact.id)}>
      {props.artefact.name}
    </div>
  );
}
