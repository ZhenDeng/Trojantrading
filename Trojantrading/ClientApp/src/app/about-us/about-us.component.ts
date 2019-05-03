import { Component, OnInit } from '@angular/core';
import { NavbarService } from '../services/navbar.service';

@Component({
  selector: 'app-about-us',
  templateUrl: './about-us.component.html',
  styleUrls: ['./about-us.component.css']
})
export class AboutUsComponent implements OnInit {

  title: string = 'About Us';

  constructor(
    private nav: NavbarService
  ) { }

  ngOnInit() {
    
    this.nav.show();
  }

}
