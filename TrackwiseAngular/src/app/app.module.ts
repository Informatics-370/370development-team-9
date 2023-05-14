import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';

import { JobsComponent } from './Admin-Screen/jobs/jobs.component';
import { OrdersComponent } from './Admin-Screen/orders/orders.component';
import { SuppliersComponent } from './Admin-Screen/suppliers/suppliers.component';
import { DriversComponent } from './Admin-Screen/drivers/drivers.component';
import { ReportsComponent } from './Admin-Screen/reports/reports.component';
import { CustomersComponent } from './Admin-Screen/customers/customers.component';
import { ClientsComponent } from './Admin-Screen/clients/clients.component';
import { TrucksComponent } from './Admin-Screen/trucks/trucks.component';
import { TrailersComponent } from './Admin-Screen/trailers/trailers.component';

import { CommonModule } from '@angular/common';




@NgModule({
  declarations: [
    AppComponent,
    JobsComponent,
    OrdersComponent,
    SuppliersComponent,
    DriversComponent,
    ReportsComponent,
    CustomersComponent,
    ClientsComponent,
    TrucksComponent,
    TrailersComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    CommonModule,
  
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
