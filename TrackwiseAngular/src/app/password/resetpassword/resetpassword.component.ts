import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { DataService } from 'src/app/services/data.service';

@Component({
  selector: 'app-resetpassword',
  templateUrl: './resetpassword.component.html',
  styleUrls: ['./resetpassword.component.scss']
})

export class ResetpasswordComponent implements OnInit{
  
  email: string = '';
  token: string = '';
  newPassword: string = '';
  confirmPassword: string = '';
  
  constructor(private route: ActivatedRoute, private dataService: DataService, private router:Router) { }

  ngOnInit() {
    this.route.queryParams.subscribe(params => {
      this.email = params['email'];
      this.token = params['token'];
    });
  }

  resetPassword() {
    // Call your user service to reset the password
    this.dataService.resetPassword(this.email, this.token, this.newPassword).subscribe(
      () => {
        // Password reset successful, navigate to a success page or login page
        {this.router.navigate(['/Authentication/login'])}
      },
      (error) => {
        // Handle password reset error (e.g., invalid token, server error, etc.)
      }
    );
  }
}
