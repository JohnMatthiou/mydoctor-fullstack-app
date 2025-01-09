import { Component, inject, OnInit } from '@angular/core';
import { SidemenuComponent } from '../sidemenu/sidemenu.component';
import { UserTableComponent } from '../user-table/user-table.component';
import { UserService } from '../../../shared/services/user.service';
import { UserDoctor, UserPatient } from '../../../shared/interfaces/backend';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-user-profile',
  standalone: true,
  imports: [SidemenuComponent, UserTableComponent, RouterLink],
  templateUrl: './user-profile.component.html',
  styleUrl: './user-profile.component.css'
})
export class UserProfileComponent implements OnInit {

  userService = inject(UserService);
  specialtyMapping = this.userService.specialtyMapping;
  loggedInUser = this.userService.user;
  user: UserPatient | UserDoctor = null;

  retrievalStatus: { success: boolean, message: string } = {
    success: false,
    message: "Loading"
  }

  ngOnInit(): void {
    this.fetchUser();
  }

  fetchUser(): void {
    if (this.loggedInUser().role === "Patient") {
      this.userService.getPatient(this.loggedInUser().id).subscribe({
        next: (response) => {
          console.log("No Errors", response)
          this.user = response;
          this.retrievalStatus = { success: true, message: "Completed" }
        },
        error: (response) => {
          console.log("Errors", response)
          this.retrievalStatus = { success: false, message: "Σφάλμα κατά τη φόρτωση του χρήστη" }
        }
      })
    } else {
      this.userService.getDoctor(this.loggedInUser().id).subscribe({
        next: (response) => {
          console.log("No Errors", response)
          const specialtyInGreek = this.specialtyMapping[response.doctorSpecialty]
          this.user = {
            ...response,
            doctorSpecialty: specialtyInGreek,
          }
          this.retrievalStatus = { success: true, message: "Completed" }
        },
        error: (response) => {
          console.log("Errors", response)
          this.retrievalStatus = { success: false, message: "Σφάλμα κατά τη φόρτωση του χρήστη" }
        }
      })
    }
  }

}
