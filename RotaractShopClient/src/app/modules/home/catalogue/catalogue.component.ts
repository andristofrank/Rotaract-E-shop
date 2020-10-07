import { Component, OnInit } from '@angular/core';
import {ActivatedRoute} from "@angular/router";

@Component({
  selector: 'app-home',
  templateUrl: './catalogue.component.html',
  styleUrls: ['./catalogue.component.css']
})
export class CatalogueComponent implements OnInit {
products;
donationPrograms;
  constructor(private activatedRoute: ActivatedRoute) {
  this.activatedRoute.data.subscribe(
    response =>{
      this.donationPrograms = response.donationPrograms;
      this.products = {
        body:  response.products.body,
        totalElements: JSON.parse(response.products.headers.get('x-pagination')).totalPages};
    }
  )
  }

  ngOnInit(): void {
  }
}
