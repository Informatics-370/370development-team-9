import { Component, OnInit } from '@angular/core';
import { DataService } from 'src/app/services/data.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Observable } from 'rxjs';
import { Job } from 'src/app/shared/job';
import { Router } from '@angular/router';

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
    job_Status_ID:''
  };

  showView: boolean = true;
  showAdd: boolean = false;
  minDateTime: string;
  
  constructor(private dataService: DataService, private router:Router) {
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
      this.dataService.GetAdmin(userID).subscribe(
        (result) => {
          if (result == null) {
            this.dataService.GetClient(userID).subscribe(
              (name) => {
                this.users = name.name;
                observer.next(this.users); // Emit the user name
                observer.complete();
              },
              (error) => observer.error(error)
            );
          } else {
            this.users = result.name;
            observer.next(this.users); // Emit the user name
            observer.complete();
          }
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

  search() {
    if (this.searchText.trim() === '') {
      // If search text is empty, revert back to original admin data
      this.jobs = [...this.originalJobs];
    } else {
      const searchTextLower = this.searchText.toLowerCase();

      // Filter the admins based on the search text
      const filteredAdmins = this.originalJobs.filter(jobs => {
        const fullName = jobs.name.toLowerCase() + ' ' + jobs.lastname.toLowerCase();
        const email = jobs.email.toLowerCase();
        const password = jobs.password.toLowerCase();
        return (
          fullName.includes(searchTextLower) ||
          jobs.lastname.toLowerCase().includes(searchTextLower) ||
          email.includes(searchTextLower) || password.includes(searchTextLower)
          
        );
      });

      // Update the admins array with the filtered results
      this.jobs = filteredAdmins;
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
