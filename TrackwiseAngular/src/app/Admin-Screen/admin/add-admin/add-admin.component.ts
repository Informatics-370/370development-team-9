import { Component } from '@angular/core';
import { DataService } from 'src/app/services/data.service';
import { Router } from '@angular/router';
import { Admin } from 'src/app/shared/admin';

@Component({
  selector: 'app-add-admin',
  templateUrl: './add-admin.component.html',
  styleUrls: ['./add-admin.component.scss']
})
export class AddAdminComponent {

  adminDetails: Admin =
  {
    admin_ID:0,
    name:"",
    lastname:"",
    email:"",
    password:"",
   
  };

  constructor(private dataService: DataService, private router:Router) { }

  ngOnInit(): void {
  }

  AddAdmin()
  {
    this.dataService.AddAdmin(this.adminDetails).subscribe({
      next: (admin) => {this.router.navigate(['/Admin-Screen/admin'])}
    })
  }
}
