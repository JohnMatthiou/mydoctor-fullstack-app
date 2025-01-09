import { Component, OnInit, inject } from '@angular/core';
import { SidemenuComponent } from '../sidemenu/sidemenu.component';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormGroup, FormControl, Validators } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatButtonModule } from '@angular/material/button';
import { UserService } from '../../../shared/services/user.service';
import { User } from '../../../shared/interfaces/backend';
import { RouterLink } from '@angular/router';


@Component({
  selector: 'app-user-update',
  standalone: true,
  imports: [SidemenuComponent, ReactiveFormsModule, MatButtonModule, MatFormFieldModule, MatInputModule, CommonModule, RouterLink],
  templateUrl: './user-update.component.html',
  styleUrl: './user-update.component.css'
})
export class UserUpdateComponent implements OnInit {
  userService = inject(UserService);
  loggedInUser = this.userService.user;

  updateStatus: { success: boolean, message: string } = {
    success: false,
    message: "Not attempted yet"
  }

  form = new FormGroup({
    username: new FormControl('', [Validators.required, Validators.minLength(2), Validators.maxLength(50), Validators.pattern(/^\S+$/)]),
    email: new FormControl('', [Validators.required, Validators.email, Validators.maxLength(100)]),
    firstname: new FormControl('', [Validators.required, Validators.minLength(2), Validators.maxLength(50), Validators.pattern(/^\S+$/)]),
    lastname: new FormControl('', [Validators.required, Validators.minLength(2), Validators.maxLength(50), Validators.pattern(/^\S+$/)]),
    phoneNumber: new FormControl('', [Validators.required, Validators.minLength(10), Validators.maxLength(15), Validators.pattern(/^\S+$/)]),
    city: new FormControl('', [Validators.required, Validators.minLength(2), Validators.maxLength(100), Validators.pattern(/^\S+$/)]),
    address: new FormControl('', [Validators.required, Validators.minLength(2), Validators.maxLength(100), Validators.pattern(/^[^\s](.*[^\s])?$/)])
  })

  returnToForm() {
    this.updateStatus = { success: false, message: 'Not attempted yet' }
    this.fetchUser();
  }

  ngOnInit(): void {
    this.fetchUser();
  }

  fetchUser(): void {
    if (this.loggedInUser().role === "Patient") {
      this.userService.getPatient(this.loggedInUser().id).subscribe({
        next: (response) => {
          console.log("No Errors", response)
          this.form.patchValue({
            username: response.username,
            email: response.email,
            firstname: response.firstname,
            lastname: response.lastname,
            phoneNumber: response.phoneNumber,
            city: response.city,
            address: response.address
          });
        },
        error: (response) => {
          console.log("Errors", response)
        }
      })
    } else {
      this.userService.getDoctor(this.loggedInUser().id).subscribe({
        next: (response) => {
          console.log("No Errors", response)
          this.form.patchValue({
            username: response.username,
            email: response.email,
            firstname: response.firstname,
            lastname: response.lastname,
            phoneNumber: response.phoneNumber,
            city: response.city,
            address: response.address
          });
        },
        error: (response) => {
          console.log("Errors", response)
        }
      })
    }
  }

  onSubmit(value: any) {

    const user: User = {
      id: this.loggedInUser().id,
      username: this.form.get("username").value,
      email: this.form.get("email").value,
      firstname: this.form.get("firstname").value,
      lastname: this.form.get("lastname").value,
      phoneNumber: this.form.get("phoneNumber").value,
      city: this.form.get("city").value,
      address: this.form.get("address").value,
    }

    if (this.loggedInUser().role === "Patient") {
      this.userService.updatePatient(this.loggedInUser().id, user).subscribe({
        next: (response) => {
          console.log("No Errors", response)
          this.updateStatus = { success: true, message: "Επιτυχής ενημέρωση προφίλ" }
        },
        error: (response) => {
          console.log("Errors", response)
          let message = response.error.message;
          this.updateStatus = { success: false, message: message }
        }
      })
    } else {
      this.userService.updateDoctor(this.loggedInUser().id, user).subscribe({
        next: (response) => {
          console.log("No Errors", response)
          this.updateStatus = { success: true, message: "Επιτυχής ενημέρωση προφίλ" }
        },
        error: (response) => {
          console.log("Errors", response)
          let message = response.error.message;
          this.updateStatus = { success: false, message: message }
        }
      })
    }

  }
}
