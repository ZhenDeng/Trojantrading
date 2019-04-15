import { ProductService } from './../services/product.service';
import { Product } from './../models/Product';
import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { MatTableDataSource } from '@angular/material';
import { Menu } from '../models/menu';

@Component({
  selector: 'app-products',
  templateUrl: './products.component.html',
  styleUrls: ['./products.component.css']
})
export class ProductsComponent implements OnInit {

  products: Product[] = [];

  dataSource = new MatTableDataSource();

  viewType: string;

  title: string = '';

  displayedColumns: string[] = ['name', 'category', 'originalPrice', 'qty', 'button'];

  
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
    private productService: ProductService
  ) { }

  ngOnInit() {

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

        this.getProducts(type);
    });
  }

  getProducts(type: string) {
    this.productService.getAllProducts().subscribe((value: Product[]) =>{
        this.products = value.filter(x => x.status.toLowerCase().includes(type));
        this.products.forEach(product => product.quantity = 1);
        this.dataSource = new MatTableDataSource(this.products);

    });
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



  ngOnDestroy(){
    
  }

}
