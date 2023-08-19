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
import { Customer } from '../shared/customer';
import { User } from '../shared/user';
import { Router } from '@angular/router';
import { Order } from '../shared/order';
import { OrderLines } from '../shared/orderLines';
import { RegisterUser } from '../shared/register';
import { Job } from '../shared/job';
import { CardPayment, CheckoutRequest, NewCard } from '../shared/cardPayment';
import { Forgotpass } from '../shared/forgotpass';
import { Document } from '../shared/document';
import { Weight } from '../shared/weight';

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

  Register( registerUser: RegisterUser ) {
    return this.httpClient.post(`${this.apiUrl}User/Register`, registerUser);
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
      sessionStorage.removeItem('cartItem')
      this.router.navigateByUrl('Authentication/login');
    }
  } 

  revertToLogin(){
    if(this.isLoggedIn == false)
    {
      this.router.navigateByUrl('Authentication/login');
    }
  }

  /*getRole */
  getRole(): void {
    var role = JSON.parse(sessionStorage.getItem("Role")!)
    if (role == "Admin") {
      this.isLoggedIn = true;
      this.isAdmin = true;
    } else if(role == "Customer"){
      this.isLoggedIn = true;
      this.isCustomer = true;
    } else if(role == "Client"){
      this.isLoggedIn = true;
      this.isClient = true;
    } else if(role == "Driver"){
      this.isLoggedIn = true;
      this.isDriver = true;
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

  GetDriver(driver_ID: string): Observable<Driver>
  {
    let token = sessionStorage.getItem('Token');
    let headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return this.httpClient.get<Driver>(`${this.apiUrl}Driver/GetDriver/${driver_ID}`, {headers} );
  }

  EditDriver(driver_ID: string , EditDriverReq: Driver):Observable<Driver>
  {
    let token = sessionStorage.getItem('Token');
    let headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
      return this.httpClient.put<Driver>(`${this.apiUrl}Driver/EditDriver/${driver_ID}`, EditDriverReq, {headers});
  }

  DeleteDriver(driver_ID: string):Observable<Driver>
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

  GetTruck(truckID: string): Observable<Truck>
  {
    let token = sessionStorage.getItem('Token');
    let headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return this.httpClient.get<Truck>(`${this.apiUrl}Truck/GetTruck/${truckID}`, {headers} );
  }

  EditTruck(truckID: string , EditTruckRequest: Truck):Observable<Truck>
  {
    let token = sessionStorage.getItem('Token');
    let headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
      return this.httpClient.put<Truck>(`${this.apiUrl}Truck/EditTruck/${truckID}`, EditTruckRequest, {headers});
  }

  DeleteTruck(truckID: string):Observable<Truck>
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

  GetTrailer(trailerID: string): Observable<Trailer>
  {
    let token = sessionStorage.getItem('Token');
    let headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return this.httpClient.get<Trailer>(`${this.apiUrl}Trailer/GetTrailer/${trailerID}`, {headers} );
  }

  EditTrailer(trailerID: string , EditTrailerRequest: Trailer):Observable<Trailer>
  {
    let token = sessionStorage.getItem('Token');
    let headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
      return this.httpClient.put<Trailer>(`${this.apiUrl}Trailer/EditTrailer/${trailerID}`, EditTrailerRequest, {headers});
  }

  DeleteTrailer(trailerID: string):Observable<Trailer>
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

  GetAdmin(admin_ID: string): Observable<Admin>
  {
    let token = sessionStorage.getItem('Token');
    let headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return this.httpClient.get<Admin>(`${this.apiUrl}Admin/GetAdmin/${admin_ID}`, {headers});
  }

  EditAdmin(admin_ID: string , EditAdminReq: Admin):Observable<Admin>
  {
    let token = sessionStorage.getItem('Token');
    let headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
      return this.httpClient.put<Admin>(`${this.apiUrl}Admin/EditAdmin/${admin_ID}`, EditAdminReq, {headers});
  }

  DeleteAdmin(admin_ID: string):Observable<Admin>
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

  GetClient(client_ID: string): Observable<Client>
  {
    let token = sessionStorage.getItem('Token');
    let headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return this.httpClient.get<Client>(`${this.apiUrl}Client/GetClient/${client_ID}` , {headers});
  }

  EditClient(client_ID: string , EditClientReq: Client):Observable<Client>
  {
    let token = sessionStorage.getItem('Token');
    let headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
      return this.httpClient.put<Client>(`${this.apiUrl}Client/EditClient/${client_ID}`, EditClientReq, {headers});
  }

  DeleteClient(client_ID: string):Observable<Client>
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

  GetSupplier(supplier_ID: string): Observable<Supplier>
  {
    let token = sessionStorage.getItem('Token');
    let headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return this.httpClient.get<Supplier>(`${this.apiUrl}Supplier/GetSupplier/${supplier_ID}` , {headers});
  }

  EditSupplier(supplier_ID: string , EditSupplierReq: Supplier):Observable<Supplier>
  {
    let token = sessionStorage.getItem('Token');
    let headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
      return this.httpClient.put<Supplier>(`${this.apiUrl}Supplier/EditSupplier/${supplier_ID}`, EditSupplierReq, {headers});
  }

  DeleteSupplier(supplier_ID: string):Observable<Supplier>
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

  GetClientJobs(): Observable<any>{
    let token = sessionStorage.getItem('Token');
    let headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return this.httpClient.get(`${this.apiUrl}Job/GetAllClientJobs`, {headers})
    .pipe(map(result => result))
  }

  GetJob(job_ID: string): Observable<Job>
  {
    let token = sessionStorage.getItem('Token');
    let headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return this.httpClient.get<Job>(`${this.apiUrl}Job/GetJob/${job_ID}`, {headers} );
  }
  
  GetDocuments(document_ID : string): Observable<any>
  {
    let token = sessionStorage.getItem('Token');
    let headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return this.httpClient.get<any>(`${this.apiUrl}Job/DeliveryDocuments/${document_ID}`, {headers} );
  }

  UpdateActualWeight(delivery_ID: string, request:Weight): Observable<any> {
    return this.httpClient.put<any>(`${this.apiUrl}Job/Updateweight/${delivery_ID}`, request);
  }
  
  CreateJob(AddJob: Job): Observable<Job>
  {
    let token = sessionStorage.getItem('Token');
    let headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return this.httpClient.post<Job>(`${this.apiUrl}Job/CreateJob/`, AddJob, {headers})
    .pipe(map(result => result))
  }

  /*PASSWORD */
  forgotPassword(email: Forgotpass) {
    console.log(email);
    return this.httpClient.post<any>(`${this.apiUrl}Mail/ForgotPasswordEmail`,email,this.httpOptions );
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

  GetProduct(product_ID: string): Observable<Product>
  {
    let token = sessionStorage.getItem('Token');
    let headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return this.httpClient.get<Product>(`${this.apiUrl}Product/GetProduct/${product_ID}`, {headers});
  }

  EditProduct(product_ID: string , EditProductReq: Product):Observable<Product>
  {
    let token = sessionStorage.getItem('Token');
    let headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
      return this.httpClient.put<Product>(`${this.apiUrl}Product/EditProduct/${product_ID}`, EditProductReq, {headers});
  }

  DeleteProduct(product_ID: string):Observable<Product>
  {
    let token = sessionStorage.getItem('Token');
    let headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
      return this.httpClient.delete<Product>(`${this.apiUrl}Product/DeleteProduct/${product_ID}`, {headers});
  }

  GetProductTypes(): Observable<any>{
    let token = sessionStorage.getItem('Token'); // Retrieve the token from localStorage
    let headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return this.httpClient.get(`${this.apiUrl}Product/GetProductType`,{headers})
    .pipe(map(result => result))
  }

  GetProductCategories(): Observable<any>{
    let token = sessionStorage.getItem('Token'); // Retrieve the token from localStorage
    let headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return this.httpClient.get(`${this.apiUrl}Product/GetProductCategory`,{headers})
    .pipe(map(result => result))
  }

  /*Customer Section*/
  GetCustomers(): Observable<any>{
    let token = sessionStorage.getItem('Token');
    let headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return this.httpClient.get(`${this.apiUrl}Customer/GetAllCustomers`, {headers})
    .pipe(map(result => result))
  }

  GetCustomer(customer_ID: string): Observable<Customer>
  {
    let token = sessionStorage.getItem('Token');
    let headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return this.httpClient.get<Customer>(`${this.apiUrl}Customer/GetCustomer/${customer_ID}`, {headers});
  }

  GetCustomerProfile(): Observable<Customer>
  {
    let token = sessionStorage.getItem('Token');
    let headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return this.httpClient.get<Customer>(`${this.apiUrl}Customer/GetCustomerProfile`, {headers});
  }

  EditCustomer(customer_ID: string , EditCustomerReq: Customer):Observable<Customer>
  {
    let token = sessionStorage.getItem('Token');
    let headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return this.httpClient.put<Customer>(`${this.apiUrl}Customer/EditCustomer/${customer_ID}`, EditCustomerReq, {headers});
  }

  EditCustomerProfile(editCustomerProfile:any):Observable<Customer>
  {
    let token = sessionStorage.getItem('Token');
    let headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return this.httpClient.put<Customer>(`${this.apiUrl}Customer/EditCustomerProfile`, editCustomerProfile, {headers});
  }

  DeleteCustomer(customer_ID: string):Observable<Customer>
  {
    let token = sessionStorage.getItem('Token');
    let headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return this.httpClient.delete<Customer>(`${this.apiUrl}Customer/DeleteCustomer/${customer_ID}`, {headers});
  }

  /*Order Section */

  CreateOrder(CheckoutRequest: CheckoutRequest): Observable<OrderLines>
  {
    let token = sessionStorage.getItem('Token');
    let headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return this.httpClient.post<OrderLines>(`${this.apiUrl}Order/CreateOrder/`, CheckoutRequest, {headers})
    .pipe(map(result => result))
  }

  GetAllOrders(): Observable<any>{
    let token = sessionStorage.getItem('Token');
    let headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return this.httpClient.get(`${this.apiUrl}Order/GetAllOrders`, {headers})
    .pipe(map(result => result))
  }

  GetOrder(order_ID: string): Observable<Order>
  {
    let token = sessionStorage.getItem('Token');
    let headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return this.httpClient.get<Order>(`${this.apiUrl}Order/GetOrder/${order_ID}`, {headers});
  }

  CollectOrder(order_ID: string):Observable<Order>
  {
    let token = sessionStorage.getItem('Token');
    let headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return this.httpClient.put<Order>(`${this.apiUrl}Order/CollectOrder/${order_ID}`, {} ,{headers});
  }

  CancelOrder(order_ID: string):Observable<Order>
  {
    let token = sessionStorage.getItem('Token');
    let headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
      return this.httpClient.put<Order>(`${this.apiUrl}Order/CancelOrder/${order_ID}`, {} ,{headers});
  }
  

  GetCustomerOrders(): Observable<any>{
    let token = sessionStorage.getItem('Token');
    let headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return this.httpClient.get<Order>(`${this.apiUrl}Order/GetAllCustomerOrders`, {headers})
    .pipe(map(result => result));
  }

  cartItems: any=[];
  itemsInCart: number = 0;
  calculateQuantity(): number {
    this.itemsInCart = 0;
    if (!sessionStorage.getItem('cartItem'))
    {
      this.itemsInCart = 0;
    }else{
      this.cartItems = JSON.parse(sessionStorage.getItem('cartItem')!)
      for (const item of this.cartItems) {
        this.itemsInCart += item.cartQuantity;
    }

      }

    return this.itemsInCart;
  }

  /*Reports Section */
  GetLoadsCarried(): Observable<any>{
    let token = sessionStorage.getItem('Token');
    let headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return this.httpClient.get(`${this.apiUrl}Report/GetLoadsCarried`, {headers})
    .pipe(map(result => result))
  }

}