import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';


import { CustomersComponent } from './Admin-Screen/customers/customers.component';

import { DriversComponent } from './Admin-Screen/drivers/drivers.component';
import { AddDriverComponent } from './Admin-Screen/drivers/add-driver/add-driver.component';
import { EditDriverComponent } from './Admin-Screen/drivers/edit-driver/edit-driver.component';

import { AdminHomeComponent } from './Admin-Screen/admin-home/admin-home.component';

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
import { AdminProfileComponent } from './Admin-Screen/admin-profile/admin-profile.component';

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
import { ForgotpasswordComponent } from './password/forgotpassword/forgotpassword.component';
import { DocumentInformationComponent } from './Admin-Screen/jobs/document-information/document-information.component';

//Customer components-----------------------------------------------------------
import { CustomerProductComponent } from './Customer-Screen/customer-products/customer-products.component';
import { CartComponent } from './Customer-Screen/cart/cart.component';
import { AboutUsComponent } from './Customer-Screen/about-us/about-us.component';
import { CustomerOrdersComponent } from './Customer-Screen/customer-orders/customer-orders.component';
import { ProfileComponent } from './Customer-Screen/profile/profile.component';
import { CustomerHomeComponent } from './Customer-Screen/customer-home/customer-home.component';
import { JobDetailsComponent } from './Admin-Screen/jobs/job-details/job-details.component';
import { EditJobComponent } from './Admin-Screen/jobs/edit-job/edit-job.component';

//Client components--------------------------------------------------------------
import { ClientJobsComponent } from './Client-Screen/client-jobs/client-jobs.component';
import { ClientAddJobComponent } from './Client-Screen/client-add-job/client-add-job.component';
import { ClientEditJobComponent } from './Client-Screen/client-edit-job/client-edit-job.component';
import { ClientJobDetailsComponent } from './Client-Screen/client-job-details/client-job-details.component';
import { AdminGuard } from './Authentication/guards/adminGuard';
import { CustomerGuard } from './Authentication/guards/customerGuard';
import { ClientGuard } from './Authentication/guards/clientGuard';





const routes: Routes = [
  { path: 'Authentication/login', component: LoginComponent },
  { path: 'Authentication/register', component: RegisterComponent },

  { path: 'Admin-Screen/admin-home', component: AdminHomeComponent, canActivate: [AdminGuard] },
  { path: 'Admin-Screen/admin-profile', component: AdminProfileComponent, canActivate: [AdminGuard] },

  { path: 'Admin-Screen/customers', component: CustomersComponent, canActivate: [AdminGuard] },

  { path: 'Admin-Screen/drivers', component: DriversComponent, canActivate: [AdminGuard] },
  { path: 'Admin-Screen/drivers/add-driver', component: AddDriverComponent, canActivate: [AdminGuard] },
  { path: 'Admin-Screen/drivers/edit-driver/:driver_ID', component: EditDriverComponent, canActivate: [AdminGuard] },

  { path: 'Admin-Screen/orders', component: OrdersComponent, canActivate: [AdminGuard] },

  { path: 'Admin-Screen/reports', component: ReportsComponent, canActivate: [AdminGuard] },

  { path: 'Admin-Screen/suppliers', component: SuppliersComponent, canActivate: [AdminGuard] },
  { path: 'Admin-Screen/suppliers/add-supplier', component: AddSupplierComponent, canActivate: [AdminGuard] },
  { path: 'Admin-Screen/suppliers/edit-supplier/:supplier_ID', component: EditSupplierComponent, canActivate: [AdminGuard] },

  { path: 'Admin-Screen/clients', component: ClientsComponent, canActivate: [AdminGuard] },
  { path: 'Admin-Screen/clients/add-client', component: AddClientComponent, canActivate: [AdminGuard] },
  { path: 'Admin-Screen/clients/edit-client/:client_ID', component: EditClientComponent, canActivate: [AdminGuard] },

  { path: 'Admin-Screen/admins', component: AdminComponent, canActivate: [AdminGuard] },
  { path: 'Admin-Screen/admins/add-admin', component: AddAdminComponent, canActivate: [AdminGuard] },
  { path: 'Admin-Screen/admins/edit-admin/:admin_ID', component: EditAdminComponent, canActivate: [AdminGuard] },

  { path: 'Admin-Screen/trucks', component: TrucksComponent, canActivate: [AdminGuard] },
  { path: 'Admin-Screen/trucks/add-trucks', component: AddTrucksComponent, canActivate: [AdminGuard] },
  { path: 'Admin-Screen/trucks/edit-truck/:truckID', component: EditTruckComponent, canActivate: [AdminGuard] },

  { path: 'Admin-Screen/trailers', component: TrailersComponent, canActivate: [AdminGuard] },
  { path: 'Admin-Screen/trailers/add-trailers', component: AddTrailersComponent, canActivate: [AdminGuard] },
  { path: 'Admin-Screen/trailers/edit-trailer/:trailerID', component: EditTrailerComponent, canActivate: [AdminGuard] },

  { path: 'Admin-Screen/products', component: ProductsComponent, canActivate: [AdminGuard] },
  { path: 'Admin-Screen/products/add-product', component: AddProductComponent, canActivate: [AdminGuard] },
  { path: 'Admin-Screen/products/edit-product/:product_ID', component: EditProductComponent, canActivate: [AdminGuard] },

  {path: '', redirectTo: 'Customer-Screen/customer-home', pathMatch:'full'},

  //Job components
  {path: 'Admin-Screen/jobs', component: JobsComponent, canActivate:[AdminGuard]},
  {path: 'Admin-Screen/jobs/document-information/:delivery_ID', component: DocumentInformationComponent, canActivate:[AdminGuard]},

  {path: 'password/forgotpassword', component: ForgotpasswordComponent},
  {path: 'Admin-Screen/jobs/job-details/:job_ID/:userName', component: JobDetailsComponent, canActivate: [AdminGuard]},
  {path: 'Admin-Screen/jobs/edit-job', component:EditJobComponent, canActivate: [AdminGuard]},


  //Customer components--------------------------------------------------
  { path: 'Customer-Screen/customer-products', component: CustomerProductComponent},
  { path: 'Customer-Screen/customer-orders', component: CustomerOrdersComponent, canActivate: [CustomerGuard] },
  { path: 'Customer-Screen/about-us', component: AboutUsComponent },
  { path: 'Customer-Screen/cart', component: CartComponent, canActivate: [CustomerGuard] },
  { path: 'Customer-Screen/profile', component: ProfileComponent, canActivate: [CustomerGuard] },
  { path: 'Customer-Screen/customer-home', component: CustomerHomeComponent },

  // Client components-------------------------------------------------
  {path: 'Client-Screen/client-jobs', component: ClientJobsComponent, canActivate: [ClientGuard]},
  {path: 'Client-Screen/client-add-jobs', component: ClientAddJobComponent, canActivate: [ClientGuard]},
  {path: 'Client-Screen/client-edit-jobs', component: ClientEditJobComponent, canActivate: [ClientGuard]},
  {path: 'Client-Screen/client-job-details/:job_ID/:userName', component: ClientJobDetailsComponent, canActivate: [ClientGuard]},


];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
