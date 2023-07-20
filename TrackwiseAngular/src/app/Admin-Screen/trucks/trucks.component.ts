import { Component, OnInit} from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { DataService} from 'src/app/services/data.service';
import { Truck } from 'src/app/shared/truck';

@Component({
  selector: 'app-trucks',
  templateUrl: './trucks.component.html',
  styleUrls: ['./trucks.component.scss']
})

export class TrucksComponent implements OnInit {

  trucks:Truck[] = []
  searchText: string = '';
  originalTrucks: Truck[]=[];

  constructor( private dataService: DataService) { }

  ngOnInit(): void {
    this.GetTrucks()

  }

  GetTrucks()
  {
    this.dataService.GetTrucks().subscribe(result => {
      let truckList:any[] = result
      this.originalTrucks = [...truckList]; // Store a copy of the original trailer data
      truckList.forEach((element) => {
        this.trucks.push(element)
      });
    })
  }

  search() {
    if (this.searchText.trim() === '') {
      // If search text is empty, revert back to original trailer data
      this.trucks = [...this.originalTrucks];
    } else {
      const searchTextLower = this.searchText.toLowerCase();

      // Filter the trailers based on the search text
      const filteredTrailers = this.originalTrucks.filter(trailer => {
        const license = trailer.truck_License.toLowerCase();
        const model = trailer.model.toLowerCase();
        const status = trailer.truckStatus.status.toLowerCase();
        return (
          license.includes(searchTextLower)||
          model.includes(searchTextLower) || 
          status.includes(searchTextLower) ||
          (searchTextLower === 'available' && status === 'available') ||
          (searchTextLower === 'unavailable' && status === 'unavailable') ||
          (searchTextLower === 'busy' && status === 'busy')
        );
      });

      // Update the drivers array with the filtered results
      this.trucks = filteredTrailers;
    }
  }

  handleKeyUp(event: KeyboardEvent) {
    if (event.key === 'Enter') {
      this.search();
    }
  }

  DeleteTruck(TruckID:string)
  {
    this.dataService.DeleteTruck(TruckID).subscribe({
      next: (response) => location.reload()
    })
  }

}

