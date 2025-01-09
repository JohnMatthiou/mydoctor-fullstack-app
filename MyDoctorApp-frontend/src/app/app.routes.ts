import { Routes } from '@angular/router';
import { WelcomeComponent } from './components/welcome/welcome.component';
import { SignupComponent } from './components/signup/signup.component';
import { UserLoginComponent } from './components/user-login/user-login.component';
import { PatientSignupComponent } from './components/patient-signup/patient-signup.component';
import { DoctorSignupComponent } from './components/doctor-signup/doctor-signup.component';
import { UserProfileComponent } from './components/user-dashboard/user-profile/user-profile.component';
import { UserUpdateComponent } from './components/user-dashboard/user-update/user-update.component';
import { DoctorSearchComponent } from './components/user-dashboard/doctor-search/doctor-search.component';
import { PatientDoctorsComponent } from './components/user-dashboard/patient-doctors/patient-doctors.component';
import { DoctorPatientsComponent } from './components/user-dashboard/doctor-patients/doctor-patients.component';
import { loggedInUserGuard } from './shared/guards/logged-in-user.guard';
import { authGuard } from './shared/guards/auth.guard';


export const routes: Routes = [
    {path: 'welcome', component: WelcomeComponent, canActivate:[loggedInUserGuard]},
    {path: 'signup', component: SignupComponent, canActivate:[loggedInUserGuard]},
    {path: 'login', component: UserLoginComponent, canActivate:[loggedInUserGuard]},
    {path: 'patient-signup', component: PatientSignupComponent, canActivate:[loggedInUserGuard]},
    {path: 'doctor-signup', component: DoctorSignupComponent, canActivate:[loggedInUserGuard]},
    {path: 'dashboard/profile', component: UserProfileComponent, canActivate:[authGuard]},
    {path: 'dashboard/user-update', component: UserUpdateComponent, canActivate:[authGuard]},
    {path: 'dashboard/doctor-search', component: DoctorSearchComponent, canActivate:[authGuard]},
    {path: 'dashboard/patient-doctors', component: PatientDoctorsComponent, canActivate:[authGuard]},
    {path: 'dashboard/doctor-patients', component: DoctorPatientsComponent, canActivate:[authGuard]},
    {path: '', redirectTo:'/welcome', pathMatch:'full'}
];
