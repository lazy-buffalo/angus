import React, {Component} from 'react';
import {withGoogleMap, GoogleMap} from 'react-google-maps';

const MapRendering = withGoogleMap((props) => {

  const onRef = (e) => {
    if (props.ref) {
      props.ref(e);
    }
  };

  return (<GoogleMap
    defaultCenter={{lat: 50.601878, lng: 3.511215}}
    defaultZoom={18}
    defaultMapTypeId="satellite"
    ref={props.onMapMounted}
  >
    {props.children}
  </GoogleMap>)
});

class Map extends Component {
  constructor(props) {
    super(props);
  }

  handleMapMounted = (map) => {
    this._map = map;
    this.props.mapRef(map);
  };

  render() {
    return <MapRendering containerElement={<div style={this.props.style}/>}
                         mapElement={<div style={{height: `100%`}}/>}
                         onMapMounted={this.handleMapMounted}>
      {this.props.children}
      {this.props.layer && this.props.layer}
      {this.props.badLayer && this.props.badLayer}
    </MapRendering>;
  }
}

export default Map;
