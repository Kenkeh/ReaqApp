import React, { useEffect } from "react";
import cytoscape from "cytoscape";
import './CJSArrowShow.css';


export default function CJSArrowShow(props){
    useEffect(()=>{
      var cy = cytoscape({
        container: document.getElementById('csjGraphContainer'+props.index), // container to render in
        elements: [ // list of graph elements to start with
          { // node a
            group: 'nodes',
            data: { id: 'a' },
            position: {
              x: 0, y:0
            },
            selected: false, // whether the element is selected (default false)
            selectable: false, // whether the selection state is mutable (default true)
            locked: false, // when locked a node's position is immutable (default false)
            grabbable: false, // whether the node can be grabbed and moved by the user
            pannable: false, // whether dragging the node causes panning instead of grabbing
          },
          { // node b
            group: 'nodes',
            data: { id: 'b' },
            position: {
              x: 500, y:0
            },
            selected: false, // whether the element is selected (default false)
            selectable: false, // whether the selection state is mutable (default true)
            locked: false, // when locked a node's position is immutable (default false)
            grabbable: false, // whether the node can be grabbed and moved by the user
            pannable: false, // whether dragging the node causes panning instead of grabbing
          },
          { // edge ab
            data: { id: 'ab', source: 'a', target: 'b' }
          }
        ],
      
        style: [ // the stylesheet for the graph
          {
            selector: 'node',
            style: {
              'background-color': '#666',
              'height': '10px',
              'width': '10px'
            }
          },
      
          {
            selector: 'edge',
            style: {
              'width': 5,
              'line-color': '#FFFFFF',
              'target-arrow-color': '#FFFFFF',
              'target-arrow-shape': props.arrowType,
              'curve-style': 'bezier',
              'target-arrow-fill': props.fillType
            }
          }
        ],
      
        layout: {
          name: 'grid',
          rows: 1
        },
        panningEnabled:false,
        autoungrabify: true
      
      });
    },[props.selected,props.index]);

    

    return (
      <div id={'csjGraphContainer'+props.index} className={props.selected ? 'csjGraphContainer' : 'csjGraphContainer csjGcBlack'}>
      </div>
    );
}