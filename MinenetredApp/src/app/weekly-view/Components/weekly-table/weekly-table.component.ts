import { Component, OnInit, Input, ɵclearResolutionOfComponentResourcesQueue, ɵConsole } from '@angular/core';
import { IProject } from './../../../Interfaces/ProjectInterface';
import {TimeEntrtyService} from '../../../Services/TimeEntryService/time-entrty.service';

@Component({
  selector: 'app-weekly-table',
  templateUrl: './weekly-table.component.html',
  styleUrls: ['./weekly-table.component.scss']
})
export class WeeklyTableComponent implements OnInit {

  constructor(private timeEntryService : TimeEntrtyService) { }

  @Input() projectList: IProject[];
  @Input() tableHeaders: Array<string>;

  hoursCounter : Array<number>;
  IsModalDisplaying : boolean;
  ngOnInit(){
    this.IsModalDisplaying = false;
  }
  ngOnChanges(){
    if(this.tableHeaders){
      this.AddHoursToProjects();
    }
  }

  DisplayModal(){
    this.IsModalDisplaying = !this.IsModalDisplaying;
  }
  private AddHoursToProjects(){
    this.projectList.forEach((project, projectIndex) => {
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
            if(projectIndex === this.projectList.length-1 && project.hoursPerday.length === this.tableHeaders.length){
              this.GetHoursPerDay();
            }
          }
        );
      });
    });
  }
  private GetHoursPerDay(){
    this.hoursCounter = new Array<number>(0,0,0,0,0);
    this.projectList.forEach((project) =>{
      project.hoursPerday.forEach((element, index) =>{
        this.hoursCounter[index] = this.hoursCounter[index] + element;
      });
    });
  }

}
