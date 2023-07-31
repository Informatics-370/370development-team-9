import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
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
  
   constructor(private dataService: DataService, private route: ActivatedRoute) { }
  
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

  ShowDetails() {
    this.showDetails = true;
    this.showDocuments = false;
  }

  ShowDocuments() {
    this.showDetails = false;
    this.showDocuments = true;
  }
}
