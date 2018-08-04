import React from 'react';
import {translate, Trans} from 'react-i18next';
import ContentWrapper from '../Layout/ContentWrapper';
import {Dropdown, DropdownToggle, DropdownMenu, DropdownItem} from 'reactstrap';
import _ from "lodash";
import HeatmapLayer from "react-google-maps/lib/components/visualization/HeatmapLayer";

import {DateRangePicker} from 'react-dates';

import Map from "../Map/Map";
import AngusMarker from "../Map/AngusMarker";
import {getCows} from "../../Services/AngusAPI";
import moment from 'moment'
import {Button, Col, Row} from "react-bootstrap";

const google = window.google;

class MapView extends React.Component {

  constructor(props) {
    super(props);

    this.state = {
      dropdownOpen: false,
      cows: [],
      startDate: moment(new Date()),
      endDate: moment(new Date()),
    }
  }

  componentDidMount() {
    this.updateData();
  }

  updateData() {
    getCows(this.state.startDate.format('YYYY/MM/DD'), this.state.endDate.format('YYYY/MM/DD'))
      .then((response) => response.json())
      .then((data) => this.setState({
        cows: data
      }, () => {
        if (this.mapRef) {
          let markers = [];//some array
          let bounds = new google.maps.LatLngBounds();
          _.forEach(this.state.cows, (cow) => {
            _.forEach(cow.locations, (location) => {
              bounds.extend(new google.maps.LatLng(location.latitude, location.longitude));
            })
          });
          this.mapRef.fitBounds(bounds);
        }
      }))
      .catch((error) => {
        console.log(error);
      });
  }

  setPreviousRange() {
    const diff = this.state.endDate.diff(this.state.startDate, 'days');
    this.setState({
      startDate: this.state.startDate.subtract(diff + 1, 'days'),
      endDate: this.state.endDate.subtract(diff + 1, 'days')
    }, () => {
      this.updateData();
    });
  }

  setNextRange() {
    const diff = this.state.endDate.diff(this.state.startDate, 'days');
    this.setState({
      startDate: this.state.startDate.add(diff + 1, 'days'),
      endDate: this.state.endDate.add(diff + 1, 'days')
    }, () => {
      this.updateData();
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

  onMapRef = (map) => {
    this.mapRef = map;
  };

  onDatesChange = ({startDate, endDate}) => {


    this.setState({
      startDate: startDate || moment(new Date()),
      endDate: endDate || moment(new Date())
    }, () => {
      this.updateData();
    })
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
            <div style={{display: 'flex'}}>
              <DateRangePicker

                startDate={this.state.startDate} // momentPropTypes.momentObj or null,
                startDateId="your_unique_start_date_id" // PropTypes.string.isRequired,
                endDate={this.state.endDate} // momentPropTypes.momentObj or null,
                endDateId="your_unique_end_date_id" // PropTypes.string.isRequired,
                onDatesChange={this.onDatesChange} // PropTypes.func.isRequired,
                focusedInput={this.state.focusedInput} // PropTypes.oneOf([START_DATE, END_DATE]) or null,
                onFocusChange={focusedInput => this.setState({focusedInput})} // PropTypes.func.isRequired,
              />
              <div className='ml-2' style={{alignSelf: 'center'}}>Déplacer l'intervalle:</div>
              <div style={{alignSelf: 'center'}}>
                <Button bsStyle="primary" style={{margin: 6}}
                        onClick={this.setPreviousRange.bind(this)}>Précédent</Button>
                <Button bsStyle="primary" style={{margin: 6}}
                        onClick={this.setNextRange.bind(this)}>Suivant</Button>
              </div>
            </div>
          </div>
          {/* END Language list */}
        </div>
        <Map layer={this.renderHeatmap()}
             style={{height: 'calc(100vh - 210px)', margin: '-20px'}}
             mapRef={this.onMapRef.bind(this)}>
          {_.map(_.filter(this.state.cows,
            (cow) => cow.locations.length > 0),
            (item, index) =>
              <AngusMarker key={index}
                           item={{
                             lat: _.first(item.locations).latitude,
                             lng: _.first(item.locations).longitude
                           }}
                           name={item.cowName}/>)}
        </Map>
      </ContentWrapper>
    );
  }

  getData() {
    return _.flatMapDeep(this.state.cows, (item, index) => {

      return _.map(item.locations, (location) => {
        return {
          location: new google.maps.LatLng(location.latitude, location.longitude),
          weight: 1
        }
      })
    });
  }

  renderHeatmap() {
    return <HeatmapLayer options={{radius: 40}} data={_.flatMapDeep(this.state.cows, (cow, index) => {

        return _.map(cow.locations, (location) => {
          return {
            location: new google.maps.LatLng(location.latitude, location.longitude),
            weight: 1
          }
        })

      }
    )}/>
  }
}

export default translate('translations')(MapView);
