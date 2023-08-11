import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { DataService } from 'src/app/services/data.service';
import { Admin } from 'src/app/shared/admin';
import {Document} from 'src/app/shared/document';

@Component({
  selector: 'app-document-information',
  templateUrl: './document-information.component.html',
  styleUrls: ['./document-information.component.scss']
})
export class DocumentInformationComponent implements OnInit{
  showWB: boolean = true;
  showFS: boolean = false;
  showM: boolean = false;
  jobs: any[] = [];
  
  docDetails: Document =
  {
    document_ID:"",
    image:"",
    type:"",
    delivery:{
        delivery_ID:"",
    },
  }
   constructor(private dataService: DataService, private router:Router ,private route: ActivatedRoute) { }
  
  ngOnInit(): void {
    this.GetJobDetails();
    this.showWB=true;
    this.showFS=false;
    this.showM=false;
  }

/*   EditAdmin()
  {    
    this.dataService.EditAdmin(this.adminDetails.admin_ID, this.adminDetails).subscribe({
      next: (response) => {this.router.navigate(['/Admin-Screen/admins']);
    }
    })
    console.log('yes')
  } */

  GetJobDetails(){
    this.route.params.subscribe({
      next: (params) => {
        this.dataService.GetJob(params['job_ID']).subscribe({
          next: (response) => {
            let userName = params['userName'];
            response.userName = userName;
            this.jobs.push(response);
            console.log(this.jobs)
          }
        });
      }
    });
  }

  ShowWB() {
    this.showWB = true;
    this.showFS = false;
    this.showM = false;
  }

  ShowFS() {
    this.showWB = false;
    this.showFS = true;
    this.showM = false;
  }
  ShowM() {
    this.showWB = false;
    this.showFS = false;
    this.showM = true;
  }
}
