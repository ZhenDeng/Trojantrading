import { Product } from './../models/Product';
import { ProductService } from './../services/product.service';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { Menu } from '../models/menu';
import { Router } from '@angular/router';

export interface PeriodicElement {
  wlpPrice: number;
  description: string;
  buyPrice: number;
  qty: string;
  button: string
}

const ELEMENT_DATA: PeriodicElement[] = [
  {description: "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor.", wlpPrice: 1.5, buyPrice: 1.0079, qty: 'H', button: "test"},
  {description:  "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor.", wlpPrice: 1.5, buyPrice: 4.0026, qty: 'He', button: "test"},
  {description:  "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor.", wlpPrice: 1.5, buyPrice: 6.941, qty: 'Li', button: "test"},
  {description:  "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor.", wlpPrice: 1.5, buyPrice: 9.0122, qty: 'Be', button: "test"},
  {description:  "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor.", wlpPrice: 1.5, buyPrice: 10.811, qty: 'B', button: "test"},
  {description:  "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor.", wlpPrice: 1.5, buyPrice: 12.0107, qty: 'C', button: "test"},
  {description:  "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor.", wlpPrice: 1.5, buyPrice: 14.0067, qty: 'N', button: "test"},
  {description:  "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor.", wlpPrice: 1.5, buyPrice: 15.9994, qty: 'O', button: "test"},
  {description:  "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor.", wlpPrice: 1.5, buyPrice: 18.9984, qty: 'F', button: "test"},
  {description:  "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor.", wlpPrice: 1.5, buyPrice: 20.1797, qty: 'Ne', button: "test"},
];

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']

})
export class HomeComponent implements OnInit, OnDestroy {

  displayedColumns: string[] = ['name', 'vipOnePrice', 'originalPrice', 'qty', 'button'];

  dataSource = ELEMENT_DATA;

  allProducts: Product[] = [];
  

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
  ]

  isHomeComponentDestroyed:boolean = false;

  constructor(
    private router: Router,
    private productService: ProductService
  ) {}

  ngOnInit() {
    //this.dataSource = ELEMENT_DATA;
    let currentURL = this.router.url;
    if(currentURL != '/home'){
      this.isHomeComponentDestroyed = true;
    }
    
    this.getAllProducts();
  }

  getAllProducts() {
    console.info('products');
    this.productService.getAllProducts().subscribe((value: Product[]) =>{
        this.allProducts = value;
    });
  }

  ngOnDestroy(){
    
    console.log("destroy home page");
  }

}
