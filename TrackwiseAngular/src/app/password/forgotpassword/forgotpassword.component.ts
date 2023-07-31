import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute, Router } from '@angular/router';
import { DataService } from 'src/app/services/data.service';

@Component({
  selector: 'app-forgotpassword',
  templateUrl: './forgotpassword.component.html',
  styleUrls: ['./forgotpassword.component.scss']
})
export class ForgotpasswordComponent implements OnInit{
  loginFormGroup: FormGroup = this.fb.group({
    email: ['', [Validators.required, Validators.email]]
  })

  isLoading:boolean = false
  errorMessage: string = '';


  constructor(private router: Router, private dataService: DataService, private fb: FormBuilder, private snackBar: MatSnackBar) { }

  ngOnInit(): void {
  }

  async Forgotpass() {
    if (this.loginFormGroup.valid) {
      this.isLoading = true;
  
      await this.dataService.forgotPassword(this.loginFormGroup.value).subscribe(
        (result) => {
        this.loginFormGroup.reset();
        },
        (error) => {
          // Handle login error
          if (error.status === 404) {
            this.errorMessage = 'Email doesnt exist. Please try again.';
          } else{
            this.snackBar.open(`Reset password email has been sent`, 'X', {duration: 5000});
            this.router.navigateByUrl('Authentication/login');
          }
          this.isLoading = false; // Stop loading spinner
        }
      );
    }
  }

}
