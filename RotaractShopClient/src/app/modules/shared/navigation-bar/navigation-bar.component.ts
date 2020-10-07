import {Input, OnInit} from '@angular/core';
import {Router} from '@angular/router';
import {AuthService} from '../../../services/auth.service';
import {Component} from '@angular/core';
import {NgbModal} from "@ng-bootstrap/ng-bootstrap";

@Component({
  selector: 'app-navigation-bar',
  templateUrl: './navigation-bar.component.html',
  styleUrls: ['./navigation-bar.component.css']
})
export class NavigationBarComponent implements OnInit {
@Input() user;
  modalOptions;
  constructor(private route: Router,
              private authService: AuthService,
              private modalService: NgbModal) {
    this.modalOptions = {
      backdrop: 'static',
      backdropClass: 'customBackdrop',
      size: 'lg'
    }
  }

  ngOnInit(): void {
  }
  logIn() {
    this.route.navigate(['auth/login']);
  }
  logout() {
    this.authService.logout();
  }
  admin() {
    this.route.navigate(['administration']);
  }
  home () {
    this.route.navigate(['home']);
  }
  orders() {
    this.route.navigate(['home/profile/orders']);
  }
  get auth() {
    return this.authService.isAuthenticated;
  }
  openModal(target){
    this.modalService.open(target, this.modalOptions);
  }
}
