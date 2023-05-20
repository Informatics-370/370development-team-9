import { Component , OnInit} from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { DataService} from 'src/app/services/data.service';
import { Driver } from 'src/app/shared/driver';

@Component({
  selector: 'app-drivers',
  templateUrl: './drivers.component.html',
  styleUrls: ['./drivers.component.scss']
})
export class DriversComponent implements OnInit {
  searchText: string = ''; // Property to store the search text
  drivers: any[] = []; // Property to store the driver data
  originalDrivers: any[] = []; // Property to store the original driver data
  
  constructor(private dataService: DataService) { }
  
  ngOnInit(): void {
    this.GetDrivers();
  }

  GetDrivers() {
    this.dataService.GetDrivers().subscribe(result => {
      let driverList: any[] = result;
      this.originalDrivers = [...driverList]; // Store a copy of the original driver data
      driverList.forEach((element) => {
        this.drivers.push(element);
        console.log(element);
      });
    });
  }

  search() {
    if (this.searchText.trim() === '') {
      // If search text is empty, revert back to original driver data
      this.drivers = [...this.originalDrivers];
    } else {
      const searchTextLower = this.searchText.toLowerCase();

      // Filter the drivers based on the search text
      const filteredDrivers = this.originalDrivers.filter(driver => {
        const fullName = driver.name.toLowerCase() + ' ' + driver.lastname.toLowerCase();
        const status = driver.driverStatus.status.toLowerCase();
        const phoneNumber = driver.phoneNumber.toLowerCase();
        return (
          fullName.includes(searchTextLower) ||
          driver.lastname.toLowerCase().includes(searchTextLower) ||
          phoneNumber.includes(searchTextLower) ||
          (searchTextLower === 'available' && status === 'available') ||
          (searchTextLower === 'unavailable' && status === 'unavailable') ||
          (searchTextLower === 'busy' && status === 'busy')
        );
      });

      // Update the drivers array with the filtered results
      this.drivers = filteredDrivers;
    }
  }

  handleKeyUp(event: KeyboardEvent) {
    if (event.key === 'Enter') {
      this.search();
    }
  }

  DeleteDriver(driver_ID:number)
  {
    this.dataService.DeleteDriver(driver_ID).subscribe({
      next: (response) => location.reload()
    })
  }

}
