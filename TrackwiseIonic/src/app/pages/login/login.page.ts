import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, FormsModule, Validators, ReactiveFormsModule } from '@angular/forms';
import { IonicModule } from '@ionic/angular';
import { Router } from '@angular/router';
import { DataserviceService } from 'src/app/services/dataservice.service';
import { HttpClient, HttpClientModule } from '@angular/common/http';

@Component({
  selector: 'app-login',
  templateUrl: './login.page.html',
  styleUrls: ['./login.page.scss'],
  standalone: true,
  imports: [IonicModule, CommonModule, FormsModule, ReactiveFormsModule,HttpClientModule],
  providers:[DataserviceService],
})
export class LoginPage {
  loginFormGroup: FormGroup = this.fb.group({
    emailaddress: ['', [Validators.required, Validators.email]],
    password: ['', Validators.required],
  })

  isLoading:boolean = false;
  errorMessage: string = '';

  constructor(private http: HttpClient, private router: Router, private dataservice : DataserviceService, private fb: FormBuilder) {}

  Route(){
    this.router.navigate(['/forgotpassword']);
  }

  async LoginUser() {
    if (this.loginFormGroup.valid) {
      this.isLoading = true;
  
      await this.dataservice.LoginUser(this.loginFormGroup.value).subscribe(
        (result) => {
          // Handle successful login
          sessionStorage.setItem('User', JSON.stringify(result.token.value.user));
          sessionStorage.setItem('Token', result.token.value.token);
          const role = result.role;
          console.log(role);
          sessionStorage.setItem('Role', JSON.stringify(role));
          this.loginFormGroup.reset();
  
          if (role == "Driver") {
            this.router.navigate(['/tabs']);
          } 
        },
        (error) => {
          // Handle login error
          if (error.status === 404) {
            this.errorMessage = 'Incorrect email or password. Please try again.';
          } else {
            this.errorMessage = 'An error occurred. Please try again later.';
          }
          this.isLoading = false; // Stop loading spinner
        }
      );
    }
  }

}



