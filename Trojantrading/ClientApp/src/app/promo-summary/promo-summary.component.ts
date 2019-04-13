import { Component, OnInit } from '@angular/core';
import { NavbarService } from '../services/navbar.service';

@Component({
  selector: 'app-promo-summary',
  templateUrl: './promo-summary.component.html',
  styleUrls: ['./promo-summary.component.css']
})
export class PromoSummaryComponent implements OnInit {

  constructor(
    private nav: NavbarService
  ) { }

  ngOnInit() {
    this.nav.hideTab();
  }

}
