import { StatusBar } from 'expo-status-bar';
import React from 'react';
import { observer } from 'mobx-react';
import { StyleSheet, Text, View, Button, Alert } from 'react-native';

import { MainState } from './states/MainState';

@observer
export default class App extends React.Component<{ state: MainState }> {

    render(): JSX.Element {
        return (
            <View style={styles.container}>

                <Button title="Test" onPress={() => Alert.alert('Simple Button pressed')}/>

                <Text>Hi from Tino!</Text>
                <StatusBar style="auto" />
            </View>
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
