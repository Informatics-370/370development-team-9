import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';


import { CustomersComponent } from './Admin-Screen/customers/customers.component';

import { DriversComponent } from './Admin-Screen/drivers/drivers.component';
import { AddDriverComponent } from './Admin-Screen/drivers/add-driver/add-driver.component';
import { EditDriverComponent } from './Admin-Screen/drivers/edit-driver/edit-driver.component';



import { OrdersComponent } from './Admin-Screen/orders/orders.component';

import { ReportsComponent } from './Admin-Screen/reports/reports.component';

import { SuppliersComponent } from './Admin-Screen/suppliers/suppliers.component';
import { AddSupplierComponent } from './Admin-Screen/suppliers/add-supplier/add-supplier.component';
import { EditSupplierComponent } from './Admin-Screen/suppliers/edit-supplier/edit-supplier.component';


import { ClientsComponent } from './Admin-Screen/clients/clients.component';
import { AddClientComponent } from './Admin-Screen/clients/add-client/add-client.component';
import { EditClientComponent } from './Admin-Screen/clients/edit-client/edit-client.component';
import { AdminComponent } from './Admin-Screen/admin/admin.component';
import { AddAdminComponent } from './Admin-Screen/admin/add-admin/add-admin.component';
import { EditAdminComponent } from './Admin-Screen/admin/edit-admin/edit-admin.component';

import { TrucksComponent } from './Admin-Screen/trucks/trucks.component';
import { AddTrucksComponent } from './Admin-Screen/trucks/add-trucks/add-trucks.component';
import { EditTruckComponent } from './Admin-Screen/trucks/edit-truck/edit-truck.component';

import { TrailersComponent } from './Admin-Screen/trailers/trailers.component';
import { AddTrailersComponent } from './Admin-Screen/trailers/add-trailers/add-trailers.component';
import { EditTrailerComponent } from './Admin-Screen/trailers/edit-trailer/edit-trailer.component';
import { ProductsComponent } from './Admin-Screen/products/products.component';
import { AddProductComponent } from './Admin-Screen/products/add-product/add-product.component';
import { EditProductComponent } from './Admin-Screen/products/edit-product/edit-product.component';
import { LoginComponent } from './Authentication/login/login.component';
import { RegisterComponent } from './Authentication/register/register.component';

//Job components
import { JobsComponent } from './Admin-Screen/jobs/jobs.component';
import { CreateJobComponent } from './Admin-Screen/jobs/create-job/create-job.component';
import { ForgotPasswordComponent } from './forgot-password/forgot-password.component';


//Job components
import { JobsComponent } from './Admin-Screen/jobs/jobs.component';
import { CreateJobComponent } from './Admin-Screen/jobs/create-job/create-job.component';
import { ForgotPasswordComponent } from './forgot-password/forgot-password.component';



//Customer components-----------------------------------------------------------
import { CustomerProductComponent } from './Customer-Screen/customer-products/customer-products.component';
import { CartComponent } from './Customer-Screen/cart/cart.component';
import { AboutUsComponent } from './Customer-Screen/about-us/about-us.component';
import { CustomerOrdersComponent } from './Customer-Screen/customer-orders/customer-orders.component';
import { ProfileComponent } from './Customer-Screen/profile/profile.component';
import { CustomerHomeComponent } from './Customer-Screen/customer-home/customer-home.component';



const routes: Routes = [
  {path: 'Authentication/login', component: LoginComponent},
  {path: 'Authentication/register', component: RegisterComponent},



  {path: 'Admin-Screen/customers', component: CustomersComponent},

  {path: 'Admin-Screen/drivers', component: DriversComponent},
  {path: 'Admin-Screen/drivers/add-driver', component: AddDriverComponent},
  {path: 'Admin-Screen/drivers/edit-driver/:driver_ID', component:EditDriverComponent},

  {path: 'Admin-Screen/orders', component: OrdersComponent},

  {path: 'Admin-Screen/reports', component: ReportsComponent},

  {path: 'Admin-Screen/suppliers', component: SuppliersComponent},
  {path: 'Admin-Screen/suppliers/add-supplier', component: AddSupplierComponent},
  {path: 'Admin-Screen/suppliers/edit-supplier/:supplier_ID', component:EditSupplierComponent},

  {path: 'Admin-Screen/clients', component: ClientsComponent},
  {path: 'Admin-Screen/clients/add-client', component: AddClientComponent},
  {path: 'Admin-Screen/clients/edit-client/:client_ID', component:EditClientComponent},

  {path: 'Admin-Screen/admins', component: AdminComponent},
  {path: 'Admin-Screen/admins/add-admin', component: AddAdminComponent},
  {path: 'Admin-Screen/admins/edit-admin/:admin_ID', component:EditAdminComponent},

  {path: 'Admin-Screen/trucks', component: TrucksComponent},
  {path: 'Admin-Screen/trucks/add-trucks', component: AddTrucksComponent},
  {path: 'Admin-Screen/trucks/edit-truck/:truckID', component:EditTruckComponent},

  {path: 'Admin-Screen/trailers', component: TrailersComponent},
  {path: 'Admin-Screen/trailers/add-trailers', component: AddTrailersComponent},
  {path: 'Admin-Screen/trailers/edit-trailer/:trailerID', component: EditTrailerComponent},

  {path: 'Admin-Screen/products', component: ProductsComponent},
  {path: 'Admin-Screen/products/add-product', component: AddProductComponent},
  {path: 'Admin-Screen/products/edit-product/:product_ID', component: EditProductComponent},

  {path: '', redirectTo: 'Authentication/login', pathMatch:'full'},

  //Job components
  {path: 'Admin-Screen/jobs', component: JobsComponent},
  {path: 'Admin-Screen/jobs/create-job', component: CreateJobComponent},
  { path: 'reset-password', component: ForgotPasswordComponent },

  //Customer components--------------------------------------------------
  {path: 'Customer-Screen/customer-products', component: CustomerProductComponent},
  {path: 'Customer-Screen/customer-orders', component: CustomerOrdersComponent},
  {path: 'Customer-Screen/about-us', component: AboutUsComponent},
  {path: 'Customer-Screen/cart', component: CartComponent},
  {path: 'Customer-Screen/profile', component: ProfileComponent},
  {path: 'Customer-Screen/customer-home', component: CustomerHomeComponent},




];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
