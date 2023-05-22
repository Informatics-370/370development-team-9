import { Component } from '@angular/core';
import { DataService } from 'src/app/services/data.service';
import { Trailer } from 'src/app/shared/trailer';

@Component({
  selector: 'app-trailers',
  templateUrl: './trailers.component.html',
  styleUrls: ['./trailers.component.scss']
})
export class TrailersComponent {
  trailers: Trailer[] = [];
  searchText: string = ''; // Property to store the search text
  originalTrailers: Trailer[] = []; // Property to store the original trailer data

  constructor( private dataService: DataService) { }

  ngOnInit(): void {
    this.GetTrailers();
  }


  GetTrailers()
  {
    this.dataService.GetTrailers().subscribe(result => {
      let trailerList:any[] = result
      this.originalTrailers = [...trailerList]; // Store a copy of the original trailer data
      trailerList.forEach((element) => {
        this.trailers.push(element)
      });
    })
  }

  search() {
    if (this.searchText.trim() === '') {
      // If search text is empty, revert back to original trailer data
      this.trailers = [...this.originalTrailers];
    } else {
      const searchTextLower = this.searchText.toLowerCase();

      // Filter the trailers based on the search text
      const filteredTrailers = this.originalTrailers.filter(trailer => {
        const license = trailer.trailer_License.toLowerCase();
        const model = trailer.model.toLowerCase();
        const weight = trailer.weight;
        const status = trailer.trailerStatus.status.toLowerCase();
        const type = trailer.trailerType.name.toLowerCase();
        return (
          license.includes(searchTextLower)||
          weight.toString().includes(searchTextLower)||
          model.includes(searchTextLower) || 
          status.includes(searchTextLower) ||
          type.includes(searchTextLower) ||
          (searchTextLower === 'available' && status === 'available') ||
          (searchTextLower === 'unavailable' && status === 'unavailable') ||
          (searchTextLower === 'busy' && status === 'busy')
        );
      });

      // Update the drivers array with the filtered results
      this.trailers = filteredTrailers;
    }
  }

  handleKeyUp(event: KeyboardEvent) {
    if (event.key === 'Enter') {
      this.search();
    }
  }

  DeleteTrailer(TrailerID:number)
  {
    this.dataService.DeleteTrailer(TrailerID).subscribe({
      next: (response) => location.reload()
    })
  }
}
