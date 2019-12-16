import { Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class UserSettingsService {

  constructor(private httpClient: HttpClient) { }

  updateRedmineKey(key: string){
    return this.httpClient.post(
      'https://localhost:44323/settings/key?Redminekey='
      + key,
      { withCredentials: true });
  }

  updateBaseAddress(address: string){
    return this.httpClient.post(
      'https://localhost:44323/settings/baseAddress?address='
      + address,
      { withCredentials: true });
  }
}