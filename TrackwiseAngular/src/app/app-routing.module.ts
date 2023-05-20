import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';


import { CustomersComponent } from './Admin-Screen/customers/customers.component';

import { DriversComponent } from './Admin-Screen/drivers/drivers.component';
import { AddDriverComponent } from './Admin-Screen/drivers/add-driver/add-driver.component';
import { EditDriverComponent } from './Admin-Screen/drivers/edit-driver/edit-driver.component';

import { JobsComponent } from './Admin-Screen/jobs/jobs.component';

import { OrdersComponent } from './Admin-Screen/orders/orders.component';

import { ReportsComponent } from './Admin-Screen/reports/reports.component';

import { SuppliersComponent } from './Admin-Screen/suppliers/suppliers.component';

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


const routes: Routes = [
  {path: 'Admin-Screen/customers', component: CustomersComponent},

  {path: 'Admin-Screen/drivers', component: DriversComponent},
  {path: 'Admin-Screen/drivers/add-driver', component: AddDriverComponent},
  {path: 'Admin-Screen/drivers/edit-driver/:driver_ID', component:EditDriverComponent},

  {path: 'Admin-Screen/jobs', component: JobsComponent},

  {path: 'Admin-Screen/orders', component: OrdersComponent},

  {path: 'Admin-Screen/reports', component: ReportsComponent},

  {path: 'Admin-Screen/suppliers', component: SuppliersComponent},

  {path: 'Admin-Screen/clients', component: ClientsComponent},
  {path: 'Admin-Screen/clients/add-client', component: AddClientComponent},
  {path: 'Admin-Screen/clients/edit-client/:clientID', component:EditClientComponent},

  {path: 'Admin-Screen/admin', component: AdminComponent},
  {path: 'Admin-Screen/admin/add-admin', component: AddAdminComponent},
  {path: 'Admin-Screen/admin/edit-admin/:adminID', component:EditAdminComponent},

  {path: 'Admin-Screen/trucks', component: TrucksComponent},
  {path: 'Admin-Screen/trucks/add-trucks', component: AddTrucksComponent},
  {path: 'Admin-Screen/trucks/edit-truck/:truckID', component:EditTruckComponent},

  {path: 'Admin-Screen/trailers', component: TrailersComponent},
  {path: 'Admin-Screen/trailers/add-trailers', component: AddTrailersComponent},
  {path: 'Admin-Screen/trailers/edit-trailer/:trailerID', component: EditTrailerComponent},

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
