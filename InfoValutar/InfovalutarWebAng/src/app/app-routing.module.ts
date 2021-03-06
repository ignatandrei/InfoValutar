import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AppComponent } from './app.component';
import { BankComponent } from './bank/bank.component';
import { BanksComponent } from './banks/banks.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { ProgrammerComponent } from './programmer/programmer.component';


const routes: Routes = [

  { path: '', redirectTo: '/dashboard', pathMatch: 'full' },
  { path: 'dashboard', component: DashboardComponent },
  {path: 'bank/:id', component: BankComponent},
  {path: 'programmers', component: ProgrammerComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
