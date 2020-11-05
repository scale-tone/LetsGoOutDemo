import { observable, action } from 'mobx'
import axios from 'axios';

import { BackendBaseUri } from './MainState';

// Main Application State
export class SendInviteState {

    @observable
    nickNamesInputText: string = '';

    @action.bound
    sendInvite() {

        const nickNames = this.nickNamesInputText;
        this.nickNamesInputText = '';

        axios.post(`${BackendBaseUri}/new-appointment`, nickNames, { headers: { 'Content-Type': 'text/plain' }})
            .catch(err => alert(`Failed to send an invite! ${err}`));
    }
}