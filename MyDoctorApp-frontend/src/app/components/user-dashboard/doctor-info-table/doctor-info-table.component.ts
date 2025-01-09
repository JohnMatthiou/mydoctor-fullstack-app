import { Component, Input } from '@angular/core';
import { DoctorInfo } from '../../../shared/interfaces/backend';

@Component({
  selector: 'app-doctor-info-table',
  standalone: true,
  imports: [],
  templateUrl: './doctor-info-table.component.html',
  styleUrl: './doctor-info-table.component.css'
})
export class DoctorInfoTableComponent {
  @Input() doctorInfo: DoctorInfo
}
