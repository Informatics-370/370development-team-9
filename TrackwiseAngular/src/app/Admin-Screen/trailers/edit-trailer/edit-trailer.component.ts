import { Component } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
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

  showHelpModal: boolean = false;

  constructor(private route: ActivatedRoute, private dataService: DataService, private router:Router, private snackBar: MatSnackBar) { }

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

    this.dataService.revertToLogin();
  }

  EditTrailer()
  {    
    this.dataService.EditTrailer(this.trailerDetails.trailerID, this.trailerDetails).subscribe({
      next: (response) => {this.router.navigate(['/Admin-Screen/trailers']);
      this.snackBar.open(`Trailer successfully edited`, 'X', {duration: 3000});}
    })
  }

  OpenHelpModal() {
    this.showHelpModal = true;
    document.body.style.overflow = 'hidden';
  }

  CloseHelpModal() {
    this.showHelpModal = false;
    document.body.style.overflow = 'auto';
  }
}
