import { Component, inject } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Credentials } from '../../shared/interfaces/backend';
import { LoggedInUser } from '../../shared/interfaces/backend';
import { UserService } from '../../shared/services/user.service';
import { Router, RouterLink } from '@angular/router';
import { jwtDecode } from 'jwt-decode';

@Component({
  selector: 'app-user-login',
  standalone: true,
  imports: [ReactiveFormsModule, RouterLink],
  templateUrl: './user-login.component.html',
  styleUrl: './user-login.component.css'
})
export class UserLoginComponent {

  userService = inject(UserService);
  router = inject(Router);

  invalidLogin = false;

  form = new FormGroup({
    username: new FormControl('', [Validators.required, Validators.minLength(2), Validators.maxLength(50)]),
    password: new FormControl('', [Validators.required, Validators.pattern(/^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?\d)(?=.*?\W).{8,20}$/)])
  })

  onSubmit() {
    const credentials = this.form.value as Credentials;
    this.userService.loginUser(credentials).subscribe({
      next: (response) => {
        const access_token = response.token
        console.log(access_token);
        localStorage.setItem("access_token", access_token);

        const decodedToken: any = jwtDecode(access_token);
        console.log(decodedToken);

        this.userService.user.set({
          name: decodedToken['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'],
          id: decodedToken['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'],
          email: decodedToken['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress'],
          role: decodedToken['http://schemas.microsoft.com/ws/2008/06/identity/claims/role']
        })
        this.router.navigate(['dashboard/profile'])
      },
      error: (error) => {
        console.log('Login Error', error);
        this.invalidLogin = true;
      }
    })
  }
}
