import React, {Component} from 'react';
import {Marker, withGoogleMap, GoogleMap} from 'react-google-maps';

import logo from '../../styles/assets/map-marker-alt.png'

class Map extends Component {
  constructor(props) {
    super(props);
  }

  render() {
    const GoogleMapExample = withGoogleMap(props => (
      <GoogleMap
        defaultCenter={{lat: 50.601878, lng: 3.511215}}
        defaultZoom={18}
        defaultMapTypeId="satellite"
      >
        <Marker position={{lat: 50.601878, lng: 3.511215}}
        />

        {this.props.children}
        {this.props.layer &&
        (this.props.layer)
        }
      </GoogleMap>
    ));

    return (
      <GoogleMapExample
        containerElement={<div style={{height: `700px`, width: '100%'}}/>}
        mapElement={<div style={{height: `100%`}}/>}
      />
    );
  }
}

export default Map;
