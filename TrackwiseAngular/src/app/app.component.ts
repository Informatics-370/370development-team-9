import { AfterViewInit, Component, AfterContentChecked,ViewChild, AfterContentInit, OnInit } from '@angular/core';
import { MatSidenav } from '@angular/material/sidenav';
import { ActivatedRoute, Router } from '@angular/router';
import { DataService } from './services/data.service';
import { Data } from 'popper.js';
import { Customer } from './shared/customer';
import { Admin } from './shared/admin';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent implements AfterContentChecked{
  title: any;
  username:string = ''

  customer : Customer =
  {
    customer_ID: "",
    name: '',
    lastName: "",
    email: ""
  }

  admin : Admin =
    {
      admin_ID: "",
      name: '',
      lastname: "",
      email: "",
      password:""
    }

  constructor(private router: Router, public dataService: DataService, private route: ActivatedRoute) {}
  
  @ViewChild('sidenav', {static:true}) sidenav!: MatSidenav;

  async ngOnInit(): Promise<void> {
    this.dataService.calculateQuantity();
    await this.dataService.GetUserName(); // Wait for GetUserName to complete
    console.log(this.dataService.username);
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
  

  async GetUserName(){
    let role = JSON.parse(sessionStorage.getItem("Role")!)
    if(role == "Customer"){
      await this.dataService.GetCustomerProfile().subscribe((result) => {

        if (result != null)
        {
          this.username = result.name
        }
      
        console.log(result)
      }) 
    } else if (role == "Admin"){
      await this.dataService.GetAdminProfile().subscribe((result) => {

        if (result != null)
        {
          this.username = result.name
        }
      
  
      })
    } else {
      console.log("result")
      this.username = ""
    }

  }


}
