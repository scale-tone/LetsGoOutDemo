import { observable, computed, action } from "mobx"
import axios from 'axios';

import { HubConnectionBuilder } from "@aspnet/signalr";

// Configuration parameters coming from .env file. On your devbox you'd probably want to override them via .env.local file.
export const BackendBaseUri = process.env.REACT_APP_BACKEND_BASE_URI as string;

// Login/logout logic
export class LoginState {

    constructor(private signalRMessageHandler: (message: any) => any) {}

    @observable
    nickName: string = '';

    @observable
    nickNameInputText: string = '';

    @computed
    get isLoggedIn() {
        return !!this.nickName;
    }

    // Establishes SignalR connection and configures Axios
    @action.bound
    login() {
        
        // Configuring SignalR
        const signalrConn = new HubConnectionBuilder()
            .withUrl(`${BackendBaseUri}?nick-name=${this.nickNameInputText}`)
            .build();
        
        // Also telling axios to put this name as an HTTP header into each request
        axios.defaults.headers.common['x-nick-name'] = this.nickNameInputText;

        // Mounting the event handler
        signalrConn.on('appointment-state-changed', this.signalRMessageHandler);

        // Establishing SignalR connection
        signalrConn.start().then(
            () => {
                this.nickName = this.nickNameInputText;
            }, err => {
                alert(`Failed to connect to SignalR:  ${JSON.stringify(err)}`);
            });
    }
}  