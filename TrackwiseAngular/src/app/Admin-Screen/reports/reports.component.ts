import { Component, OnInit } from '@angular/core';
import { DataService } from 'src/app/services/data.service';
import { Product } from 'src/app/shared/product';
import { Jobs } from 'src/app/shared/jobs';
import { jsPDF } from 'jspdf';
import autoTable from 'jspdf-autotable';
import { Observable, catchError, map, throwError } from 'rxjs';
import { HttpErrorResponse } from '@angular/common/http';
import { DatePipe, formatDate } from '@angular/common';
import { LoadsCarried } from 'src/app/shared/loadsCarried';


@Component({
  selector: 'app-reports',
  templateUrl: './reports.component.html',
  styleUrls: ['./reports.component.scss']
})
export class ReportsComponent implements OnInit {
  products: Product[] = [];

  jobs: any[] = [];
  users : string = "";
  originalJobs: any[] = [];
  loadsCarried : LoadsCarried[] = []
  pdfSrc: string | null = null;

  constructor(private dataService: DataService, private datePipe: DatePipe) { }

  ngOnInit(): void {
    this.getProducts();
    this.GetJobs();
    this.getLoadsCarried();

  }

  // Methods-------------------------------------------------------------------------

  getProducts() {
    this.dataService.GetProducts().subscribe(result => {
      this.products = result;
    });
  }


  GetJobs() {
    this.dataService.GetJobs().subscribe((result) => {
      let jobList: any[] = result;
      this.originalJobs = [...jobList]; // Store a copy of the original admin data
  
      // Use Promise.all to handle multiple asynchronous calls efficiently
      Promise.all(
        jobList.map((element) => {
          return this.GetUser(element.creator_ID).toPromise();
        })
      )
        .then((userNames) => {
          // Assign the user names to the corresponding elements
          jobList.forEach((element, index) => {
            element.userName = userNames[index];
            this.jobs.push(element);
            console.log(element);
          });
        })
        .catch((error) => console.error('Error getting user names:', error));
    });
  }

  GetUser(userID: string): Observable<string> {
    return new Observable((observer) => {
      this.dataService.GetAdmin(userID).pipe(
        catchError((error: HttpErrorResponse) => {
          // Check if the error status is 404 (Not Found)
          if (error.status === 404) {
            // The admin was not found, try getting the client data
            return this.dataService.GetClient(userID).pipe(
              map((name) => {
                // `name` will be a string in this case, so set it directly
                this.users = name.name;
                return this.users;
              }),
              catchError((clientError: HttpErrorResponse) => {
                // Handle any errors that may occur while getting the client data
                observer.error(clientError);
                return throwError(clientError);
              })
            );
          } else {
            // Handle other errors related to the GetAdmin call
            observer.error(error);
            return throwError(error);
          }
        }),
        map((result) => {
          if (typeof result === 'string') {
            // `result` is a string (not an Admin object)
            // Do whatever you need to do with the string, e.g., store it in `this.users`
            this.users = result;
          } else {
            // `result` is an Admin object
            // Access the `name` property
            this.users = result.name;
          }
          return this.users;
        })
      ).subscribe(
        (userName) => {
          observer.next(userName); // Emit the user name
          observer.complete();
        },
        (error) => observer.error(error)
      );
    });
  }



  getLoadsCarried() {
    this.dataService.GetLoadsCarried().subscribe(result => {
      this.loadsCarried = result;
      console.log(this.loadsCarried)
    });
  }




  // Generate PDFs--------------------------------------------------------------------

  // Generate Stock Report

  generateStockReport() {
    const doc = new jsPDF();

    // Image

    const img = new Image();
    img.src = "assets/LTTLogo.jpg";
    doc.addImage(img, 'JPG', 10, 5, 50, 30);

    // Text

    doc.setFontSize(18);
    doc.setTextColor(0, 79, 158);
    doc.setFont('helvetica', 'bold');
    doc.text('Product Stock Report', 130, 20);

    doc.setFontSize(10);
    doc.setTextColor(0, 0, 0);
    doc.setFont('helvetica');
    const currentDate = new Date();
    doc.text('Generated On: ' + currentDate.toLocaleDateString(), 130, 27);


    // Table and table data

    const header = ['Product ID', 'Name', 'Quantity', 'Price', 'Stock Level', ''];

    const tableData = this.products.map(prod => [
      prod.product_ID,
      prod.product_Name,
      prod.quantity,
      `R ${prod.product_Price.toFixed(2)}`,
      prod.quantity == 0 ? 'No stock available' :
       prod.quantity >= 1 && prod.quantity <= 5 ? 'Low Stock' : 
       prod.quantity >= 6 && prod.quantity <= 15 ? 'Moderate Stock':
       'High Stock'  ,
    ]);
    
    // Autotable layout

    autoTable(doc, {
      head: [header],
      body: tableData,
      startY: 50, // Adjust the starting Y-coordinate for the table
    });

    // Opening of the PDF

    const pdfData = doc.output('datauristring');
    this.pdfSrc = pdfData;

    const pdfWindow = window.open();

    // Validation for PDF load

    if (pdfWindow) {
      pdfWindow.document.write('<iframe width="100%" height="100%" src="' + pdfData + '"></iframe>');
    } else {
      console.error('Failed to open PDF preview window.');
    }
  }




  // Generate Job list report 

  generateJobReport(){

    const document = new jsPDF('landscape'); //instance of the jsPDF class

    const img = new Image();
    img.src = "assets/LTTLogo.jpg";
    document.addImage(img, 'JPG', 10, 5, 50, 30);

    // Text

    document.setFontSize(18);
    document.setTextColor(0, 79, 158);
    document.setFont('helvetica', 'bold');
    document.text('Job Report', 230, 20);

    document.setFontSize(10);
    document.setTextColor(0, 0, 0);
    document.setFont('helvetica');
    const currentDate = new Date();
    document.text('Generated On: ' + currentDate.toLocaleDateString(), 230, 27);


    
  

    const header = ['Job' + '\n' + 'ID', 'Creator', 'Start' + '\n' + 'Date', 'End' + '\n' + 'Date', 'Pickup' + '\n' + 'Location', 'Drop-off' + '\n' + 'Location', 'Job' + '\n' + 'Type', 'Job' + '\n' + 'Status']

    const tableData = this.jobs.map(job => [
      job.job_ID,
      job.userName,
      job.startDate = this.datePipe.transform(job.startDate, 'yyyy/dd/MM'),
      job.dueDate = this.datePipe.transform(job.dueDate, 'yyyy/MM/dd'),
      job.pickup_Location,
      job.dropoff_Location,
      job.jobType.name,
      job.jobStatus.name,
    
     
    ]);


   

    autoTable(document, {
      head: [header],
      body: tableData,
      startY: 50
      
    });



    const pdfData = document.output('datauristring');
    this.pdfSrc = pdfData;

    const pdfWindow = window.open();

    // Validation for PDF load

    if (pdfWindow) {
      pdfWindow.document.write('<iframe width="100%" height="100%" src="' + pdfData + '"></iframe>');
    } else {
      console.error('Failed to open PDF preview window.');
    }
  }

    // Generate Loads Carried Report

    generateLoadsCarriedReport() {
      const doc = new jsPDF();
  
      // Image
  
      const img = new Image();
      img.src = "assets/LTTLogo.jpg";
      doc.addImage(img, 'JPG', 10, 5, 50, 30);
  
      // Text
  
      doc.setFontSize(18);
      doc.setTextColor(0, 79, 158);
      doc.setFont('helvetica', 'bold');
      doc.text('Loads Carried Report', 130, 20);
  
      doc.setFontSize(10);
      doc.setTextColor(0, 0, 0);
      doc.setFont('helvetica');
      const currentDate = new Date();
      doc.text('Generated On: ' + currentDate.toLocaleDateString(), 130, 27);
  
  
      // Table and table data
  
      const header = ['Registration', 'Number of Trips', 'Weight Carried'];
  
      const tableData = this.loadsCarried.map(load => [
        load.registration,
        load.trip,
        load.weight + " T",
      ]);
      
      // Autotable layout
  
      autoTable(doc, {
        head: [header],
        body: tableData,
        startY: 50, // Adjust the starting Y-coordinate for the table
      });
  
      // Opening of the PDF
  
      const pdfData = doc.output('datauristring');
      this.pdfSrc = pdfData;
  
      const pdfWindow = window.open();
  
      // Validation for PDF load
  
      if (pdfWindow) {
        pdfWindow.document.write('<iframe width="100%" height="100%" src="' + pdfData + '"></iframe>');
      } else {
        console.error('Failed to open PDF preview window.');
      }

    }
}
