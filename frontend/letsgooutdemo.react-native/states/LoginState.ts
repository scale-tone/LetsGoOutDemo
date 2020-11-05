import { observable, computed, action } from "mobx"
import axios from 'axios';

import { HubConnectionBuilder } from "@aspnet/signalr";

import { BackendBaseUri } from './MainState';

// Login/logout logic
export class LoginState {

    constructor(private signalRMessageHandler: (message: any) => any) {

        // On any auth-related error, forcing a page reload, which in turn forces a re-login
        axios.interceptors.response.use(response => response, err => {

            if (err.message === 'Network Error') {
                location.reload(true);
                return;
            }

            return Promise.reject(err);
        });
    }

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

        // Background reconnects are essential here. That's because in 'Default' or 'Classic' service mode
        // clients get forcibly disconnected, when your backend restarts.
        signalrConn.onclose(() => {
            var tryToReconnect = () => {
                console.log('Reconnecting to SignalR...');
                signalrConn.start().then(() => {
                    console.log('Reconnected to SignalR!');
                }, () => { 
                    setTimeout(tryToReconnect, 5000);
                })
            }
            tryToReconnect();
        });

        // Establishing SignalR connection
        signalrConn.start().then(
            () => {
                this.nickName = this.nickNameInputText;
            }, err => {
                alert(`Failed to connect to SignalR:  ${JSON.stringify(err)}`);
            });
    }
}  