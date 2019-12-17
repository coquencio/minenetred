import { Injectable } from '@angular/core';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { Observable } from 'rxjs';
import { IProject } from './../../Interfaces/ProjectInterface';

@Injectable({
  providedIn: 'root'
})
export class ProjectsService {

  constructor(private httpClient: HttpClient)
  { }

  GetOpenProjects() : Observable<HttpResponse<IProject[]>>{
    return this.httpClient.get<HttpResponse<IProject[]>>(
      'https://localhost:44323/api/Projects',
      { withCredentials: true });
  }
}
