import React from 'react';
import { observer } from 'mobx-react';

import { Button, Input } from 'react-native-elements';

import { SendInviteState } from '../states/SendInviteState';

@observer
export class SendInvite extends React.Component<{ state: SendInviteState }> {

    render(): JSX.Element {

        const state = this.props.state;

        return (<>

            <Input
                placeholder='Invite folks to go out...'
                value={state.nickNamesInputText}
                onChangeText={(txt) => { state.nickNamesInputText = txt; }}
            />

            <Button title='Invite' onPress={() => { state.sendInvite() }} />
        </>);
    }
}

