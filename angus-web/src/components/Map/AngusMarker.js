import React from 'react'
import {Marker} from "react-google-maps";

export default class AngusMarker extends React.Component{

  render(){
    return (
      <Marker
        position={this.props.item}
      />
    )
  }

}