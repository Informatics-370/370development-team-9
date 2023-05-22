import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { map, Observable, Subject } from 'rxjs';
import { Driver } from '../shared/driver';
import { Truck } from 'src/app/shared/truck';
import { Trailer } from '../shared/trailer';
import { Admin } from '../shared/admin';
import { Client } from '../shared/client';
import { Supplier } from '../shared/supplier';
import { Product } from '../shared/product';

@Injectable({
  providedIn: 'root'
})
export class DataService {

  apiUrl = 'https://localhost:7049/api/'

  httpOptions ={
    headers: new HttpHeaders({
      ContentType: 'application/json'
    })
  }

  constructor(private httpClient: HttpClient) { }

  /*DRIVER SECTION*/
  GetDrivers(): Observable<any>{
    return this.httpClient.get(`${this.apiUrl}Driver/GetAllDrivers`)
    .pipe(map(result => result))
  }

  AddDriver(AddDriverReq: Driver): Observable<Driver>
  {
    return this.httpClient.post<Driver>(`${this.apiUrl}Driver/AddDriver/`, AddDriverReq)
    .pipe(map(result => result))

  }

  GetDriver(driver_ID: Number): Observable<Driver>
  {
    return this.httpClient.get<Driver>(`${this.apiUrl}Driver/GetDriver/${driver_ID}` );
  }

  EditDriver(driver_ID: number , EditDriverReq: Driver):Observable<Driver>
  {
      return this.httpClient.put<Driver>(`${this.apiUrl}Driver/EditDriver/${driver_ID}`, EditDriverReq);
  }

  DeleteDriver(driver_ID: number):Observable<Driver>
  {
      return this.httpClient.delete<Driver>(`${this.apiUrl}Driver/DeleteDriver/${driver_ID}`);
  }

  /*TRUCK SECTION*/ 
  GetTrucks(): Observable<any>{
    return this.httpClient.get(`${this.apiUrl}Truck/GetAllTrucks`)
    .pipe(map(result => result))
  }

  AddTruck(AddTruckRequest: Truck): Observable<Truck>
  {
    return this.httpClient.post<Truck>(`${this.apiUrl}Truck/AddTruck/`, AddTruckRequest)
    .pipe(map(result => result))

  }

  GetTruck(truckID: Number): Observable<Truck>
  {
    return this.httpClient.get<Truck>(`${this.apiUrl}Truck/GetTruck/${truckID}` );
  }

  EditTruck(truckID: number , EditTruckRequest: Truck):Observable<Truck>
  {
      return this.httpClient.put<Truck>(`${this.apiUrl}Truck/EditTruck/${truckID}`, EditTruckRequest);
  }

  DeleteTruck(truckID: number):Observable<Truck>
  {
      return this.httpClient.delete<Truck>(`${this.apiUrl}Truck/DeleteTruck/${truckID}`);
  }

  /*TRAILER SECTION*/ 
  GetTrailers(): Observable<any>{
    return this.httpClient.get(`${this.apiUrl}Trailer/GetAllTrailers`)
    .pipe(map(result => result))
  }

  AddTrailer(AddTrailerRequest: Trailer): Observable<Trailer>
  {
    return this.httpClient.post<Trailer>(`${this.apiUrl}Trailer/AddTrailer/`, AddTrailerRequest)
    .pipe(map(result => result))

  }

  GetTrailer(trailerID: Number): Observable<Trailer>
  {
    return this.httpClient.get<Trailer>(`${this.apiUrl}Trailer/GetTrailer/${trailerID}` );
  }

  EditTrailer(trailerID: number , EditTrailerRequest: Trailer):Observable<Trailer>
  {
      return this.httpClient.put<Trailer>(`${this.apiUrl}Trailer/EditTrailer/${trailerID}`, EditTrailerRequest);
  }

  DeleteTrailer(trailerID: number):Observable<Trailer>
  {
      return this.httpClient.delete<Trailer>(`${this.apiUrl}Trailer/DeleteTrailer/${trailerID}`);
  }

  /*Admin SECTION*/
  GetAdmins(): Observable<any>{
    return this.httpClient.get(`${this.apiUrl}Admin/GetAllAdmin`)
    .pipe(map(result => result))
  }

  AddAdmin(AddAdminReq: Admin): Observable<Admin>
  {
    return this.httpClient.post<Admin>(`${this.apiUrl}Admin/AddAdmin/`, AddAdminReq)
    .pipe(map(result => result))

  }

  GetAdmin(admin_ID: Number): Observable<Admin>
  {
    return this.httpClient.get<Admin>(`${this.apiUrl}Admin/GetAdmin/${admin_ID}` );
  }

  EditAdmin(admin_ID: number , EditAdminReq: Admin):Observable<Admin>
  {
      return this.httpClient.put<Admin>(`${this.apiUrl}Admin/EditAdmin/${admin_ID}`, EditAdminReq);
  }

  DeleteAdmin(admin_ID: number):Observable<Admin>
  {
      return this.httpClient.delete<Admin>(`${this.apiUrl}Admin/DeleteAdmin/${admin_ID}`);
  }

  /*Client SECTION*/
  GetClients(): Observable<any>{
    return this.httpClient.get(`${this.apiUrl}Client/GetAllClients`)
    .pipe(map(result => result))
  }

  AddClient(AddClientReq: Client): Observable<Client>
  {
    return this.httpClient.post<Client>(`${this.apiUrl}Client/AddClient/`, AddClientReq)
    .pipe(map(result => result))

  }

  GetClient(client_ID: Number): Observable<Client>
  {
    return this.httpClient.get<Client>(`${this.apiUrl}Client/GetDriver/${client_ID}` );
  }

  EditClient(client_ID: number , EditClientReq: Client):Observable<Client>
  {
      return this.httpClient.put<Client>(`${this.apiUrl}Client/EditClient/${client_ID}`, EditClientReq);
  }

  DeleteClient(client_ID: number):Observable<Client>
  {
      return this.httpClient.delete<Client>(`${this.apiUrl}Client/DeleteClient/${client_ID}`);
  }


  /*Supplier Section */

  GetSuppliers(): Observable<any>{
    return this.httpClient.get(`${this.apiUrl}Supplier/GetAllSuppliers`)
    .pipe(map(result => result))
  }

  AddSupplier(AddSupplierReq: Supplier): Observable<Supplier>
  {
    return this.httpClient.post<Supplier>(`${this.apiUrl}Supplier/AddSupplier/`, AddSupplierReq)
    .pipe(map(result => result))
  }

  GetSupplier(supplier_ID: Number): Observable<Supplier>
  {
    return this.httpClient.get<Supplier>(`${this.apiUrl}Supplier/GetSupplier/${supplier_ID}` );
  }

  EditSupplier(supplier_ID: number , EditSupplierReq: Supplier):Observable<Supplier>
  {
      return this.httpClient.put<Supplier>(`${this.apiUrl}Supplier/EditSupplier/${supplier_ID}`, EditSupplierReq);
  }

  DeleteSupplier(supplier_ID: number):Observable<Supplier>
  {
      return this.httpClient.delete<Supplier>(`${this.apiUrl}Supplier/DeleteSupplier/${supplier_ID}`);
  }



  /*Product Section */

  GetProducts(): Observable<any>{
    return this.httpClient.get(`${this.apiUrl}Product/GetAllProducts`)
    .pipe(map(result => result))
  }

  AddProduct(AddProductReq: Product): Observable<Product>
  {
    return this.httpClient.post<Product>(`${this.apiUrl}Product/AddProduct/`, AddProductReq)
    .pipe(map(result => result))
  }

  GetProduct(product_ID: Number): Observable<Product>
  {
    return this.httpClient.get<Product>(`${this.apiUrl}Product/GetProduct/${product_ID}` );
  }

  EditProduct(product_ID: number , EditProductReq: Product):Observable<Product>
  {
      return this.httpClient.put<Product>(`${this.apiUrl}Product/EditProduct/${product_ID}`, EditProductReq);
  }

  DeleteProduct(product_ID: number):Observable<Product>
  {
      return this.httpClient.delete<Product>(`${this.apiUrl}Product/DeleteProduct/${product_ID}`);
  }
}