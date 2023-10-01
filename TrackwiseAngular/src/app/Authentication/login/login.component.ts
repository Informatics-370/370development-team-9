import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';
import { DataService } from 'src/app/services/data.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {
  loginFormGroup: FormGroup = this.fb.group({
    emailaddress: ['', [Validators.required, Validators.email]],
    password: ['', Validators.required],
  })

  isLoading:boolean = false
  errorMessage: string = '';

  constructor(private router: Router, public dataService: DataService, private fb: FormBuilder, private snackBar: MatSnackBar) { }

  ngOnInit(): void {
  }

  async LoginUser() {
    if (this.loginFormGroup.valid) {
      this.isLoading = true;
  
      await this.dataService.LoginUser(this.loginFormGroup.value).subscribe(
        async (result) => {
          console.log(result)
          // Handle successful login
          if(result.isEmailConfirmed == false){
            this.isLoading = false;
            this.errorMessage = 'Please confirm your email address.';
          }
          else if(result.isTwoFactor == true)
          {
            this.router.navigateByUrl('Authentication/two-factor-auth');
            const user = result.token.value.user;
            sessionStorage.setItem('User', JSON.stringify(user));
            sessionStorage.setItem('OTPtime', JSON.stringify(result.expireOTPtime));
          } else{
          sessionStorage.setItem('User', JSON.stringify(result.token.value.user));
          sessionStorage.setItem('Token', result.token.value.token);
          const role = result.role;
          sessionStorage.setItem('Role', JSON.stringify(role));
          await this.dataService.GetUserName()
          this.loginFormGroup.reset();
  
          if (role == "Admin") {
            this.router.navigateByUrl('Admin-Screen/admin-home');
          } else if (role == "Customer") {
            this.router.navigateByUrl('Customer-Screen/customer-products');
          } else if (role == "Client") {
            this.router.navigateByUrl('Client-Screen/client-jobs');
          } else {
            this.router.navigateByUrl('Customer-Screen/customer-products');
          }
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
