import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { DataService } from 'src/app/services/data.service';
import { Trailer } from 'src/app/shared/trailer';

@Component({
  selector: 'app-add-trailers',
  templateUrl: './add-trailers.component.html',
  styleUrls: ['./add-trailers.component.scss']
})
export class AddTrailersComponent {
  AddTrailerRequest: Trailer =
  {
    trailerID:0,
    trailer_License:"",
    model:"",
    weight:0,
    trailer_Status_ID:0,
    trailerStatus:{
      trailer_Status_ID:0,
      status:"",
      description:""
    },
    trailer_Type_ID:0,
    trailerType:{
      trailer_Type_ID:0,
      name:"",
      description:""
    },
  };

  constructor(private dataService: DataService, private router:Router) { }

  ngOnInit(): void {
  }

  AddTrailer()
  {
    this.dataService.AddTrailer(this.AddTrailerRequest).subscribe({
      next: (trailer) => {this.router.navigate(['/Admin-Screen/trailers'])}
    })
  }
}
