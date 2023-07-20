import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { DataService } from 'src/app/services/data.service';
import { Trailer } from 'src/app/shared/trailer';

@Component({
  selector: 'app-edit-trailer',
  templateUrl: './edit-trailer.component.html',
  styleUrls: ['./edit-trailer.component.scss']
})
export class EditTrailerComponent {
  trailerDetails: Trailer =
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

  constructor(private route: ActivatedRoute, private dataService: DataService, private router:Router) { }

  ngOnInit(): void {

    this.route.params.subscribe({
      next: (params) => {

          this.dataService.GetTrailer(params['trailerID']).subscribe({
            next: (response) => {
              this.trailerDetails = response;
            }
          })

      }
    })
  }

  EditTrailer()
  {    
    this.dataService.EditTrailer(this.trailerDetails.trailerID, this.trailerDetails).subscribe({
      next: (response) => {this.router.navigate(['/Admin-Screen/trailers'])}
    })
  }
}
