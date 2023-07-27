import { Component, OnInit } from '@angular/core';
import { DataService } from 'src/app/services/data.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-jobs',
  templateUrl: './jobs.component.html',
  styleUrls: ['./jobs.component.scss']
})
export class JobsComponent implements OnInit{
  searchText: string = ''; // Property to store the search text
  admins: any[] = []; // Property to store the admin data
  originalAdmins: any[] = []; // Property to store the original admin data

  showView: boolean = true;
  showAdd: boolean = false;
  
  constructor(private dataService: DataService) { }
  
  ngOnInit(): void {
    this.GetAdmins();
    this.showView=true;
    this.showAdd=false;
  }

  GetAdmins() {
    this.dataService.GetAdmins().subscribe(result => {
      let adminList: any[] = result;
      this.originalAdmins = [...adminList]; // Store a copy of the original admin data
      adminList.forEach((element) => {
        this.admins.push(element);
        console.log(element);
      });
    });
  }

  search() {
    if (this.searchText.trim() === '') {
      // If search text is empty, revert back to original admin data
      this.admins = [...this.originalAdmins];
    } else {
      const searchTextLower = this.searchText.toLowerCase();

      // Filter the admins based on the search text
      const filteredAdmins = this.originalAdmins.filter(admin => {
        const fullName = admin.name.toLowerCase() + ' ' + admin.lastname.toLowerCase();
        const email = admin.email.toLowerCase();
        const password = admin.password.toLowerCase();
        return (
          fullName.includes(searchTextLower) ||
          admin.lastname.toLowerCase().includes(searchTextLower) ||
          email.includes(searchTextLower) || password.includes(searchTextLower)
          
        );
      });

      // Update the admins array with the filtered results
      this.admins = filteredAdmins;
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
}
