import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { DataService } from 'src/app/services/data.service';

@Component({
  selector: 'app-forgotpassword',
  templateUrl: './forgotpassword.component.html',
  styleUrls: ['./forgotpassword.component.scss']
})
export class ForgotpasswordComponent {
  email: string = '';
  message: string = '';

  constructor(private dataService: DataService) { }

  onSubmit() {
    this.dataService.forgotPassword(this.email).subscribe(
      () => {
        this.message = "Password reset email sent. Please check your inbox.";
      },
      (error) => {
        this.message = "Error: Failed to send the password reset email.";
      }
    );
  }

}
