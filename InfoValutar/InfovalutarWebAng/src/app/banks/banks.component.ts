import { Component, OnInit } from '@angular/core';
import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { Observable, from, merge, forkJoin } from 'rxjs';
import { map, shareReplay, tap, mergeMap, flatMap, concatMap, mergeScan } from 'rxjs/operators';
import { BanksService } from '../services/banks.service';
import { AutoUnsub } from 'src/utils/autoUnsub';
import { ExchangeRates } from 'src/classes/ExchangeRates';

@Component({
  selector: 'app-banks',
  templateUrl: './banks.component.html',
  styleUrls: ['./banks.component.css']
})
@AutoUnsub
export class BanksComponent implements OnInit {

  constructor(private breakpointObserver: BreakpointObserver, private banksService: BanksService) {}
  banks: string[];

  rates: Map<string, ExchangeRates[]> = new Map < string, ExchangeRates[]>();
  banksObs: Observable<string[]>;
  isHandset$: Observable<boolean> = this.breakpointObserver.observe(Breakpoints.Handset)
    .pipe(
      map(result => result.matches),
      shareReplay()
    );
  ngOnInit(): void {
    let self = this;
    this.banksObs = this.banksService.GetBanksIds();
    this.banksObs
    .pipe(
      tap(it => {
        console.log(it.length);
        self.banks = it;
        from(it)
          .pipe(
              mergeMap(i =>
                  self.banksService.GetRates(i)
                  .pipe(
                    tap(v => {
                      console.log('obtaining' + i + self.rates.size);
                      self.rates.set(i, v);
                      console.log('obtaining' + self.rates.size);

                    })
                  )

                  ),
          ).subscribe();

      })
     )
    .subscribe(it => it, err => window.alert('error:' + JSON.stringify(err))
    );

  }

}
