import * as React from 'react';
import { observer } from 'mobx-react';

import './Appointments.css';

import { AppointmentsState, AppointmentStatusEnum } from '../states/AppointmentsState';

// View of all proposed appointments
@observer
export class Appointments extends React.Component<{ nickName: string, state: AppointmentsState }> {

    render(): JSX.Element[] {
        const state = this.props.state;

        // Ordering by appointmentId descending
        const appointmentIds = Object.keys(state.appointments).reverse();

        return appointmentIds.map(id => {
            const appointment = state.appointments[id];

            var cardBorderStyle = 'border-warning';
            var statusDiv = <div className="alert alert-warning status-badge h5">Pending...</div>;
            var buttonsDisabled = false;
            switch (appointment.status) {
                case AppointmentStatusEnum.Accepted:
                    cardBorderStyle = 'border-success';
                    statusDiv = <div className="alert alert-success status-badge h5">Agreed :)</div>;
                    buttonsDisabled = true;
                    break;
                case AppointmentStatusEnum.Rejected:
                    cardBorderStyle = 'border-danger';
                    statusDiv = <div className="alert alert-danger status-badge h5">No Luck :(</div>;
                    buttonsDisabled = true;
                    break;
            }

            return (
                <div className={"card appointment-card " + cardBorderStyle} key={id}>
                    <div className="card-body container-fluid">
                        <div className="row">
                            <div className="col-sm-3">{statusDiv}</div>
                            <div className="col-sm-5">
                                Are you going out with {this.renderParticipants(appointment.participants)} tonight?
                            </div>
                            <div className="col-sm-4 text-center">
                                <button className="btn btn-success response-button"
                                    onClick={() => state.respondToAppointment(appointment.id, true)}
                                    disabled={buttonsDisabled}
                                >
                                    Yes
                                </button>
                                <button className="btn btn-danger response-button"
                                    onClick={() => state.respondToAppointment(appointment.id, false)}
                                    disabled={buttonsDisabled}
                                >
                                    No
                                </button>
                            </div>
                        </div>
                    </div>
                </div>
            );
        });
    }

    renderParticipants(participants: string[]): (JSX.Element | string)[] {
        const result = [];

        // Removing ourselves from the rendered list of participants
        const index = participants.indexOf(this.props.nickName);
        if (index > -1) {
            participants.splice(index, 1);

            // If I am the only one participant
            if (!participants.length) {
                participants.push('yourself');
            }
        }

        for (var i = 0; i < participants.length; i++) {

            const delimiter = i ? ((i === participants.length - 1) ? ' and ' : ', ') : '';
            result.push(delimiter);
            result.push(
                <h1 className="badge badge-light participant-badge" key={i}>{participants[i]}</h1>
            );
        }

        return result;
    }
}