import { Component, OnInit } from '@angular/core';
import { DataService } from 'src/app/services/data.service';
import { Product, ProductCategories, ProductTypes } from 'src/app/shared/product';
import { Jobs } from 'src/app/shared/jobs';
import { Order, OrderLine } from 'src/app/shared/order';
import { jsPDF } from 'jspdf';
import autoTable, { Row } from 'jspdf-autotable';
import { Observable, catchError, map, throwError } from 'rxjs';
import { HttpErrorResponse } from '@angular/common/http';
import { DatePipe, formatDate } from '@angular/common';
import { LoadsCarried } from 'src/app/shared/loadsCarried';
import { MileageFuel } from 'src/app/shared/mileage_fuel';
import { TruckData } from 'src/app/shared/mileage_fuel';


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
  productTypes: any[] = []; 
  productCategories: any[] = []; 
  selectedCategory: string = '';
  jobs: any[] = [];
  orders: OrderLine[] = [];
  users : string = "";
  originalJobs: any[] = [];
  loadsCarried : LoadsCarried[] = []
  mileageFuel : MileageFuel[] = [];
  truckData: TruckData[] = [];
  pdfSrc: string | null = null;


  GetProductType: ProductTypes =
  {
    product_Type_ID:"",
    name:"",
    description:""
  };

  GetProductCategory: ProductCategories =
  {
    product_Category_ID:"",
    name:"",
    description:""
  };

  constructor(private dataService: DataService, private datePipe: DatePipe) { }

  ngOnInit(): void {
    this.getProducts();
    this.GetJobs();
    this.getLoadsCarried();
    this.GetProductCategories();
    this.GetProductTypes();
    this.getOrders();

    this.getMileageFuel();
    this.getTotalSales();

  }

  // Methods-------------------------------------------------------------------------

  getProducts() {
    this.dataService.GetProducts().subscribe(result => {
      this.products = result;
    });
  }

  getOrders(){
    this.dataService.GetAllOrders().subscribe(result => {
      this.orders = result
      
      if(this.selectedCategory){
        this.dataService.GetProductCategories().subscribe(result => {
            this.orders = result;
            this.generateProductSalesReport();
        })
      }
      else {
        this.orders = [];
      }

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

  GetProductTypes() {
    this.dataService.GetProductTypes().subscribe(result => {
      this.productTypes = result; // Assign the retrieved product types directly to the "productTypes" array
      console.log(this.productTypes);
    });
  }
    
    GetProductCategories() {
      this.dataService.GetProductCategories().subscribe(result => {
        this.productCategories = result; // Assign the retrieved product types directly to the "productTypes" array
        console.log(this.productCategories);
      });
    }



  getLoadsCarried() {
    this.dataService.GetLoadsCarried().subscribe(result => {
      this.loadsCarried = result;
      console.log(this.loadsCarried)
    });
  }

  // getMileageFuel() {
  //   this.dataService.GetAllMileageFuel().subscribe(result => {
  //     this.mileageFuel = result;
  //   });
  // getMileageFuel() {
  //   this.dataService.GetAllMileageFuel().subscribe((result: TruckData[]) => {
  //     this.truckData = result; // Assign the API response to the truckData array
  //   });
  // }
  getMileageFuel() {
    this.dataService.GetAllMileageFuel().subscribe(result => {
      console.log('API response:', result);
  
      this.truckData = result; // Assign the API response to the truckData array
      console.log('Truck data:', this.truckData);
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

  // Generate Mileage/Fuel Report

  generateMileageFuelReport() {
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

    const header = ['Registration', 'Delivery', 'Mileage', 'Fuel Consumed'];

    const tableData: any[] = [];
    this.truckData.forEach(truck => {
      truck.mFList.forEach(delivery => {
        tableData.push([
          truck.registration,
          delivery.delivery_ID,
          delivery.mileage !== undefined ? delivery.mileage: 0,
          delivery.fuel !== null ? delivery.fuel : 0
        ]);
      });
    });
    

  
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


// Generate Product sales report

generateProductSalesReport(){

  const document = new jsPDF('landscape'); //instance of the jsPDF class

    const img = new Image();
    img.src = "assets/LTTLogo.jpg";
    document.addImage(img, 'JPG', 10, 5, 50, 30);

    // Text

    document.setFontSize(18);
    document.setTextColor(0, 79, 158);
    document.setFont('helvetica', 'bold');
    document.text('Product Sales Report', 230, 20);

    document.setFontSize(10);
    document.setTextColor(0, 0, 0);
    document.setFont('helvetica');
    const currentDate = new Date();
    document.text('Generated On: ' + currentDate.toLocaleDateString(), 230, 27);



    const header = ['Product Name', 'Category', 'Quantity Sold', 'Price/Quantity', 'Total (R)']

    const tableData = this.orders.map(order => {

      const filteredCategory = this.orders.filter(order => order.product.productCategory.name === this.selectedCategory);



      const totalPrice = order.product.quantity * order.product.product_Price;
  
      return [
        order.product.product_Name,
        order.product.productCategory.name,
        order.product.quantity, // Make sure this is accessing the correct property
        order.product.product_Price,
        totalPrice.toFixed(2)  // Format total price to two decimal places
      ];
    });
  

    const subTotal = tableData.reduce((total, row) => total + parseFloat(String(row[5])), 0);

    // Add an empty row if there is no data to avoid an issue with empty table rendering
    if (tableData.length === 0) {
      tableData.push(['No data', '', '', '', '', '', '']);
    }


   

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
