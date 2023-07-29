import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { DataService } from 'src/app/services/data.service';
import { Admin } from 'src/app/shared/admin';

@Component({
  selector: 'app-edit-admin',
  templateUrl: './edit-admin.component.html',
  styleUrls: ['./edit-admin.component.scss']
})
export class EditAdminComponent implements OnInit {

  adminDetails: Admin =
  {
    admin_ID:"",
    name:"",
    lastname:"",
    email:"",
    password:"",
    
  };

  constructor(private route: ActivatedRoute, private dataService: DataService, private router:Router) { }

  ngOnInit(): void {
    this.route.params.subscribe({
      next: (params) => {
        console.log(params)
          this.dataService.GetAdmin(params['admin_ID']).subscribe({
            next: (response) => {
              this.adminDetails = response;
            }
          })

      }
    })

    this.dataService.revertToLogin();
  }

  EditAdmin()
  {    
    this.dataService.EditAdmin(this.adminDetails.admin_ID, this.adminDetails).subscribe({
      next: (response) => {this.router.navigate(['/Admin-Screen/admins'])}
    })
    console.log('yes')
  }
}
