import React from 'react';
import { observer } from 'mobx-react';

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