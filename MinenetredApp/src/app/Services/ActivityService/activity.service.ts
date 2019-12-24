import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import {IActivity} from '../../Interfaces/ActivityInterface';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ActivityService {

  constructor(private httpClient : HttpClient) { }

  GetActivitiesPerProject(projectId : number) : Observable<IActivity[]> {
    return this.httpClient.get<IActivity[]>(
      'https://localhost:44323/Activities/' + projectId,
      {withCredentials : true}
    );
  }
}
