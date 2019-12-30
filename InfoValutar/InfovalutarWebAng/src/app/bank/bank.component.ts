import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Location } from '@angular/common'
@Component({
  selector: 'app-bank',
  templateUrl: './bank.component.html',
  styleUrls: ['./bank.component.css']
})
export class BankComponent implements OnInit {

  idBank: string;
  constructor(private route: ActivatedRoute, private location: Location) { }

  ngOnInit() {
    //this.idBank = this.route.snapshot.paramMap.get('id');
    this.route.params.subscribe(rp => {
      this.idBank = rp.id;
    }
    );
  }

}
