import { Component, OnInit } from '@angular/core';
import { DataService } from 'src/app/services/data.service';
import { Product } from 'src/app/shared/product';
import { jsPDF } from 'jspdf';
import autoTable from 'jspdf-autotable';
import { LoadsCarried } from 'src/app/shared/loadsCarried';

@Component({
  selector: 'app-reports',
  templateUrl: './reports.component.html',
  styleUrls: ['./reports.component.scss']
})
export class ReportsComponent implements OnInit {
  products: Product[] = [];
  loadsCarried : LoadsCarried[] = []
  pdfSrc: string | null = null;

  constructor(private dataService: DataService) { }

  ngOnInit(): void {
    this.getProducts();
    this.getLoadsCarried();
  }

  // Methods-------------------------------------------------------------------------

  getProducts() {
    this.dataService.GetProducts().subscribe(result => {
      this.products = result;
    });
  }

  getLoadsCarried() {
    this.dataService.GetLoadsCarried().subscribe(result => {
      this.loadsCarried = result;
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
