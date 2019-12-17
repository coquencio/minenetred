import { Component, OnInit, Input, ɵclearResolutionOfComponentResourcesQueue, ɵConsole } from '@angular/core';
import { IProject } from './../../../Interfaces/ProjectInterface';
import {TimeEntrtyService} from '../../../Services/TimeEntryService/time-entrty.service';
import { formatDate, formatPercent } from '@angular/common';
import { HttpResponse } from '@angular/common/http';

@Component({
  selector: 'app-weekly-table',
  templateUrl: './weekly-table.component.html',
  styleUrls: ['./weekly-table.component.scss']
})
export class WeeklyTableComponent implements OnInit {

  constructor(private timeEntryService : TimeEntrtyService) { }

  @Input() projectList: HttpResponse<IProject[]>;
  @Input() tableHeaders: Array<string>;

  hoursCounter : Array<number>;
  ngOnInit(){
    
  }
  ngOnChanges(){
    if(this.tableHeaders){
      this.AddHoursToProjects();
    }
  }
  private AddHoursToProjects(){
    console.log(this.projectList.status);
    this.projectList.body.forEach((project, projectIndex) => {
      project.hoursPerday = new Array<number>();
      this.tableHeaders.forEach((element, index) => {
        const startingIndex = element.length - 10;
        const formatedDate = element.substring(startingIndex);
        this.timeEntryService.GetHoursPerProjectAndDay(formatedDate, project.id).subscribe(
          h => {
            project.hoursPerday[index]=h;
          },
          null,
          ()=>{
            if(projectIndex === this.projectList.body.length-1 && project.hoursPerday.length === this.tableHeaders.length){
              this.GetHoursPerDay();
            }
          }
        );
      });
    });
  }
  private GetHoursPerDay(){
    this.hoursCounter = new Array<number>(0,0,0,0,0);
    this.projectList.body.forEach((project) =>{
      project.hoursPerday.forEach((element, index) =>{
        this.hoursCounter[index] = this.hoursCounter[index] + element;
      });
    });
  }

}
