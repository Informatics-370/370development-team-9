import { Injectable } from '@angular/core';
import { HttpClient, HttpClientModule, HttpHeaders } from '@angular/common/http';
import { Observable, map } from 'rxjs';
import { LoginUser } from '../shared/LoginUser';
import { User } from '../shared/User';
import { Delivery } from '../shared/Delivery';
import { Forgotpass } from '../shared/Forgotpass';
import { addDocument } from '../shared/Document';


@Injectable({
  providedIn: 'root'
})
export class DataserviceService {
  
  apiUrl = 'https://localhost:7049/api/';

  httpOptions ={
    headers: new HttpHeaders({
      ContentType: 'application/json'
    })
  }
  constructor(private httpClient: HttpClient) { }
  
  
  LoginUser(loginUser: LoginUser){
    return this.httpClient.post<User>(`${this.apiUrl}User/Login`, loginUser, this.httpOptions)
  }

  GetDriverDeliveries(): Observable<any>{
    let token = sessionStorage.getItem('Token');
    let headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return this.httpClient.get<Delivery>(`${this.apiUrl}Job/GetDriverDeliveries`, {headers})
    .pipe(map(result => result));
  }

  updateDeliveryStatus(deliveryId: string): Observable<void> {
    const url = `${this.apiUrl}Job/${deliveryId}/status`;
    return this.httpClient.put<void>(url, {});
  }

  updateJobStatus(jobId: string): Observable<void> {
    const url = `${this.apiUrl}Job/${jobId}/jobstatus`;
    return this.httpClient.put<void>(url, {});
  }

  updateDriverStatus(DriverId: string): Observable<void> {
    const url = `${this.apiUrl}Job/${DriverId}/driverstatus`;
    return this.httpClient.put<void>(url, {});
  }

  updateTrailerStatus(TrailerId: string): Observable<void> {
    const url = `${this.apiUrl}Job/${TrailerId}/trailerstatus`;
    return this.httpClient.put<void>(url, {});
  }

  updateTruckStatus(TruckId: string): Observable<void> {
    const url = `${this.apiUrl}Job/${TruckId}/truckstatus`;
    return this.httpClient.put<void>(url, {});
  }

  forgotPassword(email: Forgotpass) {
    console.log(email);
    return this.httpClient.post<any>(`${this.apiUrl}Mail/ForgotPasswordEmail`,email,this.httpOptions );
  }

  updateDeliveryStatus(deliveryId: string): Observable<void> {
    const url = `${this.apiUrl}Job/${deliveryId}/status`;
    return this.httpClient.put<void>(url, {});
  }

  updateJobStatus(jobId: string): Observable<void> {
    const url = `${this.apiUrl}Job/${jobId}/jobstatus`;
    return this.httpClient.put<void>(url, {});
  }

  updateDriverStatus(DriverId: string): Observable<void> {
    const url = `${this.apiUrl}Job/${DriverId}/driverstatus`;
    return this.httpClient.put<void>(url, {});
  }

  updateTrailerStatus(TrailerId: string): Observable<void> {
    const url = `${this.apiUrl}Job/${TrailerId}/trailerstatus`;
    return this.httpClient.put<void>(url, {});
  }

  updateTruckStatus(TruckId: string): Observable<void> {
    const url = `${this.apiUrl}Job/${TruckId}/truckstatus`;
    return this.httpClient.put<void>(url, {});
  }


  AddDoc(docrequest: addDocument): Observable<addDocument> {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
      }),
    };

    return this.httpClient
      .post<addDocument>(
        `${this.apiUrl}Job/AddDocuments/`,
        docrequest,
        httpOptions
      )
      .pipe(map((result) => result));
  }

}
