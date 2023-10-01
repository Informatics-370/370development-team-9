import { Component } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';
import { DataService } from 'src/app/services/data.service';
import { BreakInterval } from 'src/app/shared/BreakInterval';
import { JobRule } from 'src/app/shared/JobRule';
import { MaxHrs } from 'src/app/shared/MaxHrs';
import { Rest } from 'src/app/shared/Rest';
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

  CurrentBreak: BreakInterval = {
    break_Amount: 0
  };
  NewBreak: BreakInterval = {
    break_Amount: 0
  };

  CurrentRest: Rest = {
    rest_Amount: 0
  };
  NewRest: Rest = {
    rest_Amount: 0
  };

  CurrentHrs: MaxHrs = {
    hrs_Amount: 0
  };
  NewHrs: MaxHrs = {
    hrs_Amount: 0
  };

  
  showEditForm: boolean = false;
  showBreakForm: boolean = false;
  showRestForm: boolean = false;
  showHrsForm: boolean = false;

  constructor( private dataService: DataService, private dialog: MatDialog, private snackBar: MatSnackBar, private router:Router) { }
  
    ngOnInit(): void {
      this.GetVAT();
      this.GetBreak();
      this.GetRest();
      this.GetMax();
    }
  
  toggleEditForm() {
    this.showEditForm = !this.showEditForm;
  }
  toggleBreakForm() {
    this.showBreakForm = !this.showBreakForm;
  }
  toggleRestForm() {
    this.showRestForm = !this.showRestForm;
  }
  toggleHrsForm() {
    this.showHrsForm = !this.showHrsForm;
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

  GetBreak()
  {
    this.dataService.GetBreak().subscribe({
      next: (response) => {
        this.CurrentBreak = response;
      }
    })
  }
  UpdateBreak()
  {
    this.dataService.UpdateBreak(this.NewBreak.break_Amount).subscribe({
      next: (vat) => 
      {
        console.log("NEW BREAK ",this.NewBreak.break_Amount);
        this.GetBreak()
        this.NewBreak.break_Amount = 0
        this.snackBar.open(`Driver break amount successfully updated`, 'X', {duration: 3000})
      }
  
    })
  }
  
  GetRest()
  {
    this.dataService.GetRest().subscribe({
      next: (response) => {
        this.CurrentRest = response;
      }
    })
  }
  UpdateRest()
  {
    this.dataService.UpdateRest(this.NewRest.rest_Amount).subscribe({
      next: (vat) => 
      {
        this.GetRest()
        this.NewRest.rest_Amount = 0
        this.snackBar.open(`Rest period successfully updated`, 'X', {duration: 3000})
      }
  
    })
  }

  GetMax()
  {
    this.dataService.GetMax().subscribe({
      next: (response) => {
        this.CurrentHrs = response;
      }
    })
  }
  UpdateMax()
  {
    this.dataService.UpdateMax(this.NewHrs.hrs_Amount).subscribe({
      next: (vat) => 
      {
        this.GetMax()
        this.NewHrs.hrs_Amount = 0
        this.snackBar.open(`Maximum hours per day successfully updated`, 'X', {duration: 3000})
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
