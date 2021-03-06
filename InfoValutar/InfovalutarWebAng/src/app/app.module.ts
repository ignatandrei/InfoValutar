import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { BanksComponent } from './banks/banks.component';
import { LayoutModule } from '@angular/cdk/layout';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { BankComponent } from './bank/bank.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { ProgrammerComponent } from './programmer/programmer.component';
import { CachingInterceptor } from 'src/utils/CachingInterceptor';
import {HTTP_INTERCEPTORS} from '@angular/common/http';
import {MatSelectModule} from '@angular/material/select';
@NgModule({
  declarations: [
    AppComponent,
    BanksComponent,
    BankComponent,
    DashboardComponent,
    ProgrammerComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    FormsModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    LayoutModule,
    MatToolbarModule,
    MatButtonModule,
    MatSidenavModule,
    MatIconModule,
    MatListModule,
    MatSelectModule
  ],
  providers: [ {
    provide: HTTP_INTERCEPTORS,
    useClass: CachingInterceptor,
    multi: true
}, ],
  bootstrap: [AppComponent]
})
export class AppModule { }
