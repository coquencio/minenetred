import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class TimeEntrtyService {

  constructor(private httpClient: HttpClient) { }

  GetHoursPerProjectAndDay(date : string, projectId : number) : Observable<number>{
    return this.httpClient.get<number>(
      'https://localhost:44323/Entries/Hours/' + projectId + '?date=' + date,
      { withCredentials: true });
  }
}
