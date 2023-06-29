import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatIconModule } from '@angular/material/icon';
import { MatSidenavModule } from '@angular/material/sidenav';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { JobsComponent } from './Admin-Screen/jobs/jobs.component';
import { OrdersComponent } from './Admin-Screen/orders/orders.component';
import { MatToolbarModule } from '@angular/material/toolbar';
import { DriversComponent } from './Admin-Screen/drivers/drivers.component';

import { ReportsComponent } from './Admin-Screen/reports/reports.component';
import { CustomersComponent } from './Admin-Screen/customers/customers.component';


import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { MaterialModule } from 'src/app/shared/material.module';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatIconModule } from '@angular/material/icon';
import { MatBadgeModule } from '@angular/material/badge';
import { MatMenuModule } from '@angular/material/menu';

import { AddDriverComponent } from './Admin-Screen/drivers/add-driver/add-driver.component';
import { EditDriverComponent } from './Admin-Screen/drivers/edit-driver/edit-driver.component';

import { TrucksComponent } from './Admin-Screen/trucks/trucks.component';
import { AddTrucksComponent } from './Admin-Screen/trucks/add-trucks/add-trucks.component';
import { EditTruckComponent } from './Admin-Screen/trucks/edit-truck/edit-truck.component';

import { TrailersComponent } from './Admin-Screen/trailers/trailers.component';
import { AddTrailersComponent } from './Admin-Screen/trailers/add-trailers/add-trailers.component';
import { EditTrailerComponent } from './Admin-Screen/trailers/edit-trailer/edit-trailer.component';

import { AdminComponent } from './Admin-Screen/admin/admin.component';
import { AddAdminComponent } from './Admin-Screen/admin/add-admin/add-admin.component';
import { EditAdminComponent } from './Admin-Screen/admin/edit-admin/edit-admin.component';

import { ClientsComponent } from './Admin-Screen/clients/clients.component';
import { AddClientComponent } from './Admin-Screen/clients/add-client/add-client.component';
import { EditClientComponent } from './Admin-Screen/clients/edit-client/edit-client.component';

import { SuppliersComponent } from './Admin-Screen/suppliers/suppliers.component';
import { EditSupplierComponent } from './Admin-Screen/suppliers/edit-supplier/edit-supplier.component';
import { AddSupplierComponent } from './Admin-Screen/suppliers/add-supplier/add-supplier.component';

import { ProductsComponent } from './Admin-Screen/products/products.component';
import { AddProductComponent } from './Admin-Screen/products/add-product/add-product.component';
import { EditProductComponent } from './Admin-Screen/products/edit-product/edit-product.component';
import { LoginComponent } from './Authentication/login/login.component';
import { RegisterComponent } from './Authentication/register/register.component';


import { CustomerLandingPageComponent } from './Customer-Screen/customer-landing-page/customer-landing-page.component';
import { AboutUsComponent } from './Customer-Screen/about-us/about-us.component';
import { CartComponent } from './Customer-Screen/cart/cart.component';
import { CustomerOrdersComponent } from './Customer-Screen/customer-orders/customer-orders.component';




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
    EditSupplierComponent,
    AddSupplierComponent,
    ProductsComponent,
    AddProductComponent,
    EditProductComponent,
    LoginComponent,
    RegisterComponent,

    CustomerLandingPageComponent,
    CustomerOrdersComponent,
    CartComponent,
    AboutUsComponent

 
  ],
  imports: [
    BrowserModule,
    MatInputModule ,
    MatSidenavModule,
    MatIconModule,
    MatButtonModule,
    BrowserAnimationsModule,
    AppRoutingModule,
    MatToolbarModule,
    CommonModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    MaterialModule,
    MatToolbarModule,
    MatIconModule,
    MatBadgeModule,
    MatMenuModule,
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
