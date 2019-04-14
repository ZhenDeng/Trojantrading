import { Product } from './../models/Product';
import { ProductService } from './../services/product.service';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { Menu } from '../models/menu';
import { Router } from '@angular/router';
import { NavbarService } from '../services/navbar.service';
import { ShareService } from '../services/share.service';
import { MatTableDataSource } from '@angular/material';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']

})
export class HomeComponent implements OnInit, OnDestroy {

  allProducts: Product[] = [];

  dataSource = new MatTableDataSource();

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


  isHomeComponentDestroyed:boolean = false;

  constructor(
    private router: Router,
    private shareService: ShareService,
    public nav: NavbarService,
    private productService: ProductService
  ) {}

  ngOnInit() {

    let currentURL = this.router.url;
    if(currentURL != '/home'){
      this.isHomeComponentDestroyed = true;
    }
    
    this.getAllProducts();
  }

  getAllProducts() {
    this.productService.getAllProducts().subscribe((value: Product[]) =>{
        this.allProducts = value;
        this.dataSource = new MatTableDataSource(this.allProducts);

    });
  }

  applyFilter(value: string) {
    value = value.trim();
    value = value.toLowerCase();
    this.dataSource.filter = value;
  }



  ngOnDestroy(){
    
    console.log("destroy home page");
  }

}
