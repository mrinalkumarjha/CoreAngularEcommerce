import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

@Component({
  selector: 'app-pager',
  templateUrl: './pager.component.html',
  styleUrls: ['./pager.component.scss']
})
export class PagerComponent implements OnInit {

  @Input() totalCount: number;
  @Input() pageSize: number;
  @Input() pageNumber: number;
  @Output() PageChanged = new EventEmitter<number>(); // for emiting event from child comp to parent


  constructor() { }

  ngOnInit(): void {
  }

  onPagerChange(event: any) {
    this.PageChanged.emit(event.page); // .page comes from paging comp
  }

}
