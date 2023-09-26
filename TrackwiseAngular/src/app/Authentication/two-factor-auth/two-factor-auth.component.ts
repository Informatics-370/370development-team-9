import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';
import { DataService } from 'src/app/services/data.service';
import { TwoFactor } from 'src/app/shared/twoFactor';

@Component({
  selector: 'app-two-factor-auth',
  templateUrl: './two-factor-auth.component.html',
  styleUrls: ['./two-factor-auth.component.scss']
})
export class TwoFactorAuthComponent {

 twoFactorFormGroup: FormGroup = this.fb.group({
    username: JSON.parse(localStorage.getItem("User")!),
    code: ['', Validators.required],
  })

  adminDetails: TwoFactor =
  {
    code: '',
    username: ''
  };

  isLoading:boolean = false
  errorMessage: string = '';

  constructor(private router: Router, private dataService: DataService, private fb: FormBuilder, private snackBar: MatSnackBar) { }

  ngOnInit(): void {
  }

  TwoFactorAuth() {
    const role = JSON.parse(localStorage.getItem("Role")!);
  
    // Create an observable for the ConfirmTwoFactor call
    const confirmTwoFactor$ = this.dataService.ConfirmTwoFactor(this.twoFactorFormGroup.value);
  
    // Subscribe to the observable
    confirmTwoFactor$.subscribe(
      (result) => {
        sessionStorage.setItem('User', JSON.stringify(result.token.value.user));
        sessionStorage.setItem('Token', result.token.value.token);
        sessionStorage.setItem('Role', JSON.stringify(role));
        this.twoFactorFormGroup.reset();
        localStorage.removeItem("User");
        localStorage.removeItem('Role');
  
        if (role == "Admin") {
          this.router.navigateByUrl('Admin-Screen/admin-home');
        } else if (role == "Client") {
          this.router.navigateByUrl('Client-Screen/client-jobs');
        } else {
          this.router.navigateByUrl('Customer-Screen/customer-products');
        }
      },
      (error) => {
        // Handle login error
        if (error.status === 404) {
          this.errorMessage = 'Invalid OTP';
        } else {
          this.errorMessage = 'An error occurred. Please try again later.';
        }
        this.isLoading = false; // Stop loading spinner
      }
    );
  }
  
}
