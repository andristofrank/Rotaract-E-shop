import { Component, OnInit } from '@angular/core';
import {ActivatedRoute} from "@angular/router";
declare var $: any
@Component({
  selector: 'app-administration',
  templateUrl: './administration.component.html',
  styleUrls: ['./administration.component.css']
})
export class AdministrationComponent implements OnInit {
user;
  constructor(private activatedRoute: ActivatedRoute) {
    this.activatedRoute.data.subscribe(
      response =>{
        this.user = response.user;
      }
    );
  }

  ngOnInit(): void {
    $('#sidebarCollapse').on('click', () => {
      $('#sidebar').toggleClass('active');
    });
    $('#sidebarCollapse ul li').on('click', () => {
      $('#sidebar').toggleClass('active');
    })
  }

}
