import { Component } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, Validators } from '@angular/forms';
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
  }, { validator: this.passwordsMatchValidator.bind(this) });

  errorMessage: string = "";
  isLoading: boolean = false;

  constructor(public dataService: DataService, private router: Router, private fb: FormBuilder) { }

  // Custom validator function to check if passwords match
  passwordsMatchValidator(control: FormGroup) {
    const password = control.get('password')?.value;
    const reEnterPassword = control.get('ReEnterPassword')?.value;

    if (password === reEnterPassword) {
      control.get('ReEnterPassword')?.setErrors(null); // Clear the mismatch error on the control
      return null; // Passwords match
    } else {
      control.get('ReEnterPassword')?.setErrors({ mismatch: true }); // Set the mismatch error on the control
      return { mismatch: true }; // Passwords don't match, return an error object
    }
  }

  RegisterUser() {
    if (this.registerFormGroup.valid) {
      // Rest of your registration logic
    } else {
      // Display an error message for password mismatch (if needed)
      this.errorMessage = 'Passwords do not match.';
    }
  }
}
