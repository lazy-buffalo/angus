import React, {Component} from 'react';
import {withGoogleMap, GoogleMap} from 'react-google-maps';

const MapRendering = withGoogleMap((props) => <GoogleMap
  defaultCenter={{lat: 50.601878, lng: 3.511215}}
  defaultZoom={18}
  defaultMapTypeId="satellite"
>
  {props.children}
</GoogleMap>);

class Map extends Component {
  constructor(props) {
    super(props);
  }

  render() {
    return <MapRendering containerElement={<div style={this.props.style}/>}
                         mapElement={<div style={{height: `100%`}}/>}>
      {this.props.children}
      {this.props.layer &&
      (this.props.layer)}
    </MapRendering>;
  }
}

export default Map;
