import React, { useEffect } from "react";
import cytoscape from "cytoscape";
import dagre from 'cytoscape-dagre';
import './CJSGraph.css';
import { ArtefactIconImages, cytoscapeArrowHeads, ArtefactIcons } from '../../AppConsts';


export default function CJSGraph(props){

  const cytoscapeLayouts = ['breadthfirst', 'dagre', 'concentric'];

  cytoscape.use(dagre);
  useEffect(()=>{
    const artefacts = props.project.artefacts.map((artefact) =>{
      return {
        group: 'nodes',
        data: {
          id: 'a'+artefact.id,
          definition: artefact.definition,
          attributes: artefact.attributes
        },
        
        selected: false, // whether the element is selected (default false)
        selectable: false, // whether the selection state is mutable (default true)
        locked: false, // when locked a node's position is immutable (default false)
        //grabbable: false, // whether the node can be grabbed and moved by the user
        pannable: false, // whether dragging the node causes panning instead of grabbing
      }
    });
    const relationships = props.project.relationships.map((relationship) =>{
      return {
        group: 'edges',
        data: {
          id: 'r'+relationship.id,
          definition: relationship.definition,
          attributes: relationship.attributes,
          source: 'a'+relationship.parent.id,
          target: 'a'+relationship.child.id
        }
      }
    });
    
    const artefactDraggingLayerStyles = artefacts.map((node)=>{
      var css = {
        'background-color': 'transparent'
      }

      
      return {
        selector: 'drag',
        style: css
      }
    });


    const artefactStyles = artefacts.map((node) =>{
      var css = {
        "background-image": ArtefactIconImages[node.data.definition.shape],
        "background-color": 'transparent'
      }
      if (props.visualTemplate){

        const colorFound = props.visualTemplate.artefactColorFactors.filter((factor) =>factor.element.elem.id == node.data.definition.id);
        colorFound.forEach((factor)=>{
          var elemValue = 'rgb(255, 255, 255)';
          var found = false;
          node.data.attributes.forEach((attribute)=>{
            if (attribute.definition.name==factor.definition.name){
              switch (attribute.definition.type){
                case 0:
                  factor.values.forEach((value) =>{
                    if (value.key == attribute.value){
                      elemValue = 'rgb('+value.value+')';
                      found = true;
                    }
                  });
                  break;
                case 1:
                  factor.values.forEach((value) =>{
                    if (value.key[0] <= attribute.value && value.key[1] >= attribute.value){
                      elemValue = 'rgb('+value.value+')';
                      found = true;
                    }
                  });
                  break;
                case 2:
                  factor.values.forEach((value) =>{
                    if (value.key == attribute.value){
                      elemValue = 'rgb('+value.value+')';
                      found = true;
                    }
                  });
                  break;
              }
            }
          });
          if (found)
            css = {...css,
              "background-color": elemValue
            }
        });


        const sizeFound = props.visualTemplate.artefactSizeFactors.filter((factor) =>factor.element.elem.id == node.data.definition.id);
        sizeFound.forEach((factor)=>{
          var elemValue = '1';
          var found = false;
          node.data.attributes.forEach((attribute)=>{
            if (attribute.definition.name==factor.definition.name){
              switch (attribute.definition.type){
                case 0:
                  factor.values.forEach((value) =>{
                    if (value.key == attribute.value){
                      elemValue = value.value;
                      found = true;
                    }
                  });
                  break;
                case 1:
                  factor.values.forEach((value) =>{
                    if (value.key[0] <= attribute.value && value.key[1] >= attribute.value){
                      elemValue = value.value;
                      found = true;
                    }
                  });
                  break;
                case 2:
                  factor.values.forEach((value) =>{
                    if (value.key == attribute.value){
                      elemValue = value.value;
                      found = true;
                    }
                  });
                  break;
              }
            }
          });
          if (found){
            elemValue=(elemValue-1)*0.8+20
            css = {...css,
              "height": elemValue+'px',
              "width": elemValue+'px'
            }
          }
        });
      }
      return {
        selector: 'node[id = \''+node.data.id+'\']',
        style: css
      }
    });
    const relationshipStyles = relationships.map((relation) =>{

      var css = {
        'width': 1,
        'line-color': 'white',
        'target-arrow-color': 'white',
        'target-arrow-shape': cytoscapeArrowHeads[Math.floor(relation.data.definition.shape/2)],
        'curve-style': 'bezier',
        'target-arrow-fill': relation.data.definition.shape%2==0 ? 'filled' : 'hollow'
      }
      if (props.visualTemplate){

        const colorFound = props.visualTemplate.relationshipColorFactors.filter((factor) =>factor.element.elem.id == relation.data.definition.id);
        colorFound.forEach((factor)=>{
          var elemValue = 'rgb(255, 255, 255)';
          var found = false;
          relation.data.attributes.forEach((attribute)=>{
            if (attribute.definition.name==factor.definition.name){
              switch (attribute.definition.type){
                case 0:
                  factor.values.forEach((value) =>{
                    if (value.key == attribute.value){
                      elemValue = 'rgb('+value.value+')';
                      found = true;
                    }
                  });
                  break;
                case 1:
                  factor.values.forEach((value) =>{
                    if (value.key[0] <= attribute.value && value.key[1] >= attribute.value){
                      elemValue = 'rgb('+value.value+')';
                      found = true;
                    }
                  });
                  break;
                case 2:
                  factor.values.forEach((value) =>{
                    if (value.key == attribute.value){
                      elemValue = 'rgb('+value.value+')';
                      found = true;
                    }
                  });
                  break;
              }
            }
          });
          if (found)
            css = {...css,
              "line-color": elemValue,
              "target-arrow-color": elemValue
            }
        });


        const sizeFound = props.visualTemplate.relationshipSizeFactors.filter((factor) =>factor.element.elem.id == relation.data.definition.id);
        sizeFound.forEach((factor)=>{
          var elemValue = '1';
          var found = false;
          relation.data.attributes.forEach((attribute)=>{
            if (attribute.definition.name==factor.definition.name){
              switch (attribute.definition.type){
                case 0:
                  factor.values.forEach((value) =>{
                    if (value.key == attribute.value){
                      elemValue = value.value;
                      found = true;
                    }
                  });
                  break;
                case 1:
                  factor.values.forEach((value) =>{
                    if (value.key[0] <= attribute.value && value.key[1] >= attribute.value){
                      elemValue = value.value;
                      found = true;
                    }
                  });
                  break;
                case 2:
                  factor.values.forEach((value) =>{
                    if (value.key == attribute.value){
                      elemValue = value.value;
                      found = true;
                    }
                  });
                  break;
              }
            }
          });
          if (found){
            elemValue=(elemValue-1)*0.5;
            css = {...css,
              "width": elemValue
            }
          }
        });
      }

      return {
        selector: 'edge[id = \''+relation.data.id+'\']',
        style: css
      }
    });

    var elems = [];
    if (artefacts){
      if (artefacts.length){
        elems = [...elems,...artefacts];
      }else{
        elems = [...elems, artefacts];
      }
    }
    if (relationships){
      if (relationships.length){
        elems = [...elems,...relationships];
      }else{
        elems = [...elems, relationships];
      }
    }

    var graph = props.visualTemplate ? props.visualTemplate.graphTemplate : 0;

    var cy = cytoscape({
      container: document.getElementById('csjGraphContainer'+props.index), // container to render in
      elements: elems,
    
      style: [ 
        ...artefactStyles, 
        //...artefactDraggingLayerStyles, add to remove cytoscape forms
        ...relationshipStyles
      ],
    
      layout: {
        name: cytoscapeLayouts[graph]
      }
    
    });


  },[props.project, props.reset, props.visualTemplate]);

    

  return (
    <div id={'csjGraphContainer'+props.index} className='csjGraphContainerFull'>
    </div>
  );
}