import { Injectable, inject, signal, effect } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { Credentials, DoctorsSearchPage, FiltersForm, PatientsPage } from '../interfaces/backend';
import { LoggedInUser } from '../interfaces/backend';
import { Router } from '@angular/router';
import { jwtDecode } from 'jwt-decode';
import { UserPatient } from '../interfaces/backend';
import { UserDoctor } from '../interfaces/backend';
import { User } from '../interfaces/backend';

const API_URL = `${environment.apiURL}/api`

@Injectable({
  providedIn: 'root'
})
export class UserService {
  http: HttpClient = inject(HttpClient)
  router = inject(Router)
  user = signal<LoggedInUser | null>(null)

  specialtyMapping: { [key: string]: string } = {

    "Cardiologist": "ΚΑΡΔΙΟΛΟΓΟΣ",
    "Angiologist": "ΑΓΓΕΙΟΛΟΓΟΣ",
    "Pulmonologist": "ΠΝΕΥΜΟΝΟΛΟΓΟΣ",
    "Nephrologist": "ΝΕΦΡΟΛΟΓΟΣ",
    "Pathologist": "ΠΑΘΟΛΟΓΟΣ",
    "Surgeon": "ΓΕΝΙΚΟΣ ΧΕΙΡΟΥΡΓΟΣ"
  };

  constructor() {
    const access_token = localStorage.getItem("access_token")
    if (access_token) {
      const decodedToken: any = jwtDecode(access_token)

      this.user.set({
        name: decodedToken['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'],
        id: decodedToken['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'],
        email: decodedToken['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress'],
        role: decodedToken['http://schemas.microsoft.com/ws/2008/06/identity/claims/role']
      })
    }
    effect(() => {
      if (this.user()) {
        console.log("User logged in: ", this.user()?.name);
      } else {
        console.log('No user logged in');
      }
    })
  }

  registerPatient(user: UserPatient) {
    return this.http.post<UserPatient>(`${API_URL}/users/patients`, user)
  }

  registerDoctor(user: UserDoctor) {
    return this.http.post<UserDoctor>(`${API_URL}/users/doctors`, user)
  }

  loginUser(credentials: Credentials) {
    return this.http.post<{ token: string }>(`${API_URL}/users/login`, credentials)
  }

  getPatient(id: string) {
    return this.http.get<UserPatient>(`${API_URL}/patients/user/${id}`)
  }

  getDoctor(id: string) {
    return this.http.get<UserDoctor>(`${API_URL}/doctors/user/${id}`)
  }

  updatePatient(id: string, user: User) {
    return this.http.patch<UserPatient>(`${API_URL}/patients/user/${id}`, user)
  }

  updateDoctor(id: string, user: User) {
    return this.http.patch<UserDoctor>(`${API_URL}/doctors/user/${id}`, user)
  }

  addDoctorToPatient(patientId: string, doctorId: number) {
    return this.http.post<string>(`${API_URL}/patients/${patientId}/doctors/${doctorId}`, null)
  }

  deleteDoctorFromPatient(patientId: string, doctorId: number) {
    return this.http.delete<string>(`${API_URL}/patients/${patientId}/doctors/${doctorId}`)
  }

  deletePatientFromDoctor(doctorId: string, patientId: number) {
    return this.http.delete<string>(`${API_URL}/doctors/${doctorId}/patients/${patientId}`)
  }

  getFilteredDoctorsPage(filters: FiltersForm, pageNumber: number, pageSize: number) {
    const params = new HttpParams()
      .set('pageNumber', pageNumber.toString())
      .set('pageSize', pageSize.toString());

    return this.http.post<DoctorsSearchPage>(`${API_URL}/doctors`, filters, { params })
  }

  getPatientDoctorsPage(patientId: string, pageNumber: number, pageSize: number) {
    const params = new HttpParams()
      .set('pageNumber', pageNumber.toString())
      .set('pageSize', pageSize.toString());

    return this.http.get<DoctorsSearchPage>(`${API_URL}/patients/${patientId}/doctors`, { params })
  }

  getDoctorPatientsPage(doctorId: string, pageNumber: number, pageSize: number) {
    const params = new HttpParams()
      .set('pageNumber', pageNumber.toString())
      .set('pageSize', pageSize.toString());

    return this.http.get<PatientsPage>(`${API_URL}/doctors/${doctorId}/patients`, { params })
  }

  logoutUser() {
    this.user.set(null);
    localStorage.removeItem('access_token');
    this.router.navigate(['login'])
  }
}
