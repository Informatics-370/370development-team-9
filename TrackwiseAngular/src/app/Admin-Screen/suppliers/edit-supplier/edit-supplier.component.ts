import { Component, OnInit } from '@angular/core';
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
    contact_Number:'',
  };

  constructor(private route: ActivatedRoute, private dataService: DataService, private router:Router) { }

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
  }

  EditSupplier()
  {    
    this.dataService.EditSupplier(this.EditSupplierReq.supplier_ID, this.EditSupplierReq).subscribe({
      next: (response) => {this.router.navigate(['/Admin-Screen/suppliers'])}
    })
    console.log('yes')
  }



}
