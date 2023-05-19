import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { map, Observable, Subject } from 'rxjs';
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