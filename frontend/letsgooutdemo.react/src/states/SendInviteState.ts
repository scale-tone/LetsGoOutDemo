import { observable, action } from 'mobx'
import axios from 'axios';

// Configuration parameters coming from .env file. On your devbox you'd probably want to override them via .env.local file.
export const BackendBaseUri = process.env.REACT_APP_BACKEND_BASE_URI as string;

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