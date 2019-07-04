import * as React from "react";
import { action } from "mobx"
import { observer } from 'mobx-react';

import './LoginButton.css';

import { LoginState } from '../states/LoginState';

// Handles login functionality, which by now is just about providing the user's nickName
@observer
export class LoginButton extends React.Component<{ state: LoginState }> {

    render(): JSX.Element {
        const state = this.props.state;

        if (state.isLoggedIn) {
            return (
                <div>
                    <span className="navbar-text ml-auto mr-3">
                        Hello, {state.nickName}
                    </span>
                </div>
            );
        }

        return (
            <div className="form-inline">
                <input className="form-control mr-sm-2 nickname-input" type="search" placeholder="Your nickname..."
                    value={state.nickNameInputText}
                    onChange={this.onNickNameChanged} 
                    onKeyPress={this.onNickNameKeyPress} 
                />
                <button className="btn btn-secondary login-button" type="button"
                    onClick={this.onLoginButtonClicked}
                    disabled={!state.nickNameInputText}
                >
                    Connect
                </button>
            </div>
        );
    }

    @action.bound onLoginButtonClicked() {
        this.props.state.login();
    }

    @action.bound
    onNickNameKeyPress(event: React.KeyboardEvent<HTMLInputElement>) {
        if (event.key === 'Enter') {
            // Otherwise the event will bubble up and the form will be submitted
            event.preventDefault();

            this.onLoginButtonClicked();
        }
    }

    @action.bound
    onNickNameChanged(event: React.FormEvent<HTMLInputElement>) {
        this.props.state.nickNameInputText = event.currentTarget.value;
    }
}