import { Component } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';
import { DataService } from 'src/app/services/data.service';
import { Supplier } from 'src/app/shared/supplier';

@Component({
  selector: 'app-add-supplier',
  templateUrl: './add-supplier.component.html',
  styleUrls: ['./add-supplier.component.scss']
})
export class AddSupplierComponent {

  AddSupplierReq: Supplier =
  {
    supplier_ID: "",
    name: '',
    email:'',
    contact_Number:'',
    password:"",
  };

  constructor(private dataService: DataService, private router:Router, private snackBar: MatSnackBar) { }

  ngOnInit(): void {
    this.dataService.revertToLogin();
  }

  AddSupplier()
  {
    this.dataService.AddSupplier(this.AddSupplierReq).subscribe({
      next: (supplier) => {this.router.navigate(['/Admin-Screen/suppliers']);
      this.snackBar.open(this.AddSupplierReq+ ` successfully registered`, 'X', {duration: 3000});}
    })
  }

}
