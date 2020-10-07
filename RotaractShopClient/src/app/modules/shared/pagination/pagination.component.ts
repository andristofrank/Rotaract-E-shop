import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';

@Component({
  selector: 'app-pagination',
  templateUrl: './pagination.component.html',
  styleUrls: ['./pagination.component.css']
})
export class PaginationComponent implements OnInit {
@Input() pageSize;
@Input() page;
@Input() totalPages;

@Output() pageChanged = new EventEmitter<number>();
@Output() pageSizeChanged = new EventEmitter<number>();
  constructor() { }

  ngOnInit(): void {
  }
pageChange(page) {
    this.pageChanged.emit(page);
}
pageSizeChange(pageSize) {
    this.pageSizeChanged.emit(pageSize);
}
}
