import { Component, OnInit } from '@angular/core';
import { DataService } from 'src/app/services/data.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Observable, catchError, map, throwError } from 'rxjs';
import { Job } from 'src/app/shared/job';
import { Router } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';
import { MatDialog, MatDialogRef  } from '@angular/material/dialog';
import { CancelNotificationComponent } from 'src/app/ConfirmationNotifications/cancel-notification/cancel-notification.component';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-jobs',
  templateUrl: './jobs.component.html',
  styleUrls: ['./jobs.component.scss']
})
export class JobsComponent implements OnInit{
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
    job_Status_ID: '',
    jobType: '',
    jobStatus: ''
  };

  showView: boolean = true;
  showAdd: boolean = false;
  minDateTime: string;
  showHelpModal: boolean = false;
  
  constructor(private dataService: DataService, private router:Router, private dialog: MatDialog, private snackBar: MatSnackBar) {
    this.minDateTime = this.getCurrentDateTime();
   }
  
  ngOnInit(): void {
    this.GetJobs();
    this.showView=true;
    this.showAdd=false;
  }

  GetJobs() {
    this.dataService.GetJobs().subscribe((result) => {
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
      this.dataService.GetAdmin(userID).pipe(
        catchError((error: HttpErrorResponse) => {
          // Check if the error status is 404 (Not Found)
          if (error.status === 404) {
            // The admin was not found, try getting the client data
            return this.dataService.GetClient(userID).pipe(
              map((name) => {
                // `name` will be a string in this case, so set it directly
                this.users = name.name;
                return this.users;
              }),
              catchError((clientError: HttpErrorResponse) => {
                // Handle any errors that may occur while getting the client data
                observer.error(clientError);
                return throwError(clientError);
              })
            );
          } else {
            // Handle other errors related to the GetAdmin call
            observer.error(error);
            return throwError(error);
          }
        }),
        map((result) => {
          if (typeof result === 'string') {
            // `result` is a string (not an Admin object)
            // Do whatever you need to do with the string, e.g., store it in `this.users`
            this.users = result;
          } else {
            // `result` is an Admin object
            // Access the `name` property
            this.users = result.name;
          }
          return this.users;
        })
      ).subscribe(
        (userName) => {
          observer.next(userName); // Emit the user name
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
        this.GetJobs();
        this.showView = true;
        this.showAdd = false;}
      )   


  }

  openConfirmationDialog(job_ID: string): void {
    const dialogRef = this.dialog.open(CancelNotificationComponent, {
      width: '300px', // Adjust the width as needed
      data: { job_ID }
    });
  
    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.CancelJob(job_ID);
        this.snackBar.open(` Job Successfully Cancelled`, 'X', {duration: 3000});
      }
    });
  }

  CancelJob(job_ID : string) {
    this.dataService.CancelJob(job_ID).subscribe({
      next: (response) => {
        this.jobs = [];
        this.GetJobs();
      }
    });
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

  removeNegativeSign() {
    if (this.createJob.total_Weight < 0) {
      this.createJob.total_Weight = 0;
    }
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

  OpenHelpModal() {
    this.showHelpModal = true;
    document.body.style.overflow = 'hidden';
  }

  CloseHelpModal() {
    this.showHelpModal = false;
    document.body.style.overflow = 'auto';
  }
}
