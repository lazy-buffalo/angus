/**
 * Sample React Native App
 * https://github.com/facebook/react-native
 *
 * @format
 * @flow
 */

import React, {Component} from 'react';
import {Platform, StyleSheet, Text, View, DrawerLayoutAndroid, TouchableHighlight, SideBarMenu, ListView} from 'react-native';
import Row from './components/row';

const instructions = Platform.select({
  ios: 'Cmd+R to reload,\n' + 'Cmd+D or shake for dev menu',
  android:
    'Shake or press menu button for dev menu',
});

type Props = {};

//App
class LoginPage extends React.Component {
    constructor() {
        super();
        const ds = new ListView.DataSource({rowHasChanged: (r1, r2) => r1 !== r2});
        this.state = {
            dataSource:ds.cloneWithRows([{first:"first", last:"last"}, {first:"first", last:"last"}])
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
        // <View style={{flex: 1, backgroundColor: '#fff'}}>
        //   <Text style={{margin: 10, fontSize: 15, textAlign: 'left'}}>Im the Drawer!</Text>
        // </View>
      );
      return (
        <DrawerLayoutAndroid
          style={{marginTop: 28}} // safe zone
          drawerWidth={300}
          drawerPosition={DrawerLayoutAndroid.positions.left}
          renderNavigationView={() => navigationView}>
          <View style={{flex: 1, alignItems: 'center'}}>
            <Text style={{margin: 10, fontSize: 15, textAlign: 'right'}}>Hello</Text>
            <Text style={{margin: 10, fontSize: 15, textAlign: 'right'}}>World!</Text>
          </View>
        </DrawerLayoutAndroid>
      );
    }
}

export default class App extends React.Component {
  render() {
    return <LoginPage />;
  }
}

const styles = StyleSheet.create({
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
