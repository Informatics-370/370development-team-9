import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { IonicModule } from '@ionic/angular';
import { DataserviceService } from 'src/app/services/dataservice.service';
import { Router } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';

@Component({
  selector: 'app-forgotpassword',
  templateUrl: './forgotpassword.page.html',
  styleUrls: ['./forgotpassword.page.scss'],
  standalone: true,
  imports: [IonicModule, CommonModule, FormsModule, HttpClientModule,ReactiveFormsModule],
  providers:[DataserviceService],
})
export class ForgotpasswordPage{
  loginFormGroup: FormGroup = this.fb.group({
    email: ['', [Validators.required, Validators.email]]
  })

  isLoading:boolean = false
  errorMessage: string = '';
  constructor(private router: Router, private dataService: DataserviceService, private fb: FormBuilder) { }

  Forgotpass() {
    if (this.loginFormGroup.valid) {
      this.isLoading = true;
  
      this.dataService.forgotPassword(this.loginFormGroup.value).subscribe(
        (result) => {
          this.isLoading = false;
          console.log('Password reset email sent successfully.');
          this.loginFormGroup.reset();
          this.router.navigate(['/login']);
        },
        (error) => {
          this.router.navigate(['/login']);
          this.isLoading = false;
        }
      );
    }
  }

  Back(){
    this.router.navigate(['/login']);
  }

}
