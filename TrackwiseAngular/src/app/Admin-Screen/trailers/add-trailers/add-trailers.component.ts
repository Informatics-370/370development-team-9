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
    trailerID:"",
    trailer_License:"",
    model:"",
    weight:0,
    trailer_Status_ID:"",
    trailerStatus:{
      trailer_Status_ID:"",
      status:"",
      description:""
    },
    trailer_Type_ID:"",
    trailerType:{
      trailer_Type_ID:"",
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
