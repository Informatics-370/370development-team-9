import { Component, OnInit } from '@angular/core';
import { DataService } from 'src/app/services/data.service';
import { Router } from '@angular/router';
import { Job } from 'src/app/shared/job';
@Component({
  selector: 'app-create-job',
  templateUrl: './create-job.component.html',
  styleUrls: ['./create-job.component.scss']
})
export class CreateJobComponent implements OnInit{

  adminDetails: Job =
  {
    client_ID:0,
    name:"",
    phoneNumber:"",
  };
  constructor(private dataService: DataService, private router:Router) { }

  ngOnInit(): void {
  }

  CreateJob()
  {
    this.dataService.CreateJob(this.adminDetails).subscribe({
      next: (admin) => {this.router.navigate(['/Admin-Screen/jobs'])}
    })
  }
}
