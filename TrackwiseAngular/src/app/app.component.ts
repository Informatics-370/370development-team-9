import { AfterViewInit, Component, AfterContentChecked,ViewChild, AfterContentInit, OnInit } from '@angular/core';
import { MatSidenav } from '@angular/material/sidenav';
import { ActivatedRoute, Router } from '@angular/router';
import { DataService } from './services/data.service';
import { Data } from 'popper.js';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent implements AfterContentChecked{
  title: any;

  constructor(private router: Router, public dataService: DataService, private route: ActivatedRoute) {}
  
  @ViewChild('sidenav', {static:true}) sidenav!: MatSidenav;

  ngOnInit(): void{
    this.dataService.calculateQuantity();
  }

  isLoginPageOrRegisterPage(): boolean {
    const currentRoute = this.router.url;
    // Check if the current route is either login or register
    return currentRoute.includes('/Authentication/login') || currentRoute.includes('/Authentication/register');
  }

  toggleSidenav(){
    this.sidenav.toggle();
  }

  ngAfterContentChecked(): void {
    this.dataService.getRole();
  }
  

  isAdminScreen(): boolean {
    return this.router.url.includes('/Admin-Screen');
  }

  isCustomerScreen(): boolean {
    return this.router.url.includes('/Customer-Screen');
  }

}
