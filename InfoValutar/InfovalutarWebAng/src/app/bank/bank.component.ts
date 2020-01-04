import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';
import { switchMap, tap } from 'rxjs/operators';
import { BanksService } from '../services/banks.service';
import { ExchangeRates } from 'src/classes/ExchangeRates';
@Component({
  selector: 'app-bank',
  templateUrl: './bank.component.html',
  styleUrls: ['./bank.component.css']
})
export class BankComponent implements OnInit {

  idBank = '';
  rates: ExchangeRates[] = [];
  constructor(private route: ActivatedRoute, private location: Location, private bs: BanksService) { }

  ngOnInit() {
    // this.idBank = this.route.snapshot.paramMap.get('id');
    // this.route.params.subscribe(rp => {
    //   this.idBank = rp.id;
    // }
    // );
    this.route.params.pipe(
      tap(rp => {
        this.idBank = rp.id;
      })

      , switchMap((it) => this.bs.GetRates(it.id))
      , tap(v => this.rates = v)

    ).subscribe();
  }

}
