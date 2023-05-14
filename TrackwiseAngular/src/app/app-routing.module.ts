import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';


import { CustomersComponent } from './Admin-Screen/customers/customers.component';
import { DriversComponent } from './Admin-Screen/drivers/drivers.component';
import { JobsComponent } from './Admin-Screen/jobs/jobs.component';
import { OrdersComponent } from './Admin-Screen/orders/orders.component';
import { ReportsComponent } from './Admin-Screen/reports/reports.component';
import { SuppliersComponent } from './Admin-Screen/suppliers/suppliers.component';
import { ClientsComponent } from './Admin-Screen/clients/clients.component';
import { TrucksComponent } from './Admin-Screen/trucks/trucks.component';
import { TrailersComponent } from './Admin-Screen/trailers/trailers.component';


const routes: Routes = [
  {path: 'Admin-Screen/customers', component: CustomersComponent},
  {path: 'Admin-Screen/drivers', component: DriversComponent},
  {path: 'Admin-Screen/jobs', component: JobsComponent},
  {path: 'Admin-Screen/orders', component: OrdersComponent},
  {path: 'Admin-Screen/reports', component: ReportsComponent},
  {path: 'Admin-Screen/suppliers', component: SuppliersComponent},
  {path: 'Admin-Screen/clients', component: ClientsComponent},
  {path: 'Admin-Screen/trucks', component: TrucksComponent},
  {path: 'Admin-Screen/trailers', component: TrailersComponent},

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
