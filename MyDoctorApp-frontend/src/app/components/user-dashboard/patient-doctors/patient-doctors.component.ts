import { Component, OnInit, inject } from '@angular/core';
import { SidemenuComponent } from '../sidemenu/sidemenu.component';
import { PatientDoctorsDatatableComponent } from './patient-doctors-datatable/patient-doctors-datatable.component';
import { PaginationComponent } from '../pagination/pagination.component';
import { UserService } from '../../../shared/services/user.service';
import { UserDoctor } from '../../../shared/interfaces/backend';

@Component({
  selector: 'app-patient-doctors',
  standalone: true,
  imports: [SidemenuComponent, PatientDoctorsDatatableComponent, PaginationComponent],
  templateUrl: './patient-doctors.component.html',
  styleUrl: './patient-doctors.component.css'
})
export class PatientDoctorsComponent implements OnInit {

  userService = inject(UserService);
  user = this.userService.user;
  specialtyMapping = this.userService.specialtyMapping;

  doctorsList: UserDoctor[] = [];
  pageNumber: number = 1;
  totalPages: number = 1;
  pageSize: number = 5;
  noResults = false;
  errorMessage: string;

  ngOnInit(): void {
    this.fetchPatientDoctorsPage(1);
  }

  fetchData() {
    this.fetchPatientDoctorsPage(this.pageNumber);
  }

  fetchPatientDoctorsPage(pageNumber: number) {
    this.pageNumber = pageNumber;
    this.userService.getPatientDoctorsPage(this.user().id, pageNumber, this.pageSize).subscribe({
      next: (response) => {
        console.log("No Errors", response)
        this.noResults = false;
        this.doctorsList = response.data.map(doctor => {
          const specialtyInGreek = this.specialtyMapping[doctor.doctorSpecialty];
          return {
            ...doctor,
            doctorSpecialty: specialtyInGreek, 
          };
        });
        this.totalPages = response.totalPages
      },
      error: (response) => {
        console.log("Errors", response)
        this.noResults = true;
        this.errorMessage = response.error.message;
      }
    })
  }
}
