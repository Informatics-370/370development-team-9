import { Component } from '@angular/core';
import { DataService } from 'src/app/services/data.service';
import { Trailer } from 'src/app/shared/trailer';
import { MatDialog, MatDialogRef  } from '@angular/material/dialog';
import { RemoveNotificationComponent } from 'src/app/ConfirmationNotifications/remove-notification/remove-notification.component';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-trailers',
  templateUrl: './trailers.component.html',
  styleUrls: ['./trailers.component.scss']
})
export class TrailersComponent {
  trailers: Trailer[] = [];
  searchText: string = ''; // Property to store the search text
  originalTrailers: Trailer[] = []; // Property to store the original trailer data

  constructor( private dataService: DataService, private dialog: MatDialog, private snackBar: MatSnackBar) { }

  ngOnInit(): void {
    this.GetTrailers();
    this.dataService.revertToLogin();
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

  openConfirmationDialog(TrailerID: string): void {
    const dialogRef = this.dialog.open(RemoveNotificationComponent, {
      width: '300px', // Adjust the width as needed
      data: { TrailerID }
    });
  
    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.DeleteTrailer(TrailerID);
        this.snackBar.open(` Trailer Successfully Removed`, 'X', {duration: 3000});
      }
    });
  }

  DeleteTrailer(TrailerID:string)
  {
    this.dataService.DeleteTrailer(TrailerID).subscribe({
      next: (response) => location.reload()
    })
  }
}
