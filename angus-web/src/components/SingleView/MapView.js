import React from 'react';
import {translate, Trans} from 'react-i18next';
import ContentWrapper from '../Layout/ContentWrapper';
import {Row, Col, Dropdown, DropdownToggle, DropdownMenu, DropdownItem} from 'reactstrap';
import _ from "lodash";
import HeatmapLayer from "react-google-maps/lib/components/visualization/HeatmapLayer";

import Map from "../Map/Map";
import AngusMarker from "../Map/AngusMarker";
import {getCows} from "../../Services/AngusAPI";

const google = window.google;

class MapView extends React.Component {

  constructor(props) {
    super(props);

    this.state = {
      dropdownOpen: false,
      cows: []
    }
  }

  componentDidMount() {
    getCows()
      .then((response) => response.json())
      .then((data) => this.setState({
        cows: data
      }))
      .catch((error) => {
        console.log(error);
      });
  }


  changeLanguage = lng => {
    this.props.i18n.changeLanguage(lng);
  };

  toggle = () => {
    this.setState({
      dropdownOpen: !this.state.dropdownOpen
    });
  };

  render() {
    return (
      <ContentWrapper>
        <div className="content-heading">
          <div>Domain de Graux
            <small><Trans i18nKey='dashboard.WELCOME'></Trans></small>
          </div>
          {/* START Language list */}
          <div className="ml-auto">
            <Dropdown isOpen={this.state.dropdownOpen} toggle={this.toggle}>
              <DropdownToggle>
                English
              </DropdownToggle>
              <DropdownMenu className="dropdown-menu-right-forced animated fadeInUpShort">
                <DropdownItem onClick={() => this.changeLanguage('en')}>English</DropdownItem>
                <DropdownItem onClick={() => this.changeLanguage('es')}>Spanish</DropdownItem>
              </DropdownMenu>
            </Dropdown>
          </div>
          {/* END Language list */}
        </div>
        <Row>
          <Col xs={12} className="text-center">
            <Map layer={this.renderHeatmap()} style={{height: 'calc(100vh - 200px)'}}>
              {_.map(this.state.cows, (item, index) => <AngusMarker key={index}
                                                                    item={{lat: item.location.latitude, lng: item.location.longitude}}
                                                                    name={item.cowName}/>)}
            </Map>
          </Col>
        </Row>
      </ContentWrapper>
    );
  }

  renderHeatmap() {
    return <HeatmapLayer options={{radius: 100}} data={_.map(this.state.cows, (item, index) => {
        return {
          location: new google.maps.LatLng(item.location.latitude, item.location.longitude),
          weight: 1
        }
      }
    )}/>
  }
}

export default translate('translations')(MapView);
