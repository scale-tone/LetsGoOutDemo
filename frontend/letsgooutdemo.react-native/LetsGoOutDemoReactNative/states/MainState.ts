import { action, observable, computed } from 'mobx';

// Main Application State
export class MainState {

    @observable
    someString: string = 'some value';

}