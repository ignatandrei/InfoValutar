import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';
import { environment } from './../../environments/environment';
@Injectable({
  providedIn: 'root'
})
export class BanksService {

  constructor(private http:HttpClient) { }

  public GetBanksIds(): Observable<string[]> {


    const urlBanks=environment.url +'api/v1.0/TodayRates/Banks';
    return this.http.get<string[]>(urlBanks);

    
  }
}
