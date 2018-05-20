import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../shared/shared.module';
import { AppRoutingModule } from '../app-routing.module';


import { MyBetsComponent } from './usercomponents/my-bets/my-bets.component';

import { DashboardComponent } from './dashboard.component';
import { UsersettingsComponent } from './usercomponents/usersettings/usersettings.component';
import { UserstatsComponent } from './usercomponents/userstats/userstats.component';
import { UserTournamentsComponent } from './usercomponents/user-tournaments/user-tournaments.component';
import { SendBetsComponent } from './usercomponents/send-bets/send-bets.component';
import { UserHistoryComponentComponent } from './usercomponents/user-history-component/user-history-component.component';

import { HistoryComponent } from '../history/history.component';







/* FEATURE MODULE ----> CORE.MODULE.TS */
@NgModule({
   
    imports: [
      CommonModule,
      SharedModule,
      AppRoutingModule,

    ],
    declarations: [
        DashboardComponent,
        MyBetsComponent,
        UsersettingsComponent,
        UserstatsComponent,
        UserTournamentsComponent,
        SendBetsComponent,
        MyBetsComponent,
        UserHistoryComponentComponent
        
    ],
    exports: [
        DashboardComponent,
        MyBetsComponent,
        UsersettingsComponent,
        UserstatsComponent,
        UserTournamentsComponent,
        SendBetsComponent,
        MyBetsComponent,
        UserHistoryComponentComponent
    ]

  })
  export class DashboardModule { }
  