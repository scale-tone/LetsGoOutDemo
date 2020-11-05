import { LoginState } from './LoginState';
import { AppointmentsState } from './AppointmentsState';
import { SendInviteState } from './SendInviteState';

import config from '../config.json';
export const BackendBaseUri = config.backendBaseUri;

// Main Application State
export class MainState {
    appointmentsState: AppointmentsState = new AppointmentsState();
    loginState: LoginState = new LoginState(this.appointmentsState.appointmentStateChanged);
    sendInviteState: SendInviteState = new SendInviteState();
}