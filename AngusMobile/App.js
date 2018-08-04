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
import MapView, {Marker} from  'react-native-maps';
import {createStackNavigator} from 'react-navigation';
import markerIcon from './marker.png';

YellowBox.ignoreWarnings(['Warning: isMounted(...) is deprecated', 'Module RCTImageLoader']);

const instructions = Platform.select({
  ios: 'Cmd+R to reload,\n' + 'Cmd+D or shake for dev menu',
  android:
    'Shake or press menu button for dev menu',
});

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
          markers:[{title:"test", coordinate:{latitude:13.139238380834923,longitude:80.25188422300266}}]
      }
      this.openDrawer = this.openDrawer.bind(this);
  }

  openDrawer() {
      this.drawer.openDrawer();
  }

  render() {
    var navigationView = (
      <ListView
        dataSource={this.state.dataSource}
        renderRow={(data) => <Row name={data} />}/>
    );
    return (
      <DrawerLayoutAndroid
        drawerWidth={300}
        ref={_drawer => (this.drawer = _drawer)}
        drawerPosition={DrawerLayoutAndroid.positions.left}
        renderNavigationView={() => navigationView}>
          <MapView style={styles.map}
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
                latitude: 13.139238380834923,
                longitude: 80.25188422300266,
                latitudeDelta: 0.0922,
                longitudeDelta: 0.0421,
                }}>
                {
                  this.state.markers.map((marker, i) => (
                  <Marker
                    key={i}
                    title={marker.title}
                    image={markerIcon}
                    coordinate={marker.coordinate}/>
                  ))
                }
        </MapView>
        <Button
          title="Menu"
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
    flex: 1,
    justifyContent: 'center',
    alignItems: 'center',
    backgroundColor: '#F5FCFF',
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
});
