import { Component, OnInit, Input } from '@angular/core';
import { Store, select } from '@ngrx/store';

@Component({
  selector: 'app-user-settings',
  templateUrl: './user-settings.component.html',
  styleUrls: ['./user-settings.component.scss']
})
export class UserSettingsComponent implements OnInit {

  constructor(private store : Store<any>)
  { }

  message : string;
  ngOnInit() {
    this.store.pipe(select('weeklyView')).subscribe(
      w => {
        if(w){
          this.message = w.WarningMessage;
        }
      }
    );
  }

}
