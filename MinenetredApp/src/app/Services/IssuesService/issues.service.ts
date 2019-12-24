import { Injectable } from '@angular/core';
import {IIssue} from '../../Interfaces/IssueInterface';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class IssuesService {

  constructor(private httpClient : HttpClient) { }

  GetIssues(projectId :number) : Observable<IIssue[]>{
    return this.httpClient.get<IIssue[]>(
      'https://localhost:44323/Issues/'+ projectId,
      {withCredentials : true}
    );
  }
}
