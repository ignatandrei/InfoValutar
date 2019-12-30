import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AppComponent } from './app.component';
import { BankComponent } from './bank/bank.component';
import { BanksComponent } from './banks/banks.component';


const routes: Routes = [
  {
   path:'',component:BanksComponent , pathMatch:"full"
  },
  {
    path:'bank/:id',component:BankComponent
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
