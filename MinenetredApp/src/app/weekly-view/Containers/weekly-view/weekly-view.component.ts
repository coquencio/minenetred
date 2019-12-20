import { Component, OnInit, Output } from '@angular/core';
import { IProject } from './../../../Interfaces/ProjectInterface';
import {  ProjectsService } from './../../../Services/ProjectsService/projects.service';
import { HttpResponse } from '@angular/common/http';
import { Router } from '@angular/router';
import { Store, select } from '@ngrx/store';
import * as fromWeeklyView from '../../state/weekly-view.reducer';

@Component({
  selector: 'app-weekly-view',
  templateUrl: './weekly-view.component.html',
  styleUrls: ['./weekly-view.component.scss']
})
export class WeeklyViewComponent implements OnInit {

  constructor(
    private projectService: ProjectsService,
    private router : Router,
    private store : Store<fromWeeklyView.State>
    ) { }
  response : IProject[];
  formatedDates : Array<string>;
  dates : Array<Date>;


  ngOnInit() {
    this.projectService.GetOpenProjects().subscribe(
      r => {
        this.response = r;
        this.store.dispatch({
          type : 'SET_WARNING_MESSAGE',
          payload: ''
        });
      },
      r => {
        this.store.dispatch({
          type : 'SET_WARNING_MESSAGE',
          payload: r.error
        });
        this.router.navigate(['/settings']);
      }
    );
  }
  private GetDayOfTheWeek(date : Date) : string{
    const days = ['Sunday','Monday','Tuesday','Wednesday','Thursday','Friday','Saturday'];
    return days[date.getDay()];
  }
  private GetStringMonth(date : Date) : string{
    const months = ['01','02','03','04','05','06','07','08','09','10','11','12'];
    return months[date.getMonth()];
  }
  BuildDatesArray(date:string){
    this.dates = new Array<Date>();
    const FormatedDate = new Date(
      Number.parseFloat(date.substring(0,4)),
      Number.parseFloat(date.substring(5,7))-1,
      Number.parseFloat(date.substring(8))
    );
    for (let index = 0; index < 5; index++) {
      const dateToAdd = new Date(FormatedDate);
      dateToAdd.setDate(dateToAdd.getDate()+index);
      this.dates.push(dateToAdd);
    }
    this.BuildDatesFormated();
  }

  private BuildDatesFormated(){
    this.formatedDates = new Array<string>();
    this.dates.forEach(element => {
      let toAdd = '';
      toAdd += this.GetDayOfTheWeek(element) + ' ';
      toAdd += element.getFullYear() + '-';
      toAdd += this.GetStringMonth(element) + '-';
      toAdd += element.getDate()< 10 ? '0' + element.getDate() : element.getDate();
      this.formatedDates.push(toAdd);
    });
  }

}
