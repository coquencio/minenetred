import { Injectable} from '@angular/core';
import {HttpClient, HttpResponse} from '@angular/common/http';
import { Observable } from 'rxjs';
import {IAddress} from './../../Interfaces/BaseAddressInterface';
import {IApiKey} from './../../Interfaces/ApiKeyInterface';
@Injectable({
  providedIn: 'root'
})
export class UserSettingsService {

  constructor(private httpClient: HttpClient) { }

    updateRedmineKey(key: string) : Observable<any>{
    return this.httpClient.post(
      'https://localhost:44323/settings/key/'
      + key,{},
      { withCredentials: true, responseType : 'text' });
  }

  updateBaseAddress(address: string) : Observable<any>{
    return this.httpClient.post(
      'https://localhost:44323/settings/baseAddress?address='
      + address,{},
      { withCredentials: true, responseType:'text' });
  }

  getBaseAddress() : Observable<IAddress>{
    return this.httpClient.get<IAddress>('https://localhost:44323/settings/baseAddress',
    {withCredentials: true});
  }
  getApiKey() : Observable<IApiKey>{
    return this.httpClient.get<IApiKey>('https://localhost:44323/settings/key',
    {withCredentials: true});
  }
}
