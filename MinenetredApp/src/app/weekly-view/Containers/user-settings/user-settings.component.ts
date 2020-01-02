import { Component, OnInit, Input } from '@angular/core';
import { Store, select } from '@ngrx/store';
import { UserSettingsService } from '../../../Services/UserSettings/user-settings.service';
import * as fromWeeklyView from './../../state/weekly-view.reducer';

@Component({
  selector: 'app-user-settings',
  templateUrl: './user-settings.component.html',
  styleUrls: ['./user-settings.component.scss']
})
export class UserSettingsComponent implements OnInit {

  constructor(
    private store : Store<fromWeeklyView.State>,
    private userService : UserSettingsService)
  { }

  infoMessage : string;
  errorMessage : string;
  baseAddress : string;
  apiKey : string;
  IsValidAddress : boolean;
  isLoadingBaseAddress : boolean;
  isLoadingApiKey : boolean;

  ngOnInit() {
    this.infoMessage = '';
    this.errorMessage = '';
    this.IsValidAddress = false;
    this.store.pipe(select(fromWeeklyView.getWarningMessage)).subscribe(
      w => {
        if(w){
          this.errorMessage = w;
        }
      }
    );
    this.GetBaseAddress();
  }
  private GetBaseAddress(){
    this.isLoadingBaseAddress = true;
    this.userService.getBaseAddress().subscribe(
      r => {
            this.baseAddress = r.address;
            this.IsValidAddress = true;
            this.GetApiKey();
          },
      () => {
        this.IsValidAddress = false;
        this.baseAddress = '';
        this.isLoadingBaseAddress = false;
      },
      ()=> this.isLoadingBaseAddress = false
    );
  }
  private GetApiKey(){
    this.isLoadingApiKey = true;
    this.userService.getApiKey().subscribe(
      r=>{this.apiKey = r.key},
      ()=>this.isLoadingApiKey=false,
      ()=>this.isLoadingApiKey = false
    );
  }
  AddBaseAddress(){
    if(this.baseAddress === ''){
      this.errorMessage = 'Add a valid address';
      return;
    }
    if(this.baseAddress.includes(' ')){
      this.errorMessage = 'Address must not contain blank spaces';
      return;
    }
    this.isLoadingBaseAddress=true;
    this.userService.updateBaseAddress(this.baseAddress).subscribe(r => {
      this.infoMessage = r;
      this.errorMessage = '';
      this.IsValidAddress = true;
      this.isLoadingBaseAddress=false;
    }, error => {
      console.log(error);
      this.errorMessage = error.error,
        this.baseAddress = '';
        this.infoMessage = '';
        this.isLoadingBaseAddress = false;
    });
  }

  AddApiKey(){
    if(this.apiKey === ''){
      this.errorMessage = 'Add a valid key';
      return;
    }
    if(this.apiKey.includes(' ')){
      this.errorMessage = 'Key must not contain blank spaces';
      return;
    }
    this.isLoadingApiKey = true;
    this.userService.updateRedmineKey(this.apiKey).subscribe(
      r=>{
        this.infoMessage = r;
        this.errorMessage = '';
        this.isLoadingApiKey = false;
      },
      error => {
        this.errorMessage = error.error;
        this.infoMessage = '';
        this.apiKey = '';
        this.isLoadingApiKey = false;
      }
    );
  }

}
