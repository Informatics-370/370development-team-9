import { Component, OnDestroy, OnInit } from '@angular/core';
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
export class TwoFactorAuthComponent implements OnInit, OnDestroy{

 twoFactorFormGroup: FormGroup = this.fb.group({
    username: JSON.parse(sessionStorage.getItem("User")!),
    code: ['', Validators.required],
  })

  adminDetails: TwoFactor =
  {
    code: '',
    username: ''
  };

  isLoading:boolean = false
  errorMessage: string = '';
  remainingTime: number = 0; // Initialize remaining time
  timerInterval: any;
  expirationTime!: Date;
  expireTime: any;

  constructor(private router: Router, private dataService: DataService, private fb: FormBuilder, private snackBar: MatSnackBar) { }


  ngOnInit(): void {
    this.remainingTime = 0;
    // Retrieve the expiration time from sessionStorage
    const storedExpirationTime = new Date(JSON.parse(sessionStorage.getItem('OTPtime')!)).getTime();

    // Get the current time
    const currentTime = new Date().getTime();

    // Calculate the remaining time in milliseconds
    this.remainingTime = Math.floor((storedExpirationTime - currentTime) / 1000);
    if(this.remainingTime <= 0)
    {
      this.remainingTime = 0
    }
    this.startTimer(); // Start the timer when the component initializes
  }

  ngOnDestroy(): void {
    this.stopTimer(); // Stop the timer when the component is destroyed
  }

  startTimer() {
    this.timerInterval = setInterval(() => {
      if (this.remainingTime > 0) {
        this.remainingTime--;
      } else {
        this.stopTimer();
        // Handle timer expiration (e.g., show a message, allow resending OTP).
        this.errorMessage = 'Time has expired. Please resend OTP if needed.';
      }
    }, 1000); // Update every 1 second
  }

  stopTimer() {
    clearInterval(this.timerInterval);
  }

  formatTime(seconds: number): string {
    const minutes = Math.floor(seconds / 60);
    const remainingSeconds = seconds % 60;
    return `${minutes}:${remainingSeconds < 10 ? '0' : ''}${remainingSeconds}`;
  }

  async ResendTwoFactor() {
    this.stopTimer()
    await this.dataService.ResendTwoFactor(this.twoFactorFormGroup.value.username).subscribe(
      (result) => {
        const user = result.token.value.user;
        sessionStorage.setItem('User', JSON.stringify(user));
        sessionStorage.setItem('OTPtime', JSON.stringify(result.expireOTPtime));

        this.remainingTime = 0;
        // Retrieve the expiration time from sessionStorage
        const storedExpirationTime = new Date(JSON.parse(sessionStorage.getItem('OTPtime')!)).getTime();
    
        // Get the current time
        const currentTime = new Date().getTime();
    
        // Calculate the remaining time in milliseconds
        this.remainingTime = Math.floor((storedExpirationTime - currentTime) / 1000);
        if(this.remainingTime <= 0)
        {
          this.remainingTime = 0
        }
        this.startTimer(); // Start the timer when the component initializes
      })
      
  }

  TwoFactorAuth() {
  
    // Create an observable for the ConfirmTwoFactor call
    const confirmTwoFactor$ = this.dataService.ConfirmTwoFactor(this.twoFactorFormGroup.value);
  
    // Subscribe to the observable
    confirmTwoFactor$.subscribe(
      async (result) => {
        sessionStorage.setItem('User', JSON.stringify(result.token.value.user));
        sessionStorage.setItem('Token', result.token.value.token);
        let role = result.role;
        sessionStorage.setItem('Role', JSON.stringify(role));
        await this.dataService.GetUserName()
        this.twoFactorFormGroup.reset();

        sessionStorage.removeItem('OTPtime');

  
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
