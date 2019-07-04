import * as React from 'react';
import { action } from "mobx"
import { observer } from 'mobx-react';

import './SendInvite.css';

import { SendInviteState } from '../states/SendInviteState';

// Component for sending appointment invites
@observer
export class SendInvite extends React.Component<{ state: SendInviteState }> {

    render(): JSX.Element {
        const state = this.props.state;

        return (
            <div className="card appointment-card ">
                <div className="card-body container-fluid">
                    <div className="row">
                        <div className="col-sm-4 description-div">
                                Invite some folks to go out tonight:
                        </div>
                        <div className="col-sm-4">
                            <input className="form-control" type="search" placeholder="Comma-separated nicknames..."
                                value={state.nickNamesInputText}
                                onChange={this.onNickNamesChanged}
                                onKeyPress={this.onNickNamesKeyPress} 
                            />
                        </div>
                        <div className="col-sm-4 text-center">
                            <button className="btn btn-success invite-button"
                                onClick={state.sendInvite}
                                disabled={!state.nickNamesInputText}
                            >
                                Invite
                            </button>
                        </div>
                    </div>
                </div>
            </div>
        );
    }

    @action.bound
    onNickNamesKeyPress(event: React.KeyboardEvent<HTMLInputElement>) {
        if (event.key === 'Enter') {
            // Otherwise the event will bubble up and the form will be submitted
            event.preventDefault();

            this.props.state.sendInvite();
        }
    }

    @action.bound
    onNickNamesChanged(event: React.FormEvent<HTMLInputElement>) {
        this.props.state.nickNamesInputText = event.currentTarget.value;
    }
}