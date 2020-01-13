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

  GetOpenProjects() : Observable<IProject[]>{
    return this.httpClient.get<IProject[]>(
      'https://localhost:44323/api/Projects',
      { withCredentials: true });
  }
}
