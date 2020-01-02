import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {UserSettingsComponent} from './Containers/user-settings/user-settings.component';
import {NavigationComponent} from './Containers/navigation/navigation.component';
import { WeeklyViewComponent } from './Containers/weekly-view/weekly-view.component';
import { WeeklyTableComponent } from './Components/weekly-table/weekly-table.component';
import { DatePickerComponent} from './Components/date-picker/date-picker.component';
import { FormsModule } from '@angular/forms';
import {RouterModule} from '@angular/router';
import { StoreModule } from '@ngrx/store';
import { reducer } from './state/weekly-view.reducer';
import { LoadingCircleComponent } from './Components/loading-circle/loading-circle.component';

@NgModule({
  declarations: [
      WeeklyTableComponent,
      WeeklyViewComponent,
      DatePickerComponent,
      NavigationComponent,
      UserSettingsComponent,
      LoadingCircleComponent
    ],
  imports: [
    CommonModule,
    FormsModule,
    RouterModule.forRoot([
      {path:'weeklyView', component:WeeklyViewComponent},
      {path:'settings',component: UserSettingsComponent },
      {path: '', redirectTo:'weeklyView', pathMatch:'full'},
      {path: '**', redirectTo:'weeklyView', pathMatch:'full'}
    ]),
    StoreModule.forFeature('weeklyView', reducer),
  ],
  exports:[
    NavigationComponent
  ]
})
export class WeeklyViewModule { }
