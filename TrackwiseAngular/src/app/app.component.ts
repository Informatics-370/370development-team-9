import { AfterViewInit, Component, AfterContentChecked,ViewChild, AfterContentInit, OnInit } from '@angular/core';
import { MatSidenav } from '@angular/material/sidenav';
import { Router } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements AfterContentChecked{
  title: any;
  isLoggedIn = false;
  isAdmin = false;
  isCustomer = false;
  constructor(private router: Router) {}
  
  @ViewChild('sidenav', {static:true}) sidenav!: MatSidenav;


  toggleSidenav(){
    this.sidenav.toggle();
  }

  ngAfterContentChecked(): void {
    this.getRole();
  }
  
  getRole(): void {
    var role = JSON.parse(localStorage.getItem("Role")!)
    console.log('Role:', role); // Add this line
    if (role == "Admin") {
      this.isLoggedIn = true;
      this.isAdmin = true;
      console.log('Admin',this.isAdmin);
    } else if(role == "Customer"){
      this.isLoggedIn = true;
      this.isCustomer = true;
      console.log('Customer',this.isCustomer);
    }
  } 
 
    logout(){
    if(localStorage.getItem('User'))
    {
      this.isLoggedIn = false;
      this.isCustomer = false;
      this.isAdmin = false;
      localStorage.removeItem('User');
      localStorage.removeItem('Role');
      this.router.navigateByUrl('Authentication/login');
    }
  } 

}
