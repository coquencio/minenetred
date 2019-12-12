import { Component, OnInit, Output, EventEmitter} from '@angular/core';

@Component({
  selector: 'app-date-picker',
  templateUrl: './date-picker.component.html',
  styleUrls: ['./date-picker.component.scss']
})
export class DatePickerComponent implements OnInit {
  date : string;
  @Output()
  outString : EventEmitter<string> = new EventEmitter<string>();
  constructor() { }
  ngOnInit() {
  }
  changedDate(){
    this.outString.emit(this.date);
  }
}
