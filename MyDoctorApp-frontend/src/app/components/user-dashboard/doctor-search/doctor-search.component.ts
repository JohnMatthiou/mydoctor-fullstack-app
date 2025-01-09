import { Component, OnInit, inject } from '@angular/core';
import { SidemenuComponent } from '../sidemenu/sidemenu.component';
import { FiltersFormComponent } from './filters-form/filters-form.component';
import { DoctorsDatatableComponent } from './doctors-datatable/doctors-datatable.component';
import { FiltersForm, UserDoctor } from '../../../shared/interfaces/backend';
import { UserService } from '../../../shared/services/user.service';
import { PaginationComponent } from '../pagination/pagination.component';

@Component({
  selector: 'app-doctor-search',
  standalone: true,
  imports: [SidemenuComponent, FiltersFormComponent, DoctorsDatatableComponent, PaginationComponent],
  templateUrl: './doctor-search.component.html',
  styleUrl: './doctor-search.component.css'
})
export class DoctorSearchComponent implements OnInit {

  userService = inject(UserService);
  user = this.userService.user;
  specialtyMapping = this.userService.specialtyMapping;

  doctorsList: UserDoctor[] = [];
  pageNumber: number = 1;
  totalPages: number = 1;
  pageSize: number = 5;
  noResults = false;
  errorMessage: string;

  filters: FiltersForm = {
    lastname: '',
    city: '',
    specialty: ''
  }

  ngOnInit(): void {
    this.fetchFilteredDoctorsPage(1);
  }

  fetchFilteredDoctors(filters: FiltersForm) {
    this.filters = filters
    this.pageNumber = 1;
    this.fetchFilteredDoctorsPage(this.pageNumber);
  }

  fetchFilteredDoctorsPage(pageNumber: number) {
    this.pageNumber = pageNumber;
    this.userService.getFilteredDoctorsPage(this.filters, pageNumber, this.pageSize).subscribe({
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
