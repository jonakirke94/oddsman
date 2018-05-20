import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NgPrimeModule } from './ngprime/ngprime.module';
import {  ReactiveFormsModule, FormsModule } from '@angular/forms';
import { ChartsModule } from 'ng2-charts';
import { HttpClientModule } from '@angular/common/http';

/* Custom components that can be used in any module */
import { HistoryComponent } from '../history/history.component';
import { StandingComponent } from '../standing/standing.component';
import { LoadingSpinnerComponent } from '../loading-spinner/loading-spinner.component';
import { ResultsComponent } from '../adminpanel/admincomponents/results/results.component';
import { ResultFeedComponent } from '../result-feed/result-feed.component';
import { BetFeedComponent } from '../bet-feed/bet-feed.component';


@NgModule({
  imports: [
    CommonModule,
    NgPrimeModule,
    FormsModule,
    ReactiveFormsModule,
    ChartsModule,
    HttpClientModule

  ],
  exports: [ 
    NgPrimeModule,
    FormsModule,
    ReactiveFormsModule,
    ChartsModule,
    HttpClientModule,
    HistoryComponent,
    StandingComponent,
    LoadingSpinnerComponent,     
    BetFeedComponent,
    ResultFeedComponent,
    ResultsComponent
  ],
  declarations: [
    HistoryComponent,
    StandingComponent,
    LoadingSpinnerComponent,     
    BetFeedComponent,
    ResultFeedComponent,
    ResultsComponent
  ]
})
export class SharedModule { }
