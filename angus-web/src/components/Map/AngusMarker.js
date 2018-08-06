import React from 'react'
import {Marker} from "react-google-maps";


import logo from "../../styles/assets/map-marker.png";
import logoWarning from '../../styles/assets/marker_warning.png'

export default class AngusMarker extends React.Component {

  render() {
    return (
      <Marker icon={this.props.isStrange ? logoWarning : logo}
              position={this.props.item}/>
    )
  }

}