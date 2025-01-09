import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormGroup, FormControl, Validators, AbstractControl } from '@angular/forms';
import { MatSelectModule } from '@angular/material/select';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatButtonModule } from '@angular/material/button';
import { UserService } from '../../shared/services/user.service';
import { UserDoctor } from '../../shared/interfaces/backend';
import { RouterLink } from '@angular/router';


@Component({
  selector: 'app-doctor-signup',
  standalone: true,
  imports: [ReactiveFormsModule, MatButtonModule, MatFormFieldModule, MatInputModule, MatSelectModule, CommonModule, RouterLink],
  templateUrl: './doctor-signup.component.html',
  styleUrl: './doctor-signup.component.css'
})
export class DoctorSignupComponent {

  userService = inject((UserService))

  registrationStatus: { success: boolean, message: string } = {
    success: false,
    message: "Not attempted yet"
  }

  form = new FormGroup({
    username: new FormControl('', [Validators.required, Validators.minLength(2), Validators.maxLength(50), Validators.pattern(/^\S+$/)]),
    email: new FormControl('', [Validators.required, Validators.email, Validators.maxLength(100)]),
    password: new FormControl('', [Validators.required, Validators.pattern(/^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?\d)(?=.*?\W).{8,20}$/)]),
    confirmPassword: new FormControl('', Validators.required),
    firstname: new FormControl('', [Validators.required, Validators.minLength(2), Validators.maxLength(50), Validators.pattern(/^\S+$/)]),
    lastname: new FormControl('', [Validators.required, Validators.minLength(2), Validators.maxLength(50), Validators.pattern(/^\S+$/)]),
    phoneNumber: new FormControl('', [Validators.required, Validators.minLength(10), Validators.maxLength(15), Validators.pattern(/^\S+$/)]),
    city: new FormControl('', [Validators.required, Validators.minLength(2), Validators.maxLength(100), Validators.pattern(/^\S+$/)]),
    address: new FormControl('', [Validators.required, Validators.minLength(2), Validators.maxLength(100), Validators.pattern(/^[^\s](.*[^\s])?$/)]),
    afm: new FormControl('', [Validators.required, Validators.minLength(9), Validators.maxLength(15), Validators.pattern(/^\S+$/)]),
    doctorSpecialty: new FormControl('', Validators.required)
  },
    this.passwordConfirmPasswordValidator
  )

  passwordConfirmPasswordValidator(control: AbstractControl): { [key: string]: boolean } | null {
    const form = control as FormGroup
    const password = form.get('password')?.value;
    const confirmPassword = form.get('confirmPassword')?.value;

    if (password && confirmPassword && password != confirmPassword) {
      form.get("confirmPassword")?.setErrors({ passwordMismatch: true });
      return { passwordMismatch: true }
    } else {
      const confirmPasswordControl = form.get("confirmPassword");
      if (confirmPasswordControl?.errors && 'passwordMismatch' in confirmPasswordControl.errors) {
        const errors = { ...confirmPasswordControl.errors };
        delete errors['passwordMismatch'];
        confirmPasswordControl.setErrors(Object.keys(errors).length ? errors : null);
      }
    }

    return null
  }

  returnToForm() {
    this.form.get('username').reset();
    this.form.get('email').reset();
    this.form.get('afm').reset();
    this.form.get('phoneNumber').reset();
    this.registrationStatus = { success: false, message: 'Not attempted yet' }
  }

  onSubmit(value: any) {
    console.log(value);

    const user: UserDoctor = {
      username: this.form.get("username").value,
      email: this.form.get("email").value,
      password: this.form.get("password").value,
      firstname: this.form.get("firstname").value,
      lastname: this.form.get("lastname").value,
      phoneNumber: this.form.get("phoneNumber").value,
      city: this.form.get("city").value,
      address: this.form.get("address").value,
      afm: this.form.get("afm").value,
      userRole: "Doctor",
      doctorSpecialty: this.form.get("doctorSpecialty").value
    }

    console.log(user);

    this.userService.registerDoctor(user).subscribe({
      next: (response) => {
        console.log("No Errors", response)
        this.registrationStatus = { success: true, message: "Επιτυχής καταχώριση ιατρού" }
      },
      error: (response) => {
        console.log("Errors", response)
        let message = response.error.message;
        this.registrationStatus = { success: false, message: message }
      }
    })
  }
}
