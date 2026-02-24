import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  standalone: true,
  selector: 'app-users-list',
  imports: [CommonModule, FormsModule],
    templateUrl: './users-list.component.html',
})
export class UsersListComponent {
  constructor() {}
}
