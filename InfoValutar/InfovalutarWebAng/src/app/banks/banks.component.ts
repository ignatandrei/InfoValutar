import { Component, OnInit } from '@angular/core';
import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { Observable } from 'rxjs';
import { map, shareReplay } from 'rxjs/operators';
import { BanksService } from '../services/banks.service';

@Component({
  selector: 'app-banks',
  templateUrl: './banks.component.html',
  styleUrls: ['./banks.component.css']
})
export class BanksComponent implements OnInit {
  banks:string[];
  ngOnInit(): void {

    this.banksService.GetBanksIds().subscribe(
      it=>{
        console.log(it.length);
        this.banks=it;
      },
      err=> window.alert("error:"+ JSON.stringify(err))
    )
  }

  isHandset$: Observable<boolean> = this.breakpointObserver.observe(Breakpoints.Handset)
    .pipe(
      map(result => result.matches),
      shareReplay()
    );

  constructor(private breakpointObserver: BreakpointObserver, private banksService: BanksService) {}

}
