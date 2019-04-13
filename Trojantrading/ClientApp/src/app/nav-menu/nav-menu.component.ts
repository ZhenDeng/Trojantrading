import { Component, OnInit } from '@angular/core';
import { NavbarService } from '../services/navbar.service';
import { Router, ActivatedRoute } from '@angular/router';
import { ShareService } from '../services/share.service';
import { NgbDropdownConfig } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css'],
  providers: [NgbDropdownConfig]
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
    public nav: NavbarService,
    private router: Router,
    private activatedRouter: ActivatedRoute,
    private shareService: ShareService,
    private config: NgbDropdownConfig
  ) { }

  ngOnInit() {
    this.nav.show();
    this.nav.showTab();
  }

  logOut(): void{
    this.shareService.savecookies("userToken", "", 1);
    this.router.navigate(['login'], { relativeTo: this.activatedRouter.parent })
  }

}
