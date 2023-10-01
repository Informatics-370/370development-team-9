import { Component, OnInit } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute, Router } from '@angular/router';
import { DataService } from 'src/app/services/data.service';
import { Supplier } from 'src/app/shared/supplier';



@Component({
  selector: 'app-edit-supplier',
  templateUrl: './edit-supplier.component.html',
  styleUrls: ['./edit-supplier.component.scss']
})
export class EditSupplierComponent implements OnInit{

  EditSupplierReq: Supplier =
  {
    supplier_ID: "",
    name: '',
    email:'',
    password:'',
    contact_Number:'',
  };

  showHelpModal: boolean = false;

  constructor(private route: ActivatedRoute, private dataService: DataService, private router:Router, private snackBar: MatSnackBar) { }

  ngOnInit(): void {
    this.route.params.subscribe({
      next: (params) => {

          this.dataService.GetSupplier(params['supplier_ID']).subscribe({
            next: (response) => {
              this.EditSupplierReq = response;
            }
          })

      }
    })

    this.dataService.revertToLogin();
  }

  EditSupplier()
  {    
    this.dataService.EditSupplier(this.EditSupplierReq.supplier_ID, this.EditSupplierReq).subscribe({
      next: (response) => {this.router.navigate(['/Admin-Screen/suppliers']);
      this.snackBar.open(`Supplier successfully edited`, 'X', {duration: 3000});}
    })
    console.log('yes')
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
