import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { DataService } from 'src/app/services/data.service';
import { Supplier } from 'src/app/shared/supplier';

@Component({
  selector: 'app-add-supplier',
  templateUrl: './add-supplier.component.html',
  styleUrls: ['./add-supplier.component.scss']
})
export class AddSupplierComponent {

  supplierDetails: Supplier =
  {
    supplier_ID: 0,
    name: '',
    email:'',
    contactNumber:'',
  };

  constructor(private dataService: DataService, private router:Router) { }

  ngOnInit(): void {
  }

  AddSupplier()
  {
    this.dataService.AddSupplier(this.supplierDetails).subscribe({
      next: (supplier) => {this.router.navigate(['/Admin-Screen/suppliers'])}
    })
  }

}