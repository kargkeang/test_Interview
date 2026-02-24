import {Component} from '@angular/core';
import {CommonModule} from '@angular/common';

@Component({
    standalone: true,
    selector: 'app-user-detail',
    imports: [CommonModule],
    templateUrl: './user-detail.component.html',
})
export class UserDetailComponent {

    constructor() {
    }
}
