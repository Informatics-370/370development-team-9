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
  showHelpModal: boolean = false;

  constructor(private dataService: DataService, private router:Router, private snackBar: MatSnackBar) { }

  ngOnInit(): void {
    this.dataService.revertToLogin();
  }

  AddSupplier()
  {
    this.dataService.AddSupplier(this.AddSupplierReq).subscribe({
      next: (supplier) => {this.router.navigate(['/Admin-Screen/suppliers']);
      this.snackBar.open(this.AddSupplierReq.name + ` successfully registered`, 'X', {duration: 3000});}
    })
  }
  OpenHelpModal() {
    this.showHelpModal = true;
    document.body.style.overflow = 'hidden';
  }

  CloseHelpModal() {
    this.showHelpModal = false;
    document.body.style.overflow = 'auto';
  }

}
