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
import { LoginUser } from '../shared/login-user';
import { User } from '../shared/user';
import { Router } from '@angular/router';
import { Job } from '../shared/job';

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

  isLoggedIn = false;
  isAdmin = false;
  isCustomer = false;
  isClient = false;
  isDriver = false;

  // TEMPORARY LOCALSTORAGE
  constructor(private httpClient: HttpClient, private router: Router) {

    if(!localStorage.getItem('product')){
      let product = [
        {
          "id": 1,
          "product_Name": "Oil",
          "description": "American",
          "product_Price": 499,
        },
        {
          "id": 2,
          "product_Name": "Grease VXP-2",
          "description": "American",
          "product_Price": 250,
        },
        {
        "id": 3,
        "product_Name": "Oil Filter",
        "description": "American",
        "product_Price": 365,
        }
      ]
      localStorage.setItem('product', JSON.stringify(product))
    }

   }

  /*LOGIN*/
  LoginUser(loginUser: LoginUser){
    return this.httpClient.post<User>(`${this.apiUrl}User/Login`, loginUser, this.httpOptions)
  }

  /* Logout */
    logout(){
    if(sessionStorage.getItem('User'))
    {
      this.isLoggedIn = false;
      this.isCustomer = false;
      this.isAdmin = false;
      this.isClient = false;
      this.isDriver = false;
      sessionStorage.removeItem('User');
      sessionStorage.removeItem('Role');
      sessionStorage.removeItem('Token');
      this.router.navigateByUrl('Authentication/login');
    }
  } 

  /*getRole */
  getRole(): void {
    var role = JSON.parse(sessionStorage.getItem("Role")!)
    console.log('Role:', role); // Add this line
    if (role == "Admin") {
      this.isLoggedIn = true;
      this.isAdmin = true;
      console.log('Admin',this.isAdmin);
    } else if(role == "Customer"){
      this.isLoggedIn = true;
      this.isCustomer = true;
      console.log('Customer',this.isCustomer);
    } else if(role == "Client"){
      this.isLoggedIn = true;
      this.isClient = true;
      console.log('Client',this.isCustomer);
    } else if(role == "Driver"){
      this.isLoggedIn = true;
      this.isDriver = true;
      console.log('Driver',this.isCustomer);
    }
  } 

  /*DRIVER SECTION*/
  GetDrivers(): Observable<any>{
    let token = sessionStorage.getItem('Token');
    let headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return this.httpClient.get(`${this.apiUrl}Driver/GetAllDrivers`, {headers})
    .pipe(map(result => result))
  }

  AddDriver(AddDriverReq: Driver): Observable<Driver>
  {
    let token = sessionStorage.getItem('Token');
    let headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return this.httpClient.post<Driver>(`${this.apiUrl}Driver/AddDriver/`, AddDriverReq, {headers})
    .pipe(map(result => result))

  }

  GetDriver(driver_ID: Number): Observable<Driver>
  {
    let token = sessionStorage.getItem('Token');
    let headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return this.httpClient.get<Driver>(`${this.apiUrl}Driver/GetDriver/${driver_ID}`, {headers} );
  }

  EditDriver(driver_ID: number , EditDriverReq: Driver):Observable<Driver>
  {
    let token = sessionStorage.getItem('Token');
    let headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
      return this.httpClient.put<Driver>(`${this.apiUrl}Driver/EditDriver/${driver_ID}`, EditDriverReq, {headers});
  }

  DeleteDriver(driver_ID: number):Observable<Driver>
  {
    let token = sessionStorage.getItem('Token');
    let headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
      return this.httpClient.delete<Driver>(`${this.apiUrl}Driver/DeleteDriver/${driver_ID}`, {headers});
  }

  /*TRUCK SECTION*/ 
  GetTrucks(): Observable<any>{
    let token = sessionStorage.getItem('Token');
    let headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return this.httpClient.get(`${this.apiUrl}Truck/GetAllTrucks`, {headers})
    .pipe(map(result => result))
  }

  AddTruck(AddTruckRequest: Truck): Observable<Truck>
  {
    let token = sessionStorage.getItem('Token');
    let headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return this.httpClient.post<Truck>(`${this.apiUrl}Truck/AddTruck/`, AddTruckRequest, {headers})
    .pipe(map(result => result))

  }

  GetTruck(truckID: Number): Observable<Truck>
  {
    let token = sessionStorage.getItem('Token');
    let headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return this.httpClient.get<Truck>(`${this.apiUrl}Truck/GetTruck/${truckID}`, {headers} );
  }

  EditTruck(truckID: number , EditTruckRequest: Truck):Observable<Truck>
  {
    let token = sessionStorage.getItem('Token');
    let headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
      return this.httpClient.put<Truck>(`${this.apiUrl}Truck/EditTruck/${truckID}`, EditTruckRequest, {headers});
  }

  DeleteTruck(truckID: number):Observable<Truck>
  {
    let token = sessionStorage.getItem('Token');
    let headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
      return this.httpClient.delete<Truck>(`${this.apiUrl}Truck/DeleteTruck/${truckID}`, {headers});
  }

  /*TRAILER SECTION*/ 
  GetTrailers(): Observable<any>{
    let token = sessionStorage.getItem('Token');
    let headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return this.httpClient.get(`${this.apiUrl}Trailer/GetAllTrailers`, {headers})
    .pipe(map(result => result))
  }

  AddTrailer(AddTrailerRequest: Trailer): Observable<Trailer>
  {
    let token = sessionStorage.getItem('Token');
    let headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return this.httpClient.post<Trailer>(`${this.apiUrl}Trailer/AddTrailer/`, AddTrailerRequest, {headers})
    .pipe(map(result => result))

  }

  GetTrailer(trailerID: Number): Observable<Trailer>
  {
    let token = sessionStorage.getItem('Token');
    let headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return this.httpClient.get<Trailer>(`${this.apiUrl}Trailer/GetTrailer/${trailerID}`, {headers} );
  }

  EditTrailer(trailerID: number , EditTrailerRequest: Trailer):Observable<Trailer>
  {
    let token = sessionStorage.getItem('Token');
    let headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
      return this.httpClient.put<Trailer>(`${this.apiUrl}Trailer/EditTrailer/${trailerID}`, EditTrailerRequest, {headers});
  }

  DeleteTrailer(trailerID: number):Observable<Trailer>
  {
    let token = sessionStorage.getItem('Token');
    let headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
      return this.httpClient.delete<Trailer>(`${this.apiUrl}Trailer/DeleteTrailer/${trailerID}`, {headers});
  }

  /*Admin SECTION*/
  GetAdmins(): Observable<any>{
    let token = sessionStorage.getItem('Token');
    let headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return this.httpClient.get(`${this.apiUrl}Admin/GetAllAdmin`, {headers})
    .pipe(map(result => result))
  }

  AddAdmin(AddAdminReq: Admin): Observable<Admin>
  {
    let token = sessionStorage.getItem('Token');
    let headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return this.httpClient.post<Admin>(`${this.apiUrl}Admin/AddAdmin/`, AddAdminReq, {headers})
    .pipe(map(result => result))

  }

  GetAdmin(admin_ID: Number): Observable<Admin>
  {
    let token = sessionStorage.getItem('Token');
    let headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return this.httpClient.get<Admin>(`${this.apiUrl}Admin/GetAdmin/${admin_ID}`, {headers});
  }

  EditAdmin(admin_ID: number , EditAdminReq: Admin):Observable<Admin>
  {
    let token = sessionStorage.getItem('Token');
    let headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
      return this.httpClient.put<Admin>(`${this.apiUrl}Admin/EditAdmin/${admin_ID}`, EditAdminReq, {headers});
  }

  DeleteAdmin(admin_ID: number):Observable<Admin>
  {
    let token = sessionStorage.getItem('Token');
    let headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
      return this.httpClient.delete<Admin>(`${this.apiUrl}Admin/DeleteAdmin/${admin_ID}`, {headers});
  }

  /*Client SECTION*/
  GetClients(): Observable<any>{
    let token = sessionStorage.getItem('Token');
    let headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return this.httpClient.get(`${this.apiUrl}Client/GetAllClients`, {headers})
    .pipe(map(result => result))
  }

  AddClient(AddClientReq: Client): Observable<Client>
  {
    let token = sessionStorage.getItem('Token');
    let headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return this.httpClient.post<Client>(`${this.apiUrl}Client/AddClient/`, AddClientReq, {headers})
    .pipe(map(result => result))

  }

  GetClient(client_ID: Number): Observable<Client>
  {
    let token = sessionStorage.getItem('Token');
    let headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return this.httpClient.get<Client>(`${this.apiUrl}Client/GetClient/${client_ID}` , {headers});
  }

  EditClient(client_ID: number , EditClientReq: Client):Observable<Client>
  {
    let token = sessionStorage.getItem('Token');
    let headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
      return this.httpClient.put<Client>(`${this.apiUrl}Client/EditClient/${client_ID}`, EditClientReq, {headers});
  }

  DeleteClient(client_ID: number):Observable<Client>
  {
    let token = sessionStorage.getItem('Token');
    let headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
      return this.httpClient.delete<Client>(`${this.apiUrl}Client/DeleteClient/${client_ID}`, {headers});
  }


  /*Supplier Section */

  GetSuppliers(): Observable<any>{
    let token = sessionStorage.getItem('Token');
    let headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return this.httpClient.get(`${this.apiUrl}Supplier/GetAllSuppliers`, {headers})
    .pipe(map(result => result))
  }

  AddSupplier(AddSupplierReq: Supplier): Observable<Supplier>
  {
    let token = sessionStorage.getItem('Token');
    let headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return this.httpClient.post<Supplier>(`${this.apiUrl}Supplier/AddSupplier/`, AddSupplierReq, {headers})
    .pipe(map(result => result))
  }

  GetSupplier(supplier_ID: Number): Observable<Supplier>
  {
    let token = sessionStorage.getItem('Token');
    let headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return this.httpClient.get<Supplier>(`${this.apiUrl}Supplier/GetSupplier/${supplier_ID}` , {headers});
  }

  EditSupplier(supplier_ID: number , EditSupplierReq: Supplier):Observable<Supplier>
  {
    let token = sessionStorage.getItem('Token');
    let headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
      return this.httpClient.put<Supplier>(`${this.apiUrl}Supplier/EditSupplier/${supplier_ID}`, EditSupplierReq, {headers});
  }

  DeleteSupplier(supplier_ID: number):Observable<Supplier>
  {
    let token = sessionStorage.getItem('Token');
    let headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
      return this.httpClient.delete<Supplier>(`${this.apiUrl}Supplier/DeleteSupplier/${supplier_ID}`, {headers});
  }

  /*JOBS*/
  GetJobs(): Observable<any>{
    let token = sessionStorage.getItem('Token');
    let headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return this.httpClient.get(`${this.apiUrl}Job/GetAllAdminJobs`, {headers})
    .pipe(map(result => result))
  }

  CreateJob(AddProductReq: Job): Observable<Job>
  {
    let token = sessionStorage.getItem('Token');
    let headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return this.httpClient.post<Job>(`${this.apiUrl}Job/CreateJob/`, AddProductReq, {headers})
    .pipe(map(result => result))
  }

  /*PASSWORD */
  resetPassword(email: string, token: string, newPassword: string): Observable<any> {
    const resetPasswordUrl = `${this.apiUrl}User/reset-password`;
    const body = { email: email, token: token, newPassword: newPassword };
    return this.httpClient.post<any>(resetPasswordUrl, body);
  }

  forgotPassword(email: string): Observable<any> {
    const forgotPasswordUrl = `${this.apiUrl}User/forgot-password`;
    return this.httpClient.post<any>(forgotPasswordUrl, { email: email });
  }



  /*Product Section */

  GetProducts(): Observable<any>{
    let token = sessionStorage.getItem('Token');
    let headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return this.httpClient.get(`${this.apiUrl}Product/GetAllProducts`, {headers})
    .pipe(map(result => result))
  }

  AddProduct(AddProductReq: Product): Observable<Product>
  {
    let token = sessionStorage.getItem('Token');
    let headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return this.httpClient.post<Product>(`${this.apiUrl}Product/AddProduct/`, AddProductReq, {headers})
    .pipe(map(result => result))
  }

  GetProduct(product_ID: Number): Observable<Product>
  {
    let token = sessionStorage.getItem('Token');
    let headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return this.httpClient.get<Product>(`${this.apiUrl}Product/GetProduct/${product_ID}`, {headers});
  }

  EditProduct(product_ID: number , EditProductReq: Product):Observable<Product>
  {
    let token = sessionStorage.getItem('Token');
    let headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
      return this.httpClient.put<Product>(`${this.apiUrl}Product/EditProduct/${product_ID}`, EditProductReq, {headers});
  }

  DeleteProduct(product_ID: number):Observable<Product>
  {
    let token = sessionStorage.getItem('Token');
    let headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
      return this.httpClient.delete<Product>(`${this.apiUrl}Product/DeleteProduct/${product_ID}`, {headers});
  }
}