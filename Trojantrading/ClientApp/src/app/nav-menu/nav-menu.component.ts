import { Component, OnInit } from '@angular/core';
import { NavbarService } from '../services/navbar.service';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent implements OnInit {

  testJsonObj = [
    {type: 'Hand-Made Cigars'},
    {type: 'Machine-Made Cigars'},
    {type: 'Little Cigars'},
    {type: 'Cigarettes'},
    {type: 'Pipe Tobacco'},
    {type: 'Roll Your Own'},
    {type: 'Filters'},
    {type: 'Papers'},
    {type: 'Lighters'},
    {type: 'Accessories'},
  ]
  constructor(
    public nav: NavbarService
  ) { }

  ngOnInit() {
    this.nav.show();
  }

}
