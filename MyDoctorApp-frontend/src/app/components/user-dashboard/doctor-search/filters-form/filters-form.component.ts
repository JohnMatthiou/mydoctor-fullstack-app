import { Component, EventEmitter, Output } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { MatSelectModule } from '@angular/material/select';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatButtonModule } from '@angular/material/button';
import { FiltersForm } from '../../../../shared/interfaces/backend';

@Component({
  selector: 'app-filters-form',
  standalone: true,
  imports: [ReactiveFormsModule, MatFormFieldModule,
    MatSelectModule,
    MatInputModule,
    MatButtonModule],
  templateUrl: './filters-form.component.html',
  styleUrl: './filters-form.component.css'
})
export class FiltersFormComponent {
  @Output() filters = new EventEmitter<FiltersForm>()

  form = new FormGroup({
    lastname: new FormControl(''),
    city: new FormControl(''),
    specialty: new FormControl('')
  })

  onSubmit(value: any) {
    console.log(value);
    this.filters.emit(value as FiltersForm)
    this.form.reset();
  }

}
