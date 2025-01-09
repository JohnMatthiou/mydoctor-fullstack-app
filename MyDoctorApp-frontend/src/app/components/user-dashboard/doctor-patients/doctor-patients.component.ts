import { Component, OnInit, inject } from '@angular/core';
import { SidemenuComponent } from '../sidemenu/sidemenu.component';
import { DoctorPatientsDatatableComponent } from './doctor-patients-datatable/doctor-patients-datatable.component';
import { PaginationComponent } from '../pagination/pagination.component';
import { UserService } from '../../../shared/services/user.service';
import { UserPatient } from '../../../shared/interfaces/backend';

@Component({
  selector: 'app-doctor-patients',
  standalone: true,
  imports: [SidemenuComponent, DoctorPatientsDatatableComponent, PaginationComponent],
  templateUrl: './doctor-patients.component.html',
  styleUrl: './doctor-patients.component.css'
})
export class DoctorPatientsComponent implements OnInit {
  userService = inject(UserService);
  user = this.userService.user;

  patientsList: UserPatient[] = [];
  pageNumber: number = 1;
  totalPages: number = 1;
  pageSize: number = 5;
  noResults = false;
  errorMessage: string;

  ngOnInit(): void {
    this.fetchDoctorPatientsPage(1);
  }

  fetchData() {
    this.fetchDoctorPatientsPage(this.pageNumber);
  }

  fetchDoctorPatientsPage(pageNumber: number) {
    this.pageNumber = pageNumber;
    this.userService.getDoctorPatientsPage(this.user().id, pageNumber, this.pageSize).subscribe({
      next: (response) => {
        console.log("No Errors", response)
        this.noResults = false;
        this.patientsList = response.data;
        this.totalPages = response.totalPages;
      },
      error: (response) => {
        console.log("Errors", response)
        this.noResults = true;
        this.errorMessage = response.error.message;
      }
    })
  }

}
