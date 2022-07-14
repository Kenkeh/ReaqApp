import './GraphVisualizer.css';
import React, { useEffect, useState } from "react";
import cytoscape from "cytoscape";
import dagre from 'cytoscape-dagre';
import { ArtefactIconImages, cytoscapeArrowHeads } from '../../../AppConsts';


export default function GraphVisualizer(props) {
  const cytoscapeLayouts = ['dagre', 'breadthfirst', 'concentric'];

  const [cjsGraph, setCJSGraph] = useState(null); // container to render in

  const [cjsGraphFocus, setCJSGraphFocus] = useState(-1);


  cytoscape.use(dagre);
  useEffect(()=>{
    if (!props.project) return;
    var newCjsGraph;
    if (cjsGraph == null) newCjsGraph = cytoscape({container: document.getElementById('csjGraphContainerVisualizer')});
    else newCjsGraph = cjsGraph;
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

        const colorFound = props.visualTemplate.artefactColorFactors.filter((factor) =>factor.elementDefinition.id == node.data.definition.id);
        colorFound.forEach((factor)=>{
          var elemValue = 'rgb(255, 255, 255)';
          var found = false;
          node.data.attributes.forEach((attribute)=>{
            if (attribute.definition.name==factor.attributeDefinition.name){
              switch (attribute.definition.type){
                case 0:
                  factor.values.forEach((value) =>{
                    if (value.key == attribute.value){
                      elemValue = 'rgb('+value.r+', '+value.g+', '+value.b+')';
                      found = true;
                    }
                  });
                  break;
                case 1:
                  factor.values.forEach((value) =>{
                    if (value.key[0] <= attribute.value && value.key[1] >= attribute.value){
                      elemValue = 'rgb('+value.r+', '+value.g+', '+value.b+')';
                      found = true;
                    }
                  });
                  break;
                case 2:
                  factor.values.forEach((value) =>{
                    if (value.key == attribute.value){
                      elemValue = 'rgb('+value.r+', '+value.g+', '+value.b+')';
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


        const sizeFound = props.visualTemplate.artefactSizeFactors.filter((factor) =>factor.elementDefinition.id == node.data.definition.id);
        sizeFound.forEach((factor)=>{
          var elemValue = '1';
          var found = false;
          node.data.attributes.forEach((attribute)=>{
            if (attribute.definition.name==factor.attributeDefinition.name){
              switch (attribute.definition.type){
                case 0:
                  factor.values.forEach((value) =>{
                    if (value.key == attribute.value){
                      elemValue = value.size;
                      found = true;
                    }
                  });
                  break;
                case 1:
                  factor.values.forEach((value) =>{
                    if (value.key[0] <= attribute.value && value.key[1] >= attribute.value){
                      elemValue = value.size;
                      found = true;
                    }
                  });
                  break;
                case 2:
                  factor.values.forEach((value) =>{
                    if (value.key == attribute.value){
                      elemValue = value.size;
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

        const colorFound = props.visualTemplate.relationshipColorFactors.filter((factor) =>factor.elementDefinition.id == relation.data.definition.id);
        colorFound.forEach((factor)=>{
          var elemValue = 'rgb(255, 255, 255)';
          var found = false;
          relation.data.attributes.forEach((attribute)=>{
            if (attribute.definition.name==factor.attributeDefinition.name){
              switch (attribute.definition.type){
                case 0:
                  factor.values.forEach((value) =>{
                    if (value.key == attribute.value){
                      elemValue = 'rgb('+value.r+', '+value.g+', '+value.b+')';
                      found = true;
                    }
                  });
                  break;
                case 1:
                  factor.values.forEach((value) =>{
                    if (value.key[0] <= attribute.value && value.key[1] >= attribute.value){
                      elemValue = 'rgb('+value.r+', '+value.g+', '+value.b+')';
                      found = true;
                    }
                  });
                  break;
                case 2:
                  factor.values.forEach((value) =>{
                    if (value.key == attribute.value){
                      elemValue = 'rgb('+value.r+', '+value.g+', '+value.b+')';
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


        const sizeFound = props.visualTemplate.relationshipSizeFactors.filter((factor) =>factor.elementDefinition.id == relation.data.definition.id);
        sizeFound.forEach((factor)=>{
          var elemValue = '1';
          var found = false;
          relation.data.attributes.forEach((attribute)=>{
            if (attribute.definition.name==factor.attributeDefinition.name){
              switch (attribute.definition.type){
                case 0:
                  factor.values.forEach((value) =>{
                    if (value.key == attribute.value){
                      elemValue = value.size;
                      found = true;
                    }
                  });
                  break;
                case 1:
                  factor.values.forEach((value) =>{
                    if (value.key[0] <= attribute.value && value.key[1] >= attribute.value){
                      elemValue = value.size;
                      found = true;
                    }
                  });
                  break;
                case 2:
                  factor.values.forEach((value) =>{
                    if (value.key == attribute.value){
                      elemValue = value.size;
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

    newCjsGraph.add(elems);
    newCjsGraph.style([...artefactStyles, ...relationshipStyles]);
    newCjsGraph.layout({name: cytoscapeLayouts[graph]}).run();

    setCJSGraph(newCjsGraph);

    
    /*cjsGraph = cytoscape({
      container: document.getElementById('csjGraphContainerVisualizer'), // container to render in
      elements: elems,
    
      style: [ 
        ...artefactStyles, 
        //...artefactDraggingLayerStyles, add to remove cytoscape forms
        ...relationshipStyles
      ],
    
      layout: {
        name: cytoscapeLayouts[graph]
      }
    
    });*/


  },[props.project, props.reset, props.visualTemplate,props.focusedArtefact]);

  useEffect(()=>{
    if (cjsGraph!=null){
      const allEles = cjsGraph.elements('node');
      if (props.focusedArtefact>=0){
        const newElem = cjsGraph.$id('a'+props.focusedArtefact);
        const goInNewAnimation = cjsGraph.animation({
          fit:{
            eles: newElem
          },
          center:{
            eles: newElem
          },
          duration: 1000,
          easing: 'ease-in',
          queue: true
        });
        if (cjsGraphFocus>=0){
          const oldElem = cjsGraph.$id('a'+cjsGraphFocus);
          const goInOldAnimation = cjsGraph.animation({
            fit:{
              eles: oldElem
            },
            center:{
              eles: oldElem
            },
            duration: 1000,
            easing: 'ease-out',
            queue: true
          });
          goInOldAnimation.reverse().play().promise('complete').then(()=> goInNewAnimation.play());
          //const goOutOldAnimation = goInOldAnimation.reverse();
          //cjsGraph.animate(goOutOldAnimation);
          //cjsGraph.animate(goInNewAnimation);
          /*cjsGraph.animate({
            center:{
              eles: oldElem
            },
            zoom: 10,
            duration: 1000,
            queue: true
          });
          cjsGraph.animate({
            fit:{
              eles: newElem
            },
            center:{
              eles: newElem
            },
            duration: 1000,
            queue: true
          });
          */
        }else{
          cjsGraph.animate(goInNewAnimation);
          /*cjsGraph.animate({
            fit:{
              eles: newElem
            },
            center:{
              eles: newElem
            },
            duration: 1000,
            queue: true
          });
          */
        }
      }else{
        cjsGraph.animate({
          fit:{
            eles: allEles
          },
          duration: 1000,
          queue: true
        });
      }
      setCJSGraphFocus(props.focusedArtefact);
    }
  }, [props.focusedArtefact]);

    

  return (
    <div id='csjGraphContainerVisualizer' className='csjGraphContainerVisualizer'>
    </div>
  );
}
