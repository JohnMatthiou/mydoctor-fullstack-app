import { Component, Input, Inject, inject } from '@angular/core';
import { DoctorInfo, UserDoctor } from '../../../../shared/interfaces/backend';
import {
  Dialog,
  DialogRef,
  DIALOG_DATA,
  DialogModule
} from '@angular/cdk/dialog';
import { DoctorInfoTableComponent } from '../../doctor-info-table/doctor-info-table.component';
import { UserService } from '../../../../shared/services/user.service';

@Component({
  selector: 'app-doctors-datatable',
  standalone: true,
  imports: [DialogModule],
  templateUrl: './doctors-datatable.component.html',
  styleUrl: './doctors-datatable.component.css'
})
export class DoctorsDatatableComponent {
  @Input() doctors: UserDoctor[]
  userService = inject(UserService);
  user = this.userService.user;

  constructor(public dialog: Dialog) { }

  openInfoDialog(doctor: UserDoctor) {
    this.dialog.open(DoctorInfoDialog, {
      data: {
        firstname: doctor.firstname,
        lastname: doctor.lastname,
        doctorSpecialty: doctor.doctorSpecialty,
        city: doctor.city,
        address: doctor.address,
        phoneNumber: doctor.phoneNumber
      }
    });
  }

  openAddDialog(doctorId: number) {
    const dialogRef = this.dialog.open(AddConfirmationDialog, {
      data: {
        message: `Προσθήκη ιατρού στους ιατρούς μου;`
      }
    });

    dialogRef.closed.subscribe((result: string) => {
      if (result === 'confirmed') {
        this.addDoctorToPatient(doctorId);
      }
    });
  }

  addDoctorToPatient(doctorId: number) {
    this.userService.addDoctorToPatient(this.user().id, doctorId).subscribe({
      next: (response) => {
        console.log("No Errors", response)
        this.dialog.open(AddResultDialog, {
          data: {
            message: "Επιτυχής προσθήκη ιατρού"
          }
        })
      },
      error: (response) => {
        console.log("Errors", response)
        let errorMessage = response.error.message
        if (response.error.message.includes('already added to my doctors')) {
          errorMessage = "Ο ιατρός υπάρχει ήδη στους ιατρούς μου"
        }
        this.dialog.open(AddResultDialog, {
          data: {
            message: "Σφάλμα κατά την προσθήκη ιατρού",
            backendMessage: errorMessage
          }
        })
      }
    })

  }


}

@Component({
  imports: [DoctorInfoTableComponent],
  standalone: true,
  template: `
    <app-doctor-info-table [doctorInfo]="data"></app-doctor-info-table>
    <button class="btn btn-primary btn-sm" (click)="dialogRef.close()">Κλείσιμο</button>
  `,
  styles: [
    `
    :host{
      display: block;
      background: #fff;
      border-radius: 8px;
      padding: 16px;
      max-width: 500px;
    }
    `
  ]
})
export class DoctorInfoDialog {
  constructor(
    public dialogRef: DialogRef<null>,
    @Inject(DIALOG_DATA) public data: DoctorInfo
  ) { }
}


@Component({
  template: `
    <div class="modal-body">
    <p>{{ data.message }}</p>
    </div>

    <div class="modal-footer">
    <button class="btn btn-primary me-2" (click)="onConfirm()">ΟΚ</button>
    <button class="btn btn-secondary" (click)="onCancel()">Ακύρωση</button>
    </div>
  `,
  styles: [
    `
    :host{
      display: block;
      background: #fff;
      border-radius: 8px;
      padding: 16px;
      max-width: 500px;
    }
    `
  ]
})
export class AddConfirmationDialog {
  constructor(
    public dialogRef: DialogRef<string>,
    @Inject(DIALOG_DATA) public data: any
  ) { }

  onCancel() {
    this.dialogRef.close('cancelled');
  }

  onConfirm() {
    this.dialogRef.close('confirmed');
  }
}

@Component({
  template: `
    <div class="modal-body">
    <p>{{ data.message }}</p>
    <p>{{ data.backendMessage }}</p>
    </div>
    <div class="modal-footer">
    <button class="btn btn-primary btn-sm" (click)="dialogRef.close()">Κλείσιμο</button>
    </div>
  `,
  styles: [
    `
    :host{
      display: block;
      background: #fff;
      border-radius: 8px;
      padding: 16px;
      max-width: 500px;
    }
    `
  ]
})
export class AddResultDialog {
  constructor(
    public dialogRef: DialogRef<null>,
    @Inject(DIALOG_DATA) public data: any
  ) { }

}



