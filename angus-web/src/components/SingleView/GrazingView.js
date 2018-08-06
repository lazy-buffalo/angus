import React from 'react'
import ContentWrapper from "../Layout/ContentWrapper";
import {Trans, translate} from "react-i18next";

import {DrawingManager} from "react-google-maps/lib/components/drawing/DrawingManager";
import Map from "../Map/Map";

import {postGrazing} from '../../Services/AngusAPI'

const google = window.google;

class GrazingView extends React.Component {

  constructor(props) {
    super(props);

    this.handlePolygon = this.handleNewPolygon.bind(this);
  }

  handleNewPolygon(evt) {
    const type = evt.type; // "CIRCLE", "POLYGON", etc
    if (type !== 'polygon')
      return;

    const overlay = evt.overlay; // regular Google maps API object

    // Use react-google-maps instead of the created overlay object
    if (!overlay) {
      return;
    }

    google.maps.event.clearInstanceListeners(overlay);
    overlay.setMap(null);

    // Ok, now we can handle the event in a "controlled" way

    const path = overlay.getPath();

    const coords = [];
    for (var i = 0; i < path.getLength(); i++) {
      var xy = path.getAt(i);
      coords.push({lat: xy.lat(), lng: xy.lng()});
    }

    postGrazing({
      coordinates: coords
    });

  }

  onDrawingManager = (mg) => {
    this.drawingManager = mg;
    google.maps.event.addListener(mg, 'polygoncomplete', (e) => {
      console.log(e);
    });
  };

  onMapRef = (map) => {
    this.mapRef = map;
  };

  componentWillUnmount() {
    if (this.drawingManager && this.handlePolygon)
      google.maps.event.removeListener(this.drawingManager, 'polygoncomplete', this.handlePolygon);
  }

  render() {
    return (
      <ContentWrapper>
        <div className="content-heading">
          <div>Domain de Graux
            <small><Trans i18nKey='grazing.TITLE'></Trans></small>
          </div>
        </div>
        <Map style={{height: 'calc(100vh - 195px)', margin: '-20px'}}
             mapRef={this.onMapRef.bind(this)}>
          <DrawingManager
            onOverlayComplete={this.handleNewPolygon.bind(this)}
            ref={this.onDrawingManager.bind(this)}
            defaultDrawingMode={google.maps.drawing.OverlayType.POLYGON}
            defaultOptions={{
              drawingControl: true,
              drawingMode: google.maps.drawing.OverlayType.POLYGON,
              drawingControlOptions: {
                position: google.maps.ControlPosition.TOP_CENTER,
                drawingModes: [
                  google.maps.drawing.OverlayType.POLYGON,
                ],
              },
              polygonOptions: {
                fillColor: `#ffff00`,
                fillOpacity: 1,
                strokeWeight: 5,
                clickable: false,
                editable: true,
                zIndex: 1,
              },
            }}
          />
        </Map>
      </ContentWrapper>
    );
  }
}

export default translate('translations')(GrazingView);