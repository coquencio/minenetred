import { Component, OnInit, Input, ɵclearResolutionOfComponentResourcesQueue, ɵConsole } from '@angular/core';
import { IProject } from './../../../Interfaces/ProjectInterface';
import { TimeEntrtyService } from '../../../Services/TimeEntryService/time-entrty.service';
import { IIssue } from './../../../Interfaces/IssueInterface';
import { IActivity } from './../../../Interfaces/ActivityInterface';
import { IssuesService } from '../../../Services/IssuesService/issues.service';
import { ActivityService } from '../../../Services/ActivityService/activity.service';
import * as fromWeeklyView from '../../state/weekly-view.reducer';
import * as weeklyViewActions from '../../state/weeklyView.actions';
import { Store, select } from '@ngrx/store';
import { ITimeEntry } from '../../../Interfaces/TimeEntryInterface';

@Component({
  selector: 'app-weekly-table',
  templateUrl: './weekly-table.component.html',
  styleUrls: ['./weekly-table.component.scss']
})
export class WeeklyTableComponent implements OnInit {

  constructor(
    private timeEntryService: TimeEntrtyService,
    private issueService: IssuesService,
    private activityService: ActivityService,
    private store: Store<fromWeeklyView.State>
  ) { }

  @Input() projectList: IProject[];
  @Input() tableHeaders: Array<string>;

  hoursCounter: Array<number>;
  IsModalDisplaying: boolean;
  issueSelector: IIssue[];
  activitySelector: IActivity[];
  selectedProjectName: string;
  issuesLoaded: boolean;
  activitiesLoaded: boolean;
  timeEntryToPost: ITimeEntry;
  date: string;
  areHoursLoading: boolean;

  ngOnInit() {
    this.timeEntryToPost = {
      activityId: 0,
      comments: '',
      hours: 0,
      issueId: 0,
      spentOn: '',
    }
    this.areHoursLoading = false;
    this.issuesLoaded = false;
    this.activitiesLoaded = false;
    this.IsModalDisplaying = false;
    this.store.pipe(select(fromWeeklyView.getProjectName)).subscribe(
      n => this.selectedProjectName = n
    );
    this.store.pipe(select(fromWeeklyView.getSelectedDate)).subscribe(
      d => this.date = d
    );
  }
  ngOnChanges() {
    if (this.tableHeaders) {
      this.AddHoursToProjects();
    }
  }
  HideModal() {
    // Clear all
    this.issuesLoaded = false;
    this.activitiesLoaded = false;
    this.IsModalDisplaying = false;
    this.issueSelector = new Array<IIssue>();
    this.activitySelector = Array<IActivity>();
    this.store.dispatch(new weeklyViewActions.SetSelectedProjectName(''));
    this.store.dispatch(new weeklyViewActions.SetFormatedDate(''));
    this.timeEntryToPost = {
      activityId: 0,
      comments: '',
      hours: 0,
      issueId: 0,
      spentOn: '',
    }
  }
  DisplayModal(project: IProject, index: number) {
    console.log(this.tableHeaders[index]);
    this.IsModalDisplaying = true;
    this.GetIssues(project.id);
    this.GetActivities(project.id);
    this.store.dispatch(new weeklyViewActions.SetSelectedProjectName(project.name));
    this.store.dispatch(new weeklyViewActions.SetFormatedDate(this.tableHeaders[index]));
    this.timeEntryToPost.spentOn = this.date;
  }
  AddTimeEntry() {
    if (this.timeEntryToPost.comments.trim() === '') {
      window.alert('Please add a description');
      return;
    }
    if (this.timeEntryToPost.issueId === 0) {
      window.alert('Select a valid issue');
      return;
    }
    if (this.timeEntryToPost.activityId === 0) {
      window.alert('Select a valid activity');
      return;
    }
    this.timeEntryService.AddTimeEntry(this.timeEntryToPost).subscribe(
      () => {
        this.HideModal();
        this.AddHoursToProjects();
      },
      error => window.alert(error.error)
    );
  }
  GetIssues(projectId: number) {
    this.issueService.GetIssues(projectId).subscribe(
      r => this.issueSelector = r,
      null,
      () => { this.issuesLoaded = true; }
    );
  }
  GetActivities(projectId: number) {
    this.activityService.GetActivitiesPerProject(projectId).subscribe(
      r => this.activitySelector = r,
      null,
      () => { this.activitiesLoaded = true; }
    );
  }
  private AddHoursToProjects() {
    this.areHoursLoading = true;
    this.projectList.forEach((project, projectIndex) => {
      project.hoursPerday = new Array<number>();
      this.tableHeaders.forEach((element, index) => {
        const startingIndex = element.length - 10;
        const formatedDate = element.substring(startingIndex);
        this.timeEntryService.GetHoursPerProjectAndDay(formatedDate, project.id).subscribe(
          h => {
            project.hoursPerday[index] = h;
          },
          null,
          () => {
            if (projectIndex === this.projectList.length - 1 && project.hoursPerday.length === this.tableHeaders.length) {
              this.GetHoursPerDay();
              this.areHoursLoading = false;
            }
          }
        );
      });
    });
  }
  private GetHoursPerDay() {
    this.hoursCounter = new Array<number>(0, 0, 0, 0, 0);
    this.projectList.forEach((project) => {
      project.hoursPerday.forEach((element, index) => {
        this.hoursCounter[index] = this.hoursCounter[index] + element;
      });
    });
  }

}
