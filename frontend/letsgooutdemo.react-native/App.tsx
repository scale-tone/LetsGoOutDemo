import { StatusBar } from 'expo-status-bar';
import React from 'react';
import { observer } from 'mobx-react';

import axios from 'axios';


import { StyleSheet, View, Alert } from 'react-native';
import { Button, Header, Input, Icon, Text } from 'react-native-elements';



import { MainState } from './states/MainState';

import { Main } from './components/Main';


@observer
export default class App extends React.Component<{}> {

    appState: MainState = new MainState();

    render(): JSX.Element {
        return (
            <Main state={this.appState}/>
        );
    }
}


const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: '#fff',
    alignItems: 'center',
    justifyContent: 'center',
  },
});
