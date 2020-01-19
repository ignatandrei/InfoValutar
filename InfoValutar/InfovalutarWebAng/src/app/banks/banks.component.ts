import { Component, OnInit } from '@angular/core';
import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { Observable } from 'rxjs';
import { map, shareReplay } from 'rxjs/operators';
import { BanksService } from '../services/banks.service';
import { AutoUnsub } from 'src/utils/autoUnsub';

@Component({
  selector: 'app-banks',
  templateUrl: './banks.component.html',
  styleUrls: ['./banks.component.css']
})
@AutoUnsub
export class BanksComponent implements OnInit {

  constructor(private breakpointObserver: BreakpointObserver, private banksService: BanksService) {}
  banks: string[];
  banksObs: Observable<string[]>;
  isHandset$: Observable<boolean> = this.breakpointObserver.observe(Breakpoints.Handset)
    .pipe(
      map(result => result.matches),
      shareReplay()
    );
  ngOnInit(): void {
    this.banksObs = this.banksService.GetBanksIds();
    this.banksObs.subscribe(
      it => {
        console.log(it.length);
        this.banks = it;
      },
      err => window.alert('error:' + JSON.stringify(err))
    );
  }

}
