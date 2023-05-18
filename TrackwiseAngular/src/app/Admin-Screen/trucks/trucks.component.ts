import { Component, OnInit} from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { DataService} from 'src/services/data.service';
import { Truck } from 'src/shared/truck';

@Component({
  selector: 'app-trucks',
  templateUrl: './trucks.component.html',
  styleUrls: ['./trucks.component.scss']
})

export class TrucksComponent implements OnInit {

  trucks:Truck[] = []

  searchQuery: string='';
  filteredTrucks: Truck[]=[];


  constructor( private dataService: DataService) { }

  ngOnInit(): void {
    this.GetTrucks()

  }



  GetTrucks()
  {
    this.dataService.GetTrucks().subscribe(result => {
      let truckList:any[] = result
      truckList.forEach((element) => {
        this.trucks.push(element)
        console.log(element)
      });
    })
  }

  DeleteTruck(TruckID:number)
  {
    this.dataService.DeleteTruck(TruckID).subscribe({
      next: (response) => location.reload()
    })
  }

}

