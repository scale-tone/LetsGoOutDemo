import React from 'react';
import { observer } from 'mobx-react';

import { ListItem } from 'react-native-elements';

import { AppointmentsState, AppointmentStatusEnum } from '../states/AppointmentsState';

@observer
export class Appointments extends React.Component<{ nickName: string, state: AppointmentsState }> {

    render(): JSX.Element[] {

        const state = this.props.state;

        // Ordering by appointmentId descending
        const appointmentIds = Object.keys(state.appointments).reverse();
        return appointmentIds.map(id => {
            const appointment = state.appointments[id];

            var iconName, color;
            switch (appointment.status) {
                case AppointmentStatusEnum.Accepted:
                    iconName = 'check-circle';
                    color = 'green';
                    break;
                case AppointmentStatusEnum.Declined:
                    iconName = 'cancel';
                    color = 'red';
                    break;
                default:
                    iconName = 'help';
                    color = '#856404';
                    break;
            }

            return (<ListItem key={id} leftIcon={{ name: iconName, color: color }}>
                
                <ListItem.Content>
                    <ListItem.Title style={{ color }}>
                        Are you going out with {this.renderParticipants(appointment.participants)} ?
                    </ListItem.Title>
                </ListItem.Content>

                <ListItem.ButtonGroup
                    disabled={appointment.status != AppointmentStatusEnum.Pending}
                    buttons={["Yes", "No"]}
                    onPress={i => { state.respondToAppointment(appointment.id, !i) }}
                />
                
            </ListItem>);
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
            result.push(participants[i]);
        }

        return result;
    }
}