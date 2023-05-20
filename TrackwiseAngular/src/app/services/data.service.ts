import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { map, Observable, Subject } from 'rxjs';
import { Driver } from '../shared/driver';
import { Truck } from 'src/app/shared/truck';
import { Trailer } from '../shared/trailer';

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


}