import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-job-details',
  templateUrl: './job-details.component.html',
  styleUrls: ['./job-details.component.scss']
})
export class JobDetailsComponent implements OnInit{
  showDetails: boolean = true;
  showDocuments: boolean = false;
  
  // constructor(private dataService: DataService) { }
  
  ngOnInit(): void {

    this.showDetails=true;
    this.showDocuments=false;
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
