import { Product } from './../models/Product';
import { ProductService } from './../services/product.service';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { Menu } from '../models/menu';
import { Router } from '@angular/router';
import { NavbarService } from '../services/navbar.service';
import { ShareService } from '../services/share.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']

})
export class HomeComponent implements OnInit, OnDestroy {


  allProducts: Product[] = [];


  displayedColumns: string[] = ['name', 'category', 'originalPrice', 'qty', 'button'];

  navLinks: Menu[] = [
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

  isHomeComponentDestroyed: boolean = false;

  constructor(
    private router: Router,
    private shareService: ShareService,
    public nav: NavbarService,
    private productService: ProductService
  ) { }

  ngOnInit() {

    let currentURL = this.router.url;
    if (currentURL != '/home') {
      this.isHomeComponentDestroyed = true;
    }

    this.getAllProducts();
  }

  getAllProducts() {
    console.info('products');
    this.productService.getAllProducts().subscribe((value: Product[]) => {
      this.allProducts = value;
      this.allProducts.forEach(product => product.quantity = 1);
    });
  }

  _keyPress(event: any) {
    const pattern = /[0-9\+\-\ ]/;
    let inputChar = String.fromCharCode(event.charCode);

    if (!pattern.test(inputChar)) {
      // invalid character, prevent input
      event.preventDefault();
    }
  }


  ngOnDestroy() {

    console.log("destroy home page");
  }

}
