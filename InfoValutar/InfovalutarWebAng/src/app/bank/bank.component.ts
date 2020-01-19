import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common';
import { switchMap, tap } from 'rxjs/operators';
import { BanksService } from '../services/banks.service';
import { ExchangeRates } from 'src/classes/ExchangeRates';
import { Observable } from 'rxjs';
import { AutoUnsub } from 'src/utils/autoUnsub';
@Component({
  selector: 'app-bank',
  templateUrl: './bank.component.html',
  styleUrls: ['./bank.component.css']
})
@AutoUnsub
export class BankComponent implements OnInit {

  idBank = '';
  rates: ExchangeRates[] = [];
  constructor(private route: ActivatedRoute, private location: Location, private bs: BanksService) { }
  obsExchange: Observable<ExchangeRates[]>;
  ngOnInit() {
    // this.idBank = this.route.snapshot.paramMap.get('id');
    // this.route.params.subscribe(rp => {
    //   this.idBank = rp.id;
    // }
    // );

    this.obsExchange = this.route.params.pipe(
      tap(rp => {
        this.idBank = rp.id;
      })

      , switchMap((it) => this.bs.GetRates(it.id))
      , tap(v => this.rates = v)

    );

    this.obsExchange.subscribe();
  }

}
