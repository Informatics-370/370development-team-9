import { Component, OnInit } from '@angular/core';
import { DataService } from 'src/app/services/data.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Observable } from 'rxjs';
import { Job } from 'src/app/shared/job';
import { Router } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';


@Component({
  selector: 'app-client-jobs',
  templateUrl: './client-jobs.component.html',
  styleUrls: ['./client-jobs.component.scss']
})
export class ClientJobsComponent {

    searchText: string = ''; // Property to store the search text
    jobs: any[] = []; // Property to store the admin data
    originalJobs: any[] = []; // Property to store the original admin data
    users : string = "";
    
    createJob : Job = 
    {
      job_ID: '',
      startDate: '',
      dueDate: '',
      pickup_Location: '',
      dropoff_Location: '',
      total_Weight: 0,
      creator_ID: '',
      job_Type_ID: '',
      job_Status_ID:''
    };
  
    showView: boolean = true;
    showAdd: boolean = false;
    minDateTime: string;
    
    constructor(private dataService: DataService, private router:Router, private snackBar: MatSnackBar) {
      this.minDateTime = this.getCurrentDateTime();
     }
    
    ngOnInit(): void {
      this.GetClientJobs();
      this.showView=true;
      this.showAdd=false;
    }
  
    GetClientJobs() {
      this.dataService.GetClientJobs().subscribe((result) => {
        let jobList: any[] = result;
        this.originalJobs = [...jobList]; // Store a copy of the original admin data

        // Use Promise.all to handle multiple asynchronous calls efficiently
        Promise.all(
          jobList.map((element) => {
            return this.GetUser(element.creator_ID).toPromise();
          })
        )
          .then((userNames) => {
            // Assign the user names to the corresponding elements
            jobList.forEach((element, index) => {
              element.userName = userNames[index];
              this.jobs.push(element);
              console.log(element);
            });
          })
          .catch((error) => console.error('Error getting user names:', error));
      });
    }
  
    GetUser(userID: string): Observable<string> {
      return new Observable((observer) => {
        this.dataService.GetClient(userID).subscribe(
          (result) => {
            if (result && result.name) {
              // If a client is found and it has a name property, use the client's name
              this.users = result.name;
            } else {
              // If no client is found or the client doesn't have a name property, handle the error case
              observer.error("User not found");
              return;
            }
    
            observer.next(this.users); // Emit the user name
            observer.complete();
          },
          (error) => observer.error(error)
        );
      });
    }
  
    CreateJob()
    {
      this.dataService.CreateJob(this.createJob).subscribe(
        () => {
          this.jobs = [];
          this.GetClientJobs();
          this.showView = true;
          this.showAdd = false;
          this.snackBar.open(`Job successfully registered`, 'X', {duration: 3000});}
        )   
  
  
    }
  
    search() {
      if (this.searchText.trim() === '') {
        // If search text is empty, revert back to original admin data
        this.jobs = [...this.originalJobs];
      } else {
        const searchTextLower = this.searchText.toLowerCase();
  
        // Filter the admins based on the search text
        const filteredJobs = this.originalJobs.filter(jobs => {
          const jobID = jobs.job_ID.toLowerCase();
          const startDate = jobs.startDate.toLowerCase();
          const dueDate = jobs.dueDate.toLowerCase();
          const jobType = jobs.jobType.name.toLowerCase();
          const username = jobs.userName.toLowerCase();
          const jobStatus = jobs.jobStatus.name.toLowerCase();
          return (
            jobID.includes(searchTextLower) ||
            startDate.includes(searchTextLower) || 
            dueDate.includes(searchTextLower)||
            jobType.includes(searchTextLower)||
            username.includes(searchTextLower)||
            jobStatus.includes(searchTextLower)
          );
        });
  
        // Update the admins array with the filtered results
        this.jobs = filteredJobs;
      }
    }
  
    handleKeyUp(event: KeyboardEvent) {
      if (event.key === 'Enter') {
        this.search();
      }
    }
  
    DeleteAdmin(admin_ID:string)
    {
      this.dataService.DeleteAdmin(admin_ID).subscribe({
        next: (response) => location.reload()
      })
    }
  
    ShowView() {
      this.showView = true;
      this.showAdd = false;
    }
  
    ShowAdd() {
      this.showView = false;
      this.showAdd = true;
    }
  
    getCurrentDateTime(): string {
      // Get the current date and time in ISO format
      // The datetime-local input expects the value to be in the format: "YYYY-MM-DDTHH:mm"
      const now = new Date();
      const year = now.getFullYear();
      const month = String(now.getMonth() + 1).padStart(2, '0');
      const day = String(now.getDate()).padStart(2, '0');
      const hours = String(now.getHours()).padStart(2, '0');
      const minutes = String(now.getMinutes()).padStart(2, '0');
  
      return `${year}-${month}-${day}T${hours}:${minutes}`;
    }
  }
  
