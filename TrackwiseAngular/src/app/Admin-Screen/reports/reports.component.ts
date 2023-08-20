import { Component, OnInit } from '@angular/core';
import { DataService } from 'src/app/services/data.service';
import { Product } from 'src/app/shared/product';
import { jsPDF } from 'jspdf';
import autoTable from 'jspdf-autotable';
import { LoadsCarried } from 'src/app/shared/loadsCarried';

import { ChartType, ChartOptions, ChartDataset } from 'chart.js';
import html2canvas from 'html2canvas';
import {Colors} from 'chart.js';
import { MatDatepickerInputEvent } from '@angular/material/datepicker';

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
    this.getTotalSales();
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


    selectedMonths='';
    startMonth: number | any;
    startYear: number | any;
    endMonth: number | any;
    endYear: number | any;
  
    shortMonthNames: string[] = [
      'Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun',
      'Jul', 'Aug', 'Sept', 'Oct', 'Nov', 'Dec'
    ];
  
    onStartDateSelected(event: MatDatepickerInputEvent<Date>): void {
      const selectedDate: any = event.value;
      this.startMonth = selectedDate.getMonth() + 1; // January is 0 in JavaScript
      this.startYear = selectedDate.getFullYear();
  
      // Get short month name based on the month number
      const startShortMonth = this.shortMonthNames[this.startMonth - 1];
    }
  
    onEndDateSelected(event: MatDatepickerInputEvent<Date>): void {
      const selectedDate: any = event.value;
      this.endMonth = selectedDate.getMonth() + 1;
      this.endYear = selectedDate.getFullYear();
  
      // Get short month name based on the month number
      const endShortMonth = this.shortMonthNames[this.endMonth - 1];
    }
   

    totalSalesData: any[] = [];
    salesArray: number[] = []; // Array to store total sales
    average: number[] = [];
    datesArray: Date[] = [];   // Array to store dates
    amountarray: any[] = [];
    selectedMonthsInput: string = ''; // Input for comma-separated months

    getTotalSales(): void {
      this.dataService.GetTotalSales().subscribe(
        data => {
          const startDate = new Date(this.startYear, this.startMonth - 1);
          const endDate = new Date(this.endYear, this.endMonth);
          
          // Filter data based on selected date range
          const filteredData = data.filter((sale: any) => {
            const saleDate = new Date(sale.date);
            return saleDate >= startDate && saleDate <= endDate;
          });

          this.salesArray = data.map((sale: any) => sale.total);
          // Calculate and set average sales
          const totalSalesSum = this.salesArray.reduce((sum, sale) => sum + sale, 0);
          const totalMonths = this.salesArray.length;
          const averageSales = totalSalesSum / totalMonths;
          this.average = Array(this.salesArray.length).fill(averageSales);

          // Update chart data
          this.lineChartData[0].data = filteredData.map((sale: any) => sale.total);
          this.lineChartData[1].data = this.average;
          this.lineChartLabels = filteredData.map((sale: any) => new Date(sale.date).toLocaleDateString('en-US', { year: 'numeric', month: 'short' }));
          

        },
        error => {
          console.error('Error fetching total sales:', error);
        }
      );
    }

    //TOTAL SALES GRAPH
    lineChartData: ChartDataset[] = [
      { data: this.salesArray, label: 'Sales' },
      { data: this.average, label: 'Average Sales'}];

  lineChartLabels: string[] = [];
  lineChartOptions = {
    responsive: true,
    
    plugins: {
      legend: {
        display: true,
      },
    },
    scales: {
      x: {
        display: true,
        title: {
          display: true,
          text: 'Date in months',
        },
      },
      y: {
        display: true,
        beginAtZero: true,
        title: {
          display: true,
          text: 'Sales Amount in (R)',
        },
      },
    },
    elements: {
      line: {
        tension: 0.5, // Adjust the tension value (between 0 and 1)
      },
      point: {
        radius: 4,
        borderWidth: 2,
        hoverRadius: 8,
        backgroundColor: 'rgba(255, 0, 0, 1)',
      },
    },
  };
  lineChartLegend = true;
  lineChartPlugins = [];
  lineChartType: ChartType = 'line';

  generateSalesReport() {
    this.getTotalSales();
    const doc = new jsPDF();
    // Image
    const img = new Image();
    img.src = "assets/LTTLogo.jpg";
    doc.addImage(img, 'JPG', 10, 5, 50, 30);
    // Text
    doc.setFontSize(18);
    doc.setTextColor(0, 79, 158);
    doc.setFont('helvetica', 'bold');
    doc.text('Total Sales Report', 130, 20);

    doc.setFontSize(10);
    doc.setTextColor(0, 0, 0);
    doc.setFont('helvetica');
    const currentDate = new Date();
    doc.text('Generated On: ' + currentDate.toLocaleDateString(), 130, 27);
    doc.text('Generated By: ' + name ,130, 32)

    let Data = document.getElementById('htmlData')!;
    let Graph = '';
  // Canvas Options

  html2canvas(Data).then(canvas => {
  
    // Convert canvas to image and add to PDF
    let Graph = canvas.toDataURL('image/png');
    const img1 = new Image();
    img1.src = Graph;
    
    doc.addImage(img1, 'PNG', 5, 50, 200, 150);
  
    // Opening of the PDF
    const pdfData = doc.output('datauristring');
    const pdfWindow = window.open();
  
    // Validation for PDF load
    if (pdfWindow) {
      pdfWindow.document.write('<iframe width="100%" height="100%" src="' + pdfData + '"></iframe>');
    } else {
      console.error('Failed to open PDF preview window.');
    }
  });
  }

}
