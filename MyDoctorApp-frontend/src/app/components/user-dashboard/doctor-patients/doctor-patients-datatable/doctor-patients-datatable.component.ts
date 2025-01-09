import { Component, Input, Inject, inject, Output, EventEmitter } from '@angular/core';
import {
  Dialog,
  DialogRef,
  DIALOG_DATA,
  DialogModule
} from '@angular/cdk/dialog';
import { UserService } from '../../../../shared/services/user.service';
import { UserPatient } from '../../../../shared/interfaces/backend';


@Component({
  selector: 'app-doctor-patients-datatable',
  standalone: true,
  imports: [DialogModule],
  templateUrl: './doctor-patients-datatable.component.html',
  styleUrl: './doctor-patients-datatable.component.css'
})
export class DoctorPatientsDatatableComponent {
  @Input() patients: UserPatient[]
  @Output() dataChanged = new EventEmitter<any>();
  userService = inject(UserService);
  user = this.userService.user;

  constructor(public dialog: Dialog) { }

  openDeleteDialog(patientId: number) {
    const dialogRef = this.dialog.open(DeleteConfirmationDialog, {
      data: {
        message: `Διαγραφή ασθενή από τους ασθενείς μου;`
      }
    });

    dialogRef.closed.subscribe((result: string) => {
      if (result === 'confirmed') {
        this.deletePatientFromDoctor(patientId);
      }
    });
  }

  deletePatientFromDoctor(patientId: number) {
    this.userService.deletePatientFromDoctor(this.user().id, patientId).subscribe({
      next: (response) => {
        console.log("No Errors", response)
        const dialogRef = this.dialog.open(DeleteResultDialog, {
          data: {
            message: "Επιτυχής διαγραφή ασθενή"
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
            message: "Σφάλμα κατά την διαγραφή ασθενή",
            backendMessage: errorMessage
          }
        })
      }
    })
  }
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