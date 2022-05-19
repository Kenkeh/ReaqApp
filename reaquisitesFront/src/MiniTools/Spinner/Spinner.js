import './Spinner.css';
import React from 'react';



export default class Spinner extends React.Component {
  render(){
    if (this.props.left){
      return (
        <div className="spinner" style={{animationDuration: this.props.msCycle, animationDirection: "reverse"}}>{this.props.children}</div>
      );
    }else{
      return (
        <div className="spinner" style={{animationDuration: this.props.msCycle}}>{this.props.children}</div>
      );
    }
  }
};
