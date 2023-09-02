import { Component, OnInit } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute, Router } from '@angular/router';
import { DataService } from 'src/app/services/data.service';

@Component({
  selector: 'app-job-details',
  templateUrl: './job-details.component.html',
  styleUrls: ['./job-details.component.scss']
})
export class JobDetailsComponent implements OnInit{
  showDetails: boolean = true;
  showDocuments: boolean = false;
  jobs: any[] = [];
  
   constructor(private dataService: DataService, private router: Router, private route: ActivatedRoute, private snackBar: MatSnackBar) { }
  
  ngOnInit(): void {

    this.GetJobDetails();
    this.showDetails=true;
    this.showDocuments=false;
  }

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

  CompleteJob(job : any) {
    let capturedCount = 0;

    job.deliveries.forEach((element: { mileageCaptured: boolean; weightCaptured: boolean; }) => {
      if(element.mileageCaptured == true && element.weightCaptured == true){
        capturedCount++;
      }

      if(capturedCount == job.deliveryCount){
        this.dataService.CompleteJob(job.job_ID).subscribe({
          next: (response) => {
            this.router.navigate(['/Admin-Screen/jobs']);
          } 
        });
      } else {
        this.snackBar.open(` Delivery info not captured`, 'X', {duration: 3000});
      }
    });

    
  }

  ShowDetails() {
    this.showDetails = true;
    this.showDocuments = false;
  }

  ShowDocuments() {
    this.showDetails = false;
    this.showDocuments = true;
  }
}
