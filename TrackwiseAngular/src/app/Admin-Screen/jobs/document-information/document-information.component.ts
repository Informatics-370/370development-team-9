import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { DataService } from 'src/app/services/data.service';
import { Admin } from 'src/app/shared/admin';
import {Document} from 'src/app/shared/document';

@Component({
  selector: 'app-document-information',
  templateUrl: './document-information.component.html',
  styleUrls: ['./document-information.component.scss']
})
export class DocumentInformationComponent implements OnInit{
  showWB: boolean = true;
  showFS: boolean = false;
  showM: boolean = false;

  delivery_ID: string ="";
  hasWeighBridgeDocuments: boolean = false;
  hasFuelDocuments: boolean = false;
  hasMileageDocuments: boolean = false;
  documents: any[] = [];
  /* docDetails: Document =
  {
    document_ID:"",
    image:"",
    type:"",
    delivery:{
        delivery_ID:"",
    },
  } */
   constructor(private dataService: DataService, private router:Router ,private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.delivery_ID = params['delivery_ID'];
      this.loadDocuments(this.delivery_ID);
    });
    this.showWB=true;
    this.showFS=false;
    this.showM=false;
  }

  loadDocuments(delivery_ID : string) {
    this.dataService.GetDocuments(delivery_ID).subscribe(response=>{
      
        let custOrderslist: any[] = response;
        custOrderslist.forEach((element) => {
          this.documents.push(element);
          if (element.docType === 'Weight') {
            this.hasWeighBridgeDocuments = true;
        } else if(element.docType === 'Fuel'){
            this.hasFuelDocuments = true;
        } else if(element.docType === 'Mileage'){
            this.hasMileageDocuments = true;
        }
          console.log(element);
        });
        console.log(this.documents);
      
    });
  }

  actualWeight: any

  updateActualWeight(document: any) {
    if (document.actualWeight !== undefined && document.actualWeight !== null) {
        const payload = {
            actual_weight: document.actualWeight, // Use 'actual_weight' as expected by your backend
            // Add any other necessary properties
        };

        this.dataService.UpdateActualWeight(document.Delivery_ID, payload).subscribe(
            response => {
                // Handle success response
                console.log("Actual weight updated successfully.");
            },
            error => {
                // Handle error
                console.error("Error updating actual weight:", error);
            }
        );
    }
}

  ShowWB() {
    this.showWB = true;
    this.showFS = false;
    this.showM = false;
  }

  ShowFS() {
    this.showWB = false;
    this.showFS = true;
    this.showM = false;
  }
  ShowM() {
    this.showWB = false;
    this.showFS = false;
    this.showM = true;
  }
}
