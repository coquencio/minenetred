import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {UserSettingsComponent} from './Containers/user-settings/user-settings.component';
import {NavigationComponent} from './Containers/navigation/navigation.component';
import { WeeklyViewComponent } from './Containers/weekly-view/weekly-view.component';
import { WeeklyTableComponent } from './Components/weekly-table/weekly-table.component';
import { DatePickerComponent} from './Components/date-picker/date-picker.component';
import { FormsModule } from '@angular/forms';
import {RouterModule} from '@angular/router';



@NgModule({
  declarations: [
      WeeklyTableComponent,
      WeeklyViewComponent,
      DatePickerComponent,
      NavigationComponent,
      UserSettingsComponent
    ],
  imports: [
    CommonModule,
    FormsModule,
    RouterModule.forRoot([
      {path:'weeklyView', component:WeeklyViewComponent},
      {path:'settings',component: UserSettingsComponent },
      {path: '', redirectTo:'SpentTime', pathMatch:'full'}
    ])
  ],
  exports:[
    NavigationComponent
  ]
})
export class WeeklyViewModule { }
