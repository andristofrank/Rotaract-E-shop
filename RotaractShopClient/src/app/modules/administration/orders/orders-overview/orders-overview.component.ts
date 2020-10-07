import { Component, OnInit } from '@angular/core';
import {ResourcesService} from "../../../../services/resources.service";
import {Status} from "../../../../../../APIs/models/Status";
import {NgbModal} from "@ng-bootstrap/ng-bootstrap";
import {ActivatedRoute} from "@angular/router";

@Component({
  selector: 'app-orders-overview',
  templateUrl: './orders-overview.component.html',
  styleUrls: ['./orders-overview.component.css']
})
export class OrdersOverviewComponent implements OnInit {
  status = Status;
  active;
  constructor() {
    this.active = 1;
  }

  ngOnInit(): void {
  }

}
