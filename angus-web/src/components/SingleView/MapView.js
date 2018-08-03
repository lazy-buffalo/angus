import React from 'react';
import {translate, Trans} from 'react-i18next';
import ContentWrapper from '../Layout/ContentWrapper';
import {Row, Col, Dropdown, DropdownToggle, DropdownMenu, DropdownItem} from 'reactstrap';
import Map from "../Map/Map";
import _ from "lodash";
import AngusMarker from "../Map/AngusMarker";
import {getCows} from "../../Services/AngusAPI";

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
            <Map>
              {_.map(this.state.cows, (item, index) => <AngusMarker key={index}
                                                                    item={{lat: item.latitude, lng: item.longitude}}/>)}
            </Map>
          </Col>
        </Row>
      </ContentWrapper>
    );
  }
}

export default translate('translations')(MapView);
