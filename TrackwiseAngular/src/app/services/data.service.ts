import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { map, Observable, Subject } from 'rxjs';
import { Truck } from 'src/app/shared/truck';

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
}