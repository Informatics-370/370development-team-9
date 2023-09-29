import { Component } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';
import { DataService } from 'src/app/services/data.service';
import { JobRule } from 'src/app/shared/JobRule';
import { VAT } from 'src/app/shared/VAT';

@Component({
  selector: 'app-business-rules',
  templateUrl: './business-rules.component.html',
  styleUrls: ['./business-rules.component.scss']
})
export class BusinessRulesComponent {
  CurrentVAT: VAT = {
    vaT_Amount: 0
  };

  NewVAT: VAT = {
    vaT_Amount: 0
  };

  CurrentRule: JobRule = {
    break: 0,
    rest: 0,
    maxHrs: 0
  };
  NewRule: JobRule = {
    break: 0,
    rest: 0,
    maxHrs: 0
  };
  
  showEditForm: boolean = false;

  constructor( private dataService: DataService, private dialog: MatDialog, private snackBar: MatSnackBar, private router:Router) { }
  
    ngOnInit(): void {
      this.GetVAT();
      
    }
  
  toggleEditForm() {
    this.showEditForm = !this.showEditForm;
  }

  GetVAT()
  {
    this.dataService.GetVAT().subscribe({
      next: (response) => {
        this.CurrentVAT = response;
      }
    })
  }

  UpdateVAT()
  {
    this.dataService.UpdateVAT(this.NewVAT.vaT_Amount/100).subscribe({
      next: (vat) => 
      {
        this.GetVAT()
        this.NewVAT.vaT_Amount = 0
        this.snackBar.open(`VAT successfully updated`, 'X', {duration: 3000})
      }
  
    })
  }
  UpdateBreak()
  {
    this.dataService.UpdateVAT(this.NewVAT.vaT_Amount/100).subscribe({
      next: (vat) => 
      {
        this.GetVAT()
        this.NewVAT.vaT_Amount = 0
        this.snackBar.open(`VAT successfully updated`, 'X', {duration: 3000})
      }
  
    })
  }
  UpdateRest()
  {
    this.dataService.UpdateVAT(this.NewVAT.vaT_Amount/100).subscribe({
      next: (vat) => 
      {
        this.GetVAT()
        this.NewVAT.vaT_Amount = 0
        this.snackBar.open(`VAT successfully updated`, 'X', {duration: 3000})
      }
  
    })
  }
  UpdateMax()
  {
    this.dataService.UpdateVAT(this.NewVAT.vaT_Amount/100).subscribe({
      next: (vat) => 
      {
        this.GetVAT()
        this.NewVAT.vaT_Amount = 0
        this.snackBar.open(`VAT successfully updated`, 'X', {duration: 3000})
      }
  
    })
  }



  removeNegativeSign() {
    if (this.NewVAT.vaT_Amount < 0) {
      this.NewVAT.vaT_Amount = 0;
    }

    if (this.NewVAT.vaT_Amount > 100) {
      this.NewVAT.vaT_Amount = 100;
    }
  }

}
