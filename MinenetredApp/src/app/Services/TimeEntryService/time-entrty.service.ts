import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ITimeEntry } from '../../Interfaces/TimeEntryInterface';

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

  AddTimeEntry(entry : ITimeEntry) : Observable<any>{
    return this.httpClient.post(
      'https://localhost:44323/Entries',
      entry,
      {withCredentials: true});
  }

}
