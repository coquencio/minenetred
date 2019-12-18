import { Component, OnInit, Input } from '@angular/core';
import { Store, select } from '@ngrx/store';
import { UserSettingsService } from '../../../Services/UserSettings/user-settings.service';
import { HttpResponse } from '@angular/common/http';

@Component({
  selector: 'app-user-settings',
  templateUrl: './user-settings.component.html',
  styleUrls: ['./user-settings.component.scss']
})
export class UserSettingsComponent implements OnInit {

  constructor(
    private store : Store<any>,
    private userService : UserSettingsService)
  { }

  infoMessage : string;
  errorMessage : string;
  baseAddress : string;
  apiKey : string;
  IsValidAddress : boolean;

  ngOnInit() {
    this.infoMessage = '';
    this.errorMessage = '';
    this.IsValidAddress = false;
    this.store.pipe(select('weeklyView')).subscribe(
      w => {
        if(w){
          this.errorMessage = w.WarningMessage;
        }
      }
    );
    this.GetBaseAddress();
  }
  private GetBaseAddress(){
    this.userService.getBaseAddress().subscribe(
      r => {
            this.baseAddress = r.address;
            this.IsValidAddress = true;
            this.GetApiKey();
          },
      () => {
        this.IsValidAddress = false;
        this.baseAddress = '';
      }
    );
  }
  private GetApiKey(){
    console.log("simon");
    this.userService.getApiKey().subscribe(
      r=>{this.apiKey = r.key},
    );
  }
  AddBaseAddress(){
    if(this.baseAddress == ''){
      this.errorMessage = 'Add a valid address';
      return;
    }
    if(this.baseAddress.includes(' ')){
      this.errorMessage = 'Address must not contain blank spaces';
      return;
    }
    this.userService.updateBaseAddress(this.baseAddress).subscribe(r => {
      this.infoMessage = r;
      this.errorMessage = '';
      this.IsValidAddress = true;
    }, error => {
      console.log(error);
      this.errorMessage = error.error,
        this.baseAddress = '';
    });
  }

  AddApiKey(){
    if(this.apiKey == ''){
      this.errorMessage = 'Add a valid key';
      return;
    }
    if(this.apiKey.includes(' ')){
      this.errorMessage = 'Address must not contain blank spaces';
      return;
    }
    this.userService.updateRedmineKey(this.apiKey).subscribe(
      r=>{
        this.infoMessage = r;
        this.errorMessage = '';
      },
      error => {this.errorMessage = error.error}
    );
  }

}
