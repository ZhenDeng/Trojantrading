import { ProductService } from './../services/product.service';
import { Product } from './../models/Product';
import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { MatTableDataSource } from '@angular/material';
import { Menu } from '../models/menu';
import { ShareService } from '../services/share.service';

@Component({
  selector: 'app-products',
  templateUrl: './products.component.html',
  styleUrls: ['./products.component.css']
})
export class ProductsComponent implements OnInit {

  products: Product[] = [];

  filteredProducts: Product[] = [];

  dataSource = new MatTableDataSource();

  viewType: string;

  title: string = '';

  displayedColumns: string[] = ['name', 'category', 'originalPrice', 'qty', 'button'];

  role: string;
  
  navLinks:Menu[] = [
    {
      path: '/home',
      label: 'All Products',
      id: 'allProducts'
    },
    {
      path: '/productsview/new',
      label: 'New Product',
      id: 'newProduct'
    },
    {
      path: '/productsview/promotion',
      label: 'Promotions',
      id: 'promotions'
    },
    {
      path: '/productsview/soldout',
      label: 'Sold Out',
      id: 'soldout'
    },
  ];
  
  constructor(
    private router: Router,
    private activatedRouter: ActivatedRoute,
    private productService: ProductService,
    private shareService: ShareService
  ) { }

  ngOnInit() {
    this.role = this.shareService.readCookie("role");
    this.activatedRouter.paramMap.subscribe(param => {
        this.viewType = param.get('type');
        const type = this.viewType.toLowerCase();
        if ( type === 'new') {
          this.title = 'New Products';
        } else if (type.includes('promotion')) {
          this.title = 'Promotions';
        } else {
          this.title = 'Sold Out';
        }

        if(this.role === "admin") {
          this.displayedColumns = ['name', 'category', 'originalPrice', 'agentPrice', 'resellerPrice', 'qty', 'button'];
        } else if (this.role === "agent") {
          this.displayedColumns = ['name', 'category', 'originalPrice', 'agentPrice', 'qty', 'button'];
        } else if (this.role === "reseller") {
          this.displayedColumns = ['name', 'category', 'originalPrice', 'resellerPrice', 'qty', 'button'];
        }
        
        if (!this.products.length) {
          this.getProducts(type);
        } else {
          this.filterProducts(type, this.products);
        }
          
    });
  }

  getProducts(type: string) {
    this.productService.getAllProducts().subscribe((value: Product[]) =>{
      this.products = value;
      this.filterProducts(type, value);
    });
  }
 
  filterProducts(type: string, value: Product[]) {
      this.filteredProducts = value.filter(x => x.status.toLowerCase().includes(type));
      this.filteredProducts.forEach(product => product.quantity = 1);
      this.dataSource = new MatTableDataSource(this.filteredProducts);
  }
 
  applyFilter(value: string) {
    value = value.trim();
    value = value.toLowerCase();
    this.dataSource.filter = value;
  }

  _keyPress(event: any) {
    const pattern = /[0-9\+\-\ ]/;
    let inputChar = String.fromCharCode(event.charCode);

    if (!pattern.test(inputChar)) {
      // invalid character, prevent input
      event.preventDefault();
    }
  }

}
