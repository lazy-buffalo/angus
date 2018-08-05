import React from 'react';
import {translate, Trans} from 'react-i18next';
import ContentWrapper from '../Layout/ContentWrapper';
import {Dropdown, DropdownToggle, DropdownMenu, DropdownItem} from 'reactstrap';
import _ from "lodash";
import HeatmapLayer from "react-google-maps/lib/components/visualization/HeatmapLayer";
import {Polygon} from "react-google-maps";

import {DateRangePicker} from 'react-dates';

import Map from "../Map/Map";
import AngusMarker from "../Map/AngusMarker";
import {getCows, getGrazing} from "../../Services/AngusAPI";
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
    this.updateGrazing();
  }

  updateGrazing() {
    getGrazing()
      .then((response) => response.json())
      .then((data) => this.setState({
        grazing: data
      }));
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
        <Map layer={this.renderHeatmap(this.getStrangeCows(false))}
             badLayer={this.renderHeatmap(this.getStrangeCows(true), 'blue')}
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
          {_.map(this.state.grazing, (grazing, index) => {
            const coordinates = _.map(grazing.coordinates, (latlng) => {
              return {lat: latlng.lat, lng: latlng.lng}
            });
            return <Polygon key={index} path={coordinates} options={{
              fillColor: `#ffff00`,
              fillOpacity: 0.3,
              strokeWeight: 5,
              clickable: false
            }}/>;
          })}
        </Map>
      </ContentWrapper>
    );
  }

  getStrangeCows(strange) {
    return _.filter(this.state.cows, (cow) => cow.hasStrangeLocation === strange);
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

  renderHeatmap(cows, gradient) {

    const options = {
      radius: 40
    };

    if (gradient === 'blue') {
      options.gradient = [
        'rgba(0, 255, 255, 0)',
        'rgba(0, 255, 255, 1)',
        'rgba(0, 191, 255, 1)',
        'rgba(0, 127, 255, 1)',
        'rgba(0, 63, 255, 1)',
        'rgba(0, 0, 255, 1)',
        'rgba(0, 0, 223, 1)',
        'rgba(0, 0, 191, 1)',
        'rgba(0, 0, 159, 1)',
        'rgba(0, 0, 127, 1)',
        'rgba(63, 0, 91, 1)',
        'rgba(127, 0, 63, 1)',
        'rgba(191, 0, 31, 1)',
        'rgba(255, 0, 0, 1)'
      ];
    }

    return <HeatmapLayer options={options} data={_.flatMapDeep(cows, (cow, index) => {

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

export default translate(
  'translations'
)(
  MapView
)
;
