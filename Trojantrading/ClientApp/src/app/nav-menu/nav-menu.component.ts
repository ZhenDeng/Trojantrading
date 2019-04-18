import { ProductService } from './../services/product.service';
import { Component, OnInit } from '@angular/core';
import { NavbarService } from '../services/navbar.service';
import { Router, ActivatedRoute } from '@angular/router';
import { ShareService } from '../services/share.service';
import { NgbDropdownConfig } from '@ng-bootstrap/ng-bootstrap';
import { Product } from '../models/Product';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css'],
  providers: [NgbDropdownConfig]
})
export class NavMenuComponent implements OnInit {

  testJsonObj = [
    {type: 'Hand-Made Cigars', category: 'hand-made'},
    {type: 'Machine-Made Cigars', category: 'machine-made'},
    {type: 'Little Cigars', category: 'little-cigars'},
    {type: 'Cigarettes', category: 'cigarettes'},
    {type: 'Pipe Tobacco', category: 'pipe-tobacco'},
    {type: 'Roll Your Own', category: 'roll-your-won'},
    {type: 'Filters', category: 'filters'},
    {type: 'Papers', category: 'papers'},
    {type: 'Lighters', category: 'lighters'},
    {type: 'Accessories', category: 'accessories'},
  ];

  products: Product[];

  constructor(
    public nav: NavbarService,
    private router: Router,
    private activatedRouter: ActivatedRoute,
    private shareService: ShareService,
    private productService: ProductService,
    private config: NgbDropdownConfig
  ) { }

  ngOnInit() {
    this.nav.show();
    this.nav.showTab();
  }

  proceedToCheckout(): void{
    this.router.navigate(["/cart"]);
  }

  manageRedirect(category: string) {
    this.router.navigate(['home'], {
      relativeTo: this.activatedRouter,
      queryParams: {
        category: category
      }
    });
  }

  logOut(): void{
    this.shareService.savecookies("userToken", "", 1);
    this.router.navigate(['login'], { relativeTo: this.activatedRouter.parent })
  }

}
