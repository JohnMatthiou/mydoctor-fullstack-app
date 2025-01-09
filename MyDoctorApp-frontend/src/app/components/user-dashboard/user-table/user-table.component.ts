import { Component, Input, inject } from '@angular/core';
import { UserDoctor, UserPatient } from '../../../shared/interfaces/backend';

@Component({
  selector: 'app-user-table',
  standalone: true,
  imports: [],
  templateUrl: './user-table.component.html',
  styleUrl: './user-table.component.css'
})
export class UserTableComponent {
  @Input() user: UserPatient | UserDoctor
}
