import { Component, OnInit } from '@angular/core';
import { NavbarService } from '../services/navbar.service';

@Component({
  selector: 'app-terms',
  templateUrl: './terms.component.html',
  styleUrls: ['./terms.component.css']
})
export class TermsComponent implements OnInit {

  title:string = "Terms and Conditions";

  constructor(private nav: NavbarService) { }

  ngOnInit() {
    this.nav.hideTab();
    this.nav.show();
  }

}
