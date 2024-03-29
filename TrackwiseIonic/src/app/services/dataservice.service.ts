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

  deliveries: Delivery[] = []; // Array to store the deliveries

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

  forgotPassword(email: Forgotpass) {
    console.log(email);
    return this.httpClient.post<any>(`${this.apiUrl}Mail/ForgotPasswordEmail`,email,this.httpOptions );
  }

  GetAllDriverDeliveries() {
    this.GetDriverDeliveries().subscribe(result => {
      this.deliveries = result;
      console.log(result)
    });
  }

  CompleteDelivery(delivery_ID: string):Observable<any>
  {
    let token = sessionStorage.getItem('Token');
    let headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
      return this.httpClient.put<any>(`${this.apiUrl}Job/CompleteDelivery/${delivery_ID}`, {}, {headers});
  }

  AddDoc(docrequest: addDocument): Observable<addDocument> {
    console.log(docrequest)
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
