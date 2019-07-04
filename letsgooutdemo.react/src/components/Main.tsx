import * as React from 'react';
import { observer } from 'mobx-react';

import { MainState } from '../states/MainState';

import { Appointments } from './Appointments';
import { SendInvite } from './SendInvite';

// The main application view
@observer
export class Main extends React.Component<{ state: MainState }> {

    render(): JSX.Element {
        const mainState = this.props.state;

        if (!mainState.loginState.isLoggedIn) {
            return <div/>;
        }

        return (
            <div>
                <SendInvite state={mainState.sendInviteState} />
                <Appointments nickName={mainState.loginState.nickName} state={mainState.appointmentsState} />
            </div>
        );
    }
}