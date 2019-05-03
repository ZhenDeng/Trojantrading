import { Component, OnInit } from '@angular/core';
import { NavbarService } from '../services/navbar.service';

@Component({
  selector: 'app-contact-us',
  templateUrl: './contact-us.component.html',
  styleUrls: ['./contact-us.component.css']
})
export class ContactUsComponent implements OnInit {

  title: string = 'Contact Us';

  constructor(
    private nav: NavbarService
  ) { }

  ngOnInit() {
    
    this.nav.show();
  }

}
