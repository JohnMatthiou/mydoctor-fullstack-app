import { Component, Input, Inject, inject, Output, EventEmitter } from '@angular/core';
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
  selector: 'app-patient-doctors-datatable',
  standalone: true,
  imports: [DialogModule],
  templateUrl: './patient-doctors-datatable.component.html',
  styleUrl: './patient-doctors-datatable.component.css'
})
export class PatientDoctorsDatatableComponent {
  @Input() doctors: UserDoctor[]
  @Output() dataChanged = new EventEmitter<any>();
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

  openDeleteDialog(doctorId: number) {
    const dialogRef = this.dialog.open(DeleteConfirmationDialog, {
      data: {
        message: `Διαγραφή ιατρού από τους ιατρούς μου;`
      }
    });

    dialogRef.closed.subscribe((result: string) => {
      if (result === 'confirmed') {
        this.deleteDoctorFromPatient(doctorId);
      }
    });
  }


  deleteDoctorFromPatient(doctorId: number) {
    this.userService.deleteDoctorFromPatient(this.user().id, doctorId).subscribe({
      next: (response) => {
        console.log("No Errors", response)
        const dialogRef = this.dialog.open(DeleteResultDialog, {
          data: {
            message: "Επιτυχής διαγραφή ιατρού"
          }
        });

        dialogRef.closed.subscribe(() => {
          this.dataChanged.emit();
        });
      },
      error: (response) => {
        console.log("Errors", response)
        let errorMessage = response.error.message
        this.dialog.open(DeleteResultDialog, {
          data: {
            message: "Σφάλμα κατά την διαγραφή ιατρού",
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
export class DeleteConfirmationDialog {
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
export class DeleteResultDialog {
  constructor(
    public dialogRef: DialogRef<null>,
    @Inject(DIALOG_DATA) public data: any
  ) { }

}
