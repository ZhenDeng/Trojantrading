import { Product } from './../models/Product';
import { ProductService } from './../services/product.service';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { Menu } from '../models/menu';
import { Router, ActivatedRoute } from '@angular/router';
import { NavbarService } from '../services/navbar.service';
import { ShareService } from '../services/share.service';
import { MatTableDataSource } from '@angular/material';
import { ShoppingCartService } from '../services/shopping-cart.service';
import { AdminService } from '../services/admin.service';
import { User } from '../models/user';
import { ApiResponse } from '../models/ApiResponse';
import { ShoppingItem } from '../models/shoppingItem';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']

})
export class HomeComponent implements OnInit, OnDestroy {

  title: string;

  allProducts: Product[] = [];
  filteredProducts: Product[] = [];
  role: string;
  category: string = '';
  dataSource = new MatTableDataSource();
  user: User;
  shoppingItem: ShoppingItem;

  displayedColumns: string[] = ['name', 'category', 'originalPrice', 'agentPrice', 'resellerPrice', 'qty', 'button'];

  navLinks: Menu[] = [
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


  isHomeComponentDestroyed: boolean = false;

  constructor(
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private shareService: ShareService,
    public nav: NavbarService,
    private productService: ProductService,
    private shoppingCartService: ShoppingCartService,
    private adminService: AdminService
  ) { }

  ngOnInit() {

    this.activatedRoute.queryParamMap.subscribe(param => {
      this.category = param.get('category');

      this.title = 'Products in All Categories';

      this.dataSource = new MatTableDataSource();

      this.getAllProducts();

    });

    let currentURL = this.router.url;
    if (!currentURL.includes('/home')) {
      this.isHomeComponentDestroyed = true;
    }
    this.role = this.shareService.readCookie("role");
    this.nav.showTab();
    this.getAllProducts();
    this.adminService.GetUserByAccount(this.shareService.readCookie("userName")).subscribe((res: User) => {
      if (res) {
        this.user = res;
        this.shoppingCartService.AddShoppingCart(res.id).subscribe((res: ApiResponse) => {
          console.info(res);
        },
          (error: any) => {
            console.info(error);
          });
      }
    },
      (error: any) => {
        console.info(error);
      });
  }


  getAllProducts() {
    this.productService.getAllProducts().subscribe((value: Product[]) => {
      this.allProducts = value;
      this.allProducts.forEach(product => product.quantity = 1);

      if (value.length && this.category != null && this.category != '') {
        this.filterProductsByCategory();
      } else {
        this.dataSource = new MatTableDataSource(this.allProducts);
      }

    },
      (error: any) => {
        console.info(error);
      });
  }

  filterProductsByCategory() {

    this.title = `Products ${this.category}`;
    this.filteredProducts = this.allProducts.filter(x => x.category.toLowerCase().includes(this.category.toLowerCase()));
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

  addToCart(product: Product): void {
    this.shoppingItem = {
      amount: product.quantity,
      product: product
    }
    this.shoppingCartService.UpdateShoppingCart(this.user.id, this.shoppingItem).subscribe((res: ApiResponse) => {
      if (res.status == "success") {
        this.shareService.showSuccess("#" + product.id, res.message, "right");
      }else{
        this.shareService.showError("#" + product.id, res.message, "right");
      }
    },
      (error: any) => {
        console.info(error);
      });
  }

  ngOnDestroy() {
    console.log("destroy home page");
  }

}
