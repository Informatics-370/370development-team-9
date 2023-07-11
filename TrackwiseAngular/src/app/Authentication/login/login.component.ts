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

  constructor(private router: Router, private dataService: DataService, private fb: FormBuilder, private snackBar: MatSnackBar) { }

  ngOnInit(): void {
  }

  async LoginUser(){
    if(this.loginFormGroup.valid)
    {
      this.isLoading = true

      await this.dataService.LoginUser(this.loginFormGroup.value).subscribe(result => {
        sessionStorage.setItem('User', JSON.stringify(result.token.value.user));
        sessionStorage.setItem('Token', result.token.value.token);
        const role = result.role;
        sessionStorage.setItem('Role', JSON.stringify(role));
        this.loginFormGroup.reset();

        if(role == "Admin")
        {
          this.router.navigateByUrl('Admin-Screen/admins')
        } else if(role == "Customer")
        {
          this.router.navigateByUrl('Customer-Screen/customer-landing-page')
        }

      })
    }
  }
}
