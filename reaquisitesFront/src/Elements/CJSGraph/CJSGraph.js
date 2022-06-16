import React, { useEffect } from "react";
import cytoscape from "cytoscape";
import './CJSGraph.css';


export default function CJSGraph(props){
    useEffect(()=>{
      var cy = cytoscape({
        container: document.getElementById('csjGraphContainer'+props.index), // container to render in
        elements: [ // list of graph elements to start with
          { // node a
            data: { id: 'a' }
          },
          { // node b
            data: { id: 'b' }
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
              'label': 'data(id)'
            }
          },
      
          {
            selector: 'edge',
            style: {
              'width': 3,
              'line-color': '#ccc',
              'target-arrow-color': '#ccc',
              'target-arrow-shape': 'triangle',
              'curve-style': 'bezier'
            }
          }
        ],
      
        layout: {
          name: 'grid',
          rows: 1
        }
      
      });
    },[]);

    

    return (
      <div id={'csjGraphContainer'+props.index} className='csjGraphContainer'>
      </div>
    );
}