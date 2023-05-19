import { Component } from '@angular/core';
import { DataService } from 'src/app/services/data.service';
import { Trailer } from 'src/app/shared/trailer';

@Component({
  selector: 'app-trailers',
  templateUrl: './trailers.component.html',
  styleUrls: ['./trailers.component.scss']
})
export class TrailersComponent {
  trailers:Trailer[] = []

  searchQuery: string='';
  filteredTrailer: Trailer[]=[];


  constructor( private dataService: DataService) { }

  ngOnInit(): void {
    this.GetTrailers()

  }

  GetTrailers()
  {
    this.dataService.GetTrailers().subscribe(result => {
      let trailerList:any[] = result
      trailerList.forEach((element) => {
        this.trailers.push(element)
        console.log(element)
      });
    })
  }

  DeleteTrailer(TrailerID:number)
  {
    this.dataService.DeleteTrailer(TrailerID).subscribe({
      next: (response) => location.reload()
    })
  }
}
