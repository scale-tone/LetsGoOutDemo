import React from 'react';
import { observer } from 'mobx-react';

import { Button, Input, Text } from 'react-native-elements';

import { LoginState } from '../states/LoginState';

@observer
export class LoginButton extends React.Component<{ state: LoginState }> {

    render(): JSX.Element {

        const state = this.props.state;

        if (!!state.nickName) {
            return (<>
                <Text h4 style={{padding: 10}}>Hello, {state.nickName}</Text>
            </>);
        }

        return (<>

            <Input
                placeholder='Your nickname...'
                value={state.nickNameInputText}
                onChangeText={(txt) => { state.nickNameInputText = txt; }}
            />

            <Button title='Login' onPress={() => { state.login() }} />
        </>);
    }
}

