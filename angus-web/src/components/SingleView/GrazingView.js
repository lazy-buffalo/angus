import React from 'react'
import ContentWrapper from "../Layout/ContentWrapper";
import {Trans, translate} from "react-i18next";
import {Col, Row} from "reactstrap";

import { DrawingManager } from "react-google-maps/lib/components/drawing/DrawingManager";
import Map from "../Map/Map";

const google = window.google;

class GrazingView extends React.Component {

  render() {
    return (
      <ContentWrapper>
        <div className="content-heading">
          <div>Domain de Graux
            <small><Trans i18nKey='grazing.TITLE'></Trans></small>
          </div>
        </div>
        <Row>
          <Col xs={12} className="text-center">
            <Map>
              <DrawingManager
                defaultDrawingMode={google.maps.drawing.OverlayType.POLYGON}
                defaultOptions={{
                  drawingControl: true,
                  drawingMode: google.maps.drawing.OverlayType.MARKER,
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
          </Col>
        </Row>
      </ContentWrapper>
    );
  }
}

export default translate('translations')(GrazingView);