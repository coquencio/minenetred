import { Injectable} from '@angular/core';
import {HttpClient, HttpResponse} from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserSettingsService {

  constructor(private httpClient: HttpClient) { }

    updateRedmineKey(key: string) : Observable<any>{
    return this.httpClient.post(
      'https://localhost:44323/settings/key/'
      + key,{},
      { withCredentials: true });
  }

  updateBaseAddress(address: string) : Observable<any>{
    return this.httpClient.post(
      'https://localhost:44323/settings/baseAddress?address='
      + address,{},
      { withCredentials: true });
  }
}
