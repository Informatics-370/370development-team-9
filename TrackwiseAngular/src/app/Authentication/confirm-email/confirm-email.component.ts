import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute, Router } from '@angular/router';
import { DataService } from 'src/app/services/data.service';
import { ConfirmEmail } from 'src/app/shared/confirmEmail';

@Component({
  selector: 'app-confirm-email',
  templateUrl: './confirm-email.component.html',
  styleUrls: ['./confirm-email.component.scss']
})
export class ConfirmEmailComponent {

  confirmDetails: ConfirmEmail =
  {
    token: '',
    email: ''
  };

  isLoading:boolean = false
  errorMessage: string = '';

  constructor(private router: Router, private dataService: DataService, private fb: FormBuilder, private snackBar: MatSnackBar, private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.route.params.subscribe({
      next: (params) => {
        this.confirmDetails.token = params['token'].replace('%2F', '/');
        this.confirmDetails.email = params['email'];
        console.log(params);
        this.dataService.ConfirmEmail(this.confirmDetails).subscribe({
          next: (response) => {
            // Handle the confirmation
          }
        });
      }
    });
  }

  RedirectToLogin(){
    this.dataService.revertToLogin();
  }
}
