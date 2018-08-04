import React from 'react'
import {Marker} from "react-google-maps";


import logo from "../../styles/assets/map-marker.png";

export default class AngusMarker extends React.Component {

  render() {
    return (
      <Marker icon={logo}
              position={this.props.item}/>
    )
  }

}