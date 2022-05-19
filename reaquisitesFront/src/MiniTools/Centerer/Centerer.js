import './Centerer.css';
import React from 'react';



export default class Centerer extends React.Component {
  render(){
    return (
      <div id={this.props.id} className="centerer_container" style={this.props.style}>
        {this.props.children}
      </div>
    );
  }
};
