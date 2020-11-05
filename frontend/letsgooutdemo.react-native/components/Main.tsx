import { StatusBar } from 'expo-status-bar';
import React from 'react';
import { observer } from 'mobx-react';

import { View } from 'react-native';
import { Header } from 'react-native-elements';

import { MainState } from '../states/MainState';
import { LoginButton } from './LoginButton';
import { SendInvite } from './SendInvite';
import { Appointments } from './Appointments';

@observer
export class Main extends React.Component<{ state: MainState }> {

    render(): JSX.Element {

        const state = this.props.state;

        return (
            <View>
                <Header centerComponent={{ text: 'Let\'s Go Out Demo', style: { color: '#fff' } }}/>

                <LoginButton state={state.loginState} />

                {state.loginState.isLoggedIn && (<>
                    <SendInvite state={state.sendInviteState} />
                    <Appointments state={state.appointmentsState} nickName={state.loginState.nickName}/>
                </>)}

                <StatusBar style="auto"/>
            </View>
        );
    }
}
