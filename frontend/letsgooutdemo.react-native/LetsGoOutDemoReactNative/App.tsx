import { StatusBar } from 'expo-status-bar';
import React from 'react';
import { observer } from 'mobx-react';

import axios from 'axios';


import { StyleSheet, Text, View, Button, Alert } from 'react-native';



import { MainState } from './states/MainState';

@observer
export class MainView extends React.Component<{ state: MainState }> {

    render(): JSX.Element {
        return (
            <View style={styles.container}>

                <Button title="Test" onPress={() => {

                    this.props.state.someString = '';

                    axios.get('https://konstjstest.azurewebsites.net/api/httptrigger1').then(result => {

                        this.props.state.someString = JSON.stringify(result.data);

                    });


                    //                    Alert.alert('Simple Button pressed')
                }} />

                <Text>{this.props.state.someString}</Text>
                <StatusBar style="auto" />
            </View>
        );
    }
}


@observer
export default class App extends React.Component<{}> {

    appState: MainState = new MainState();

    render(): JSX.Element {
        return (
            <MainView state={this.appState}></MainView>
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
