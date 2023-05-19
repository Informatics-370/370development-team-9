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

  drivers:Driver[] = []

  searchQuery: string='';
  filteredTrucks: Driver[]=[];


  constructor( private dataService: DataService) { }

  ngOnInit(): void {
    this.GetDrivers()
  }

  GetDrivers()
  {
    this.dataService.GetDrivers().subscribe(result => {
      let driverList:any[] = result
      driverList.forEach((element) => {
        this.drivers.push(element)
        console.log(element)
      });
    })
  }

  DeleteDriver(driver_ID:number)
  {
    this.dataService.DeleteDriver(driver_ID).subscribe({
      next: (response) => location.reload()
    })
  }

}
