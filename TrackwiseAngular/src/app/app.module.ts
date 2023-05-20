import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

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
import { HttpClientModule } from '@angular/common/http';
import { MaterialModule } from 'src/app/shared/material.module';

import { AddDriverComponent } from './Admin-Screen/drivers/add-driver/add-driver.component';
import { EditDriverComponent } from './Admin-Screen/drivers/edit-driver/edit-driver.component';

import { AddTrucksComponent } from './Admin-Screen/trucks/add-trucks/add-trucks.component';
import { EditTruckComponent } from './Admin-Screen/trucks/edit-truck/edit-truck.component';

import { AddTrailersComponent } from './Admin-Screen/trailers/add-trailers/add-trailers.component';
import { EditTrailerComponent } from './Admin-Screen/trailers/edit-trailer/edit-trailer.component';

import { AdminComponent } from './Admin-Screen/admin/admin.component';
import { AddAdminComponent } from './Admin-Screen/admin/add-admin/add-admin.component';
import { EditAdminComponent } from './Admin-Screen/admin/edit-admin/edit-admin.component';

import { AddClientComponent } from './Admin-Screen/clients/add-client/add-client.component';
import { EditClientComponent } from './Admin-Screen/clients/edit-client/edit-client.component';





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
    AddTrucksComponent,
    EditTruckComponent,
    AddTrailersComponent,
    EditTrailerComponent,
    AddDriverComponent,
    EditDriverComponent,
    AdminComponent,
    AddAdminComponent,
    EditAdminComponent,
    AddClientComponent,
    EditClientComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    CommonModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    MaterialModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
