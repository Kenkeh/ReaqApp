import { PinDropSharp } from '@mui/icons-material';
import { useState } from 'react';
import ElementsList from './ElementsList/ElementsList';
import GraphVisualizer from './GraphVisualizer/GraphVisualizer';
import './ProjectVisualization.css';
import VisualizationManage from './VisualizationManage/VisualizationManage';



export default function ProjectVisualization(props) {

  const [focusedArtefact, setFocusedArtefact] = useState(-1);


  return (
    <div className='projectVisualizationContainer'>
      <VisualizationManage/>
      <GraphVisualizer 
        project={props.project}
        focusedArtefact={focusedArtefact}
      />
      <ElementsList
        artefacts={props.project ? props.project.artefacts : undefined}
        focusOnArtefact={(foc) => setFocusedArtefact(foc)}
        selectedArtefact={focusedArtefact}
      />
    </div>
  );
}
