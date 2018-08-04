import React from 'react'
import logo from "../../styles/assets/map-marker.png";
import {Marker} from "react-google-maps";

export default class AngusMarker extends React.Component {

  render() {
    return (
      <Marker icon={logo}
              position={this.props.item}/>
    )
  }

}