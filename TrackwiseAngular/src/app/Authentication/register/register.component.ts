import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { DataService } from 'src/app/services/data.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent {
  registerFormGroup: FormGroup = this.fb.group({
    name: ['', Validators.required],
    lastName: ['', Validators.required],
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required, Validators.pattern(/^(?=.*[A-Z])(?=.*[0-9]).{8,}$/)]],
    ReEnterPassword: ['', Validators.required],
  });

  errorMessage: string = "";
  isLoading: boolean = false;

  constructor(public dataService: DataService, private router: Router, private fb: FormBuilder) { }

  matchPassword(control: AbstractControl) {
    const password = this.registerFormGroup.get('password')?.value;
    const reEnterPassword = control.value;
  
    if (password === reEnterPassword) {
      return null; // Passwords match
    } else {
      return { mismatch: true }; // Passwords don't match, return an error object
    }
  }
  
  // Custom validator function to check if passwords match
  passwordsMatchValidator(control: FormGroup) {
    const password = control.get('password')?.value;
    const reEnterPassword = control.get('ReEnterPassword')?.value;

    if (password === reEnterPassword) {
      return null; // Passwords match
    } else {
      control.get('ReEnterPassword')?.setErrors({ mismatch: true }); // Set the mismatch error on the control
      return { mismatch: true }; // Passwords don't match, return an error object
    }
  }

  RegisterUser() {
    if (this.registerFormGroup.valid && this.registerFormGroup.get('password')?.value === this.registerFormGroup.get('ReEnterPassword')?.value) {
      this.isLoading = true;
      console.log(this.registerFormGroup.value);

      // Remove ReEnterPassword from the request data
      const registerData = { ...this.registerFormGroup.value };
      delete registerData.ReEnterPassword;

      this.dataService.Register(registerData).subscribe(
        (result: any) => {
          this.router.navigateByUrl('Authentication/login');
        },
        (error) => {
          if (error.status === 500) {
            this.errorMessage = 'Account already exists.';
          } else {
            this.errorMessage = 'An error occurred. Please try again later.';
          }
          console.error(error); // Log the error response to the console for debugging
          this.isLoading = false; // Set isLoading to false in case of an error
        }
      );
    } else {
      // Display an error message for password mismatch
      this.errorMessage = 'Passwords do not match.';
    }
  }

  }

