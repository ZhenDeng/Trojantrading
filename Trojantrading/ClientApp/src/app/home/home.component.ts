import { Component, OnInit, OnDestroy } from '@angular/core';
import { Menu } from '../models/menu';
import { Router } from '@angular/router';
import { NavbarService } from '../services/navbar.service';
import { Product } from '../models/Product';
import { ShareService } from '../services/share.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']

})
export class HomeComponent implements OnInit, OnDestroy {

  displayedColumns: string[] = ['name', 'category', 'originalPrice', 'button'];

  navLinks:Menu[] = [
    {
      path: '/home',
      label: 'All Products',
      id: 'allProducts'
    },
    {
      path: '/product/new',
      label: 'New Product',
      id: 'newProduct'
    },
    {
      path: '/product/promotion',
      label: 'Promotions',
      id: 'promotions'
    },
    {
      path: '/product/soldout',
      label: 'Sold Out',
      id: 'soldout'
    },
  ];

  dataSource: Product[];

  isHomeComponentDestroyed:boolean = false;

  constructor(
    private router: Router,
    private shareService: ShareService,
    public nav: NavbarService
  ) {}

  ngOnInit() {
    this.dataSource = this.shareService.product;
    this.nav.showTab();
    let currentURL = this.router.url;
    if(currentURL != '/home'){
      this.isHomeComponentDestroyed = true;
    }
    
  }

  ngOnDestroy(){
    
    console.log("destroy home page");
  }

}
