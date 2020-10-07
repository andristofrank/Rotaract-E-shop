import { Component, OnInit } from '@angular/core';
declare var $: any;
@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {

  constructor() { }

  ngOnInit(): void {
    $('#sidebarCollapse').on('click', () => {
      $('#sidebar').toggleClass('active');
    });
    $('#sidebarCollapse ul li').on('click', () => {
      $('#sidebar').toggleClass('active');
    })
  }

}
