export interface Credentials {
    username: string,
    password: string
}

export interface UserPatient {
    id?: number,
    username: string,
    email: string,
    password: string,
    firstname: string,
    lastname: string,
    phoneNumber: string,
    city: string,
    address: string,
    amka: string,
    userRole: "Patient"
}

export interface UserDoctor {
    id?: number,
    username: string,
    email: string,
    password: string,
    firstname: string,
    lastname: string,
    phoneNumber: string,
    city: string,
    address: string,
    afm: string,
    userRole: "Doctor",
    doctorSpecialty: string
}

export interface User {
    id: string,
    username: string,
    email: string,
    firstname: string,
    lastname: string,
    phoneNumber: string,
    city: string,
    address: string
}

export interface LoggedInUser {
    name: string,
    id: string,
    email: string,
    role: string
}

export interface FiltersForm {
    lastname: string,
    city: string,
    specialty: string
}

export interface DoctorInfo {
    firstname: string,
    lastname: string,
    doctorSpecialty: string,
    city: string,
    address: string,
    phoneNumber: string
}

export interface DoctorsSearchPage {
    data: UserDoctor[],
    totalRecords: number,
    pageNumber: number,
    pageSize: number,
    totalPages: number
}

export interface PatientsPage {
    data: UserPatient[],
    totalRecords: number,
    pageNumber: number,
    pageSize: number,
    totalPages: number
}

