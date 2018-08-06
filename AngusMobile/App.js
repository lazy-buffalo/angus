/**
 * Sample React Native App
 * https://github.com/facebook/react-native
 *
 * @format
 * @flow
 */

import React, {Component} from 'react';
import {Button, Platform, StyleSheet, Text, View, DrawerLayoutAndroid, TouchableHighlight, SideBarMenu, ListView, YellowBox} from 'react-native';
import Row from './components/row';
import MapView, {Marker, PROVIDER_GOOGLE } from  'react-native-maps';
import {createStackNavigator} from 'react-navigation';
import markerIcon from './marker.png';
import markerWarningIcon from './marker_warning.png';
import {getCows, getGrazing} from "./services/angusAPI";
import CalendarPicker from 'react-native-calendar-picker';
import Moment from 'moment';
import _ from "lodash";

YellowBox.ignoreWarnings(['Warning: isMounted(...) is deprecated', 'Module RCTImageLoader']);

const instructions = Platform.select({
  ios: 'Cmd+R to reload,\n' + 'Cmd+D or shake for dev menu',
  android:
    'Shake or press menu button for dev menu',
});

const MIN_ADULT_TEMPERATURE = 30;
const MAX_ADULT_TEMPERATURE = 39;

type Props = {};

//App
class NavigationBarBackground extends React.Component {
  render() {
    return (
      <View>
        <Text>Hello</Text>
        <Button
          onPress={() => alert('This is a button!')}
          title="Info"
          color="#000"/>
      </View>
    );
  }
}

// headerTitle: <NavigationBarBackground />
class MapScreen extends React.Component {
  static navigationOptions = {
    header: null
  };

  constructor() {
      super();
      const ds = new ListView.DataSource({rowHasChanged: (r1, r2) => r1 !== r2});
      this.state = {
          dataSource:ds.cloneWithRows([{first:"first", last:"last"}, {first:"first", last:"last"}]),
          cows:[],
          grazings:[],
          selectedStartDate: new Date(),
          selectedEndDate: new Date()
      }
      this.openDrawer = this.openDrawer.bind(this);
      this.onDateChange = this.onDateChange.bind(this);
  }

  onDateChange(date, type) {
      if (type === 'END_DATE') {
        this.setState({
          selectedEndDate: date,
        }, () => {
          this.updateCows(); // get cows data
        });
      } else {
        this.setState({
          selectedStartDate: date,
          selectedEndDate: date,
        }, () => {
          this.updateCows(); // get cows data
        });
      }
    }

  componentDidMount() {
    this.updateCows();
    this.updateGrazing();
  }

  updateCows() {
    getCows(Moment(this.state.selectedStartDate).format('YYYY/MM/DD'), Moment(this.state.selectedEndDate).format('YYYY/MM/DD'))
        .then((response) => response.json())
        .then((data) => this.setState({
          cows: data
        }))
        .catch((error) => {
          console.log(error);
        });
  }

  updateGrazing() {
    getGrazing()
        .then((response) => response.json())
        .then((data) => this.setState({
          grazings: data
        }))
        .catch((error) => {
          console.log(error);
        });
  }


  openDrawer() {
      this.drawer.openDrawer();
  }

  getData(strange) {
    const cows = _.filter(this.state.cows, (cow) => cow.hasStrangeLocation === strange);
    return _.flatMapDeep(cows, (item, index) => {
      return _.map(item.locations, (location) => {
        return {
          latitude: location.latitude,
          longitude: location.longitude,
          weight: 1
        }
      })
    });
  }

  getGrazings() {
    return  _.map(this.state.grazings, (grazing) => {
        return {
          id: grazing.id,
          coordinates:_.map(grazing.coordinates, (coord) => {
            return {
              latitude: coord.lat,
              longitude: coord.lng
            }
        })}
      })
  }

  // <ListView
  //   dataSource={this.state.dataSource}
  //   renderRow={(data) => <Row name={data} />}/>
  render() {
    const { selectedStartDate, selectedEndDate } = this.state;
    const maxDate = new Date(); // today
    const startDate = selectedStartDate ? selectedStartDate.toString() : '';
    const endDate = selectedEndDate ? selectedEndDate.toString() : '';
    const cows = this.getData(false);
    const strangeCows = this.getData(true);
    const grazings = this.getGrazings();

    var navigationView = (
      <View style={styles.container}>
       <CalendarPicker
         width={300}
         maxDate={maxDate}
         weekdays={['Lun', 'Mar', 'Mer', 'Jeu', 'Ven', 'Sam', 'Dim']}
         months={['Janvier', 'Février', 'Mars', 'Avril', 'Mai', 'Juin', 'Juillet', 'Août', 'Septembre', 'Octobre', 'Novembre', 'Décembre']}
         previousTitle={'Précédent'}
         nextTitle={'Suivant'}
         startFromMonday={true}
         allowRangeSelection={true}
         onDateChange={this.onDateChange}/>
     </View>
    );
    return (
      <DrawerLayoutAndroid
        drawerWidth={300}
        ref={_drawer => (this.drawer = _drawer)}
        drawerPosition={DrawerLayoutAndroid.positions.left}
        renderNavigationView={() => navigationView}>
          <MapView style={styles.map}
            mapType="satellite"
            showsUserLocation={false}
            showsPointsOfInterest={false}
            showsCompass={false}
            showsTraffic={false}
            showsIndoors={false}
            rotateEnabled={false}
            followsUserLocation={false}
            toolbarEnabled={false}
            showsMyLocationButton={false}
            initialRegion={{
                latitude: 50.6014,
                longitude: 3.5113,
                latitudeDelta: 0.0922,
                longitudeDelta: 0.0421,
                }}>
                {
                  grazings.length > 0 &&
                  (_.map(grazings, (grazing) => {
                    return <MapView.Polygon
                      key={grazing.id}
                      strokeWidth={2}
                      strokeColor={"#FFFFFF"}
                      coordinates={grazing.coordinates} />
                  }))
                }
                {
                  cows.length > 0 &&
                  (<MapView.Heatmap
                    points={cows}
                    radius={40}
                    heatmapMode={"POINTS_DENSITY"}/>)
                }
                {
                  strangeCows.length > 0 &&
                  (<MapView.Heatmap
                    points={strangeCows}
                    gradient={{
                             colors: ["#00000000", "#00FFFF", "#00FFFF", "#00BFFF", "#007FFF", "#003FFF", "#0000FF", "#0000DF", "#0000BF", "#00009F", "#00007F", "#3F005B", "#7F003F", "#BF001F", "#FF0000"],
                             values: [0, 0.0714, 0.1428, 0.2142, 0.2857, 0.3571, 0.4285, 0.5, 0.5714, 0.6428, 0.7142, 0.7857, 0.8571, 0.9285, 1]}}
                    radius={40}
                    heatmapMode={"POINTS_DENSITY"}/>)
                }
                {
                  this.state.cows.length > 0 &&
                  _.map(_.filter(this.state.cows, (cow) => { return cow.locations.length > 0}), cow =>
                    {
                      return  <Marker
                        key={cow.cowId}
                        title={cow.cowName + (cow.temperatures.length > 0 ? " - " + cow.temperatures[0].temperature + "°C" : "")}
                        image={cow.temperatures.length > 0 && cow.temperatures[0].temperature > MIN_ADULT_TEMPERATURE && cow.temperatures[0].temperature < MAX_ADULT_TEMPERATURE ?
                           markerIcon : markerWarningIcon}
                        coordinate={cow.locations[0]}/>
                    }
                  )
                }
        </MapView>
        <Button
          title="Options"
          onPress={this.openDrawer}/>
      </DrawerLayoutAndroid>
    );
  }
}

const App = createStackNavigator({
  Map: { screen: MapScreen }
});
export default App;

const styles = StyleSheet.create({
  map: {
     height: 100,
     flex: 1
   },
  icon: {
    width: 24,
    height: 24,
  },
  container: {
    flex:1,
    marginTop:20
  },
  welcome: {
    fontSize: 20,
    textAlign: 'center',
    margin: 10,
  },
  instructions: {
    textAlign: 'center',
    color: '#333333',
    marginBottom: 5,
  },
  centered: {
    textAlign: 'center',
    color: '#333333',
  },
});
