import React from 'react'
import ContentWrapper from "../Layout/ContentWrapper";
import {Trans, translate} from "react-i18next";
import {Col, Dropdown, DropdownItem, DropdownMenu, DropdownToggle, Row} from "reactstrap";
import Map from "../Map/Map";

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

            </Map>
          </Col>
        </Row>
      </ContentWrapper>
    );
  }
}

export default translate('translations')(GrazingView);