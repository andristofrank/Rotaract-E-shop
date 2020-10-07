import { Component, OnInit } from '@angular/core';
import {ActivatedRoute, Router} from "@angular/router";

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
user;
  constructor(private activatedRoute: ActivatedRoute,
              private route: Router) {
    this.activatedRoute.data.subscribe(
      response => {
        console.log(response);
        this.user = response.user;
        if( this.user !== undefined) {
          sessionStorage.setItem('account', JSON.stringify(this.user));
        } else {
          sessionStorage.setItem('account', null);
        }
      },
      (err) => {
        sessionStorage.removeItem('key');
        sessionStorage.removeItem('account');
        //window.location.href = 'http://localhost:4200/home/catalogue';
        window.location.href = 'https://rotaract-eshop.web.app/home/catalogue';
      }
      )
  }

  ngOnInit(): void {
  }

}
