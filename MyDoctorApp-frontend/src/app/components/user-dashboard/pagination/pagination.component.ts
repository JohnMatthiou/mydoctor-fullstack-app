import { Component, Input, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-pagination',
  standalone: true,
  imports: [],
  templateUrl: './pagination.component.html',
  styleUrl: './pagination.component.css'
})
export class PaginationComponent {
  @Output() pageNumber = new EventEmitter<number>()
  @Input() currentPage: number = 1; 
  @Input() totalPages: number = 1;

  goToPage(value: number) {
      this.pageNumber.emit(value)
    }
}
