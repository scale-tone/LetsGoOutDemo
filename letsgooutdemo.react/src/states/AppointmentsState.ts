import { observable, action } from "mobx"
import axios from 'axios';

export const BackendBaseUri = process.env.REACT_APP_BACKEND_BASE_URI as string;

export enum AppointmentStatusEnum {
    Pending = 0,
    Accepted,
    Declined
}

// State of one particular appointment
class AppointmentState {
    // Appointment's Id. Typically it is a creation timestamp in a sortable format.
    id: string = '';
    // List of proposed participant's nickNames
    participants: string[] = [];
    // Current status of the appointment
    status: AppointmentStatusEnum = AppointmentStatusEnum.Pending;
}

// Appointments handling logic
export class AppointmentsState {

    // Appointments are stored as a dictionary
    @observable
    appointments: { [id: string]: AppointmentState } = {};

    // Handles 'appointment-state-changed' event from server
    @action.bound
    appointmentStateChanged(newState: AppointmentState) {
        // Simply merging the newly arrived state with the map of existing ones
        this.appointments[newState.id] = { ...this.appointments[newState.id], ...newState };
    }

    // Sends a response about the appointment
    @action.bound
    respondToAppointment(id: string, accepted: boolean) {

        // Responding to the server with Arranged status
        axios.post(`${BackendBaseUri}/appointments/${id}`, accepted ? AppointmentStatusEnum.Accepted : AppointmentStatusEnum.Declined);        
    }
}