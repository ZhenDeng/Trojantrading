import { Product, Category } from './../models/Product';
import { ProductService } from './../services/product.service';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { Menu } from '../models/menu';
import { Router, ActivatedRoute } from '@angular/router';
import { NavbarService } from '../services/navbar.service';
import { ShareService } from '../services/share.service';
import { MatTableDataSource, MatDialog } from '@angular/material';
import { ShoppingCartService } from '../services/shopping-cart.service';
import { AdminService } from '../services/admin.service';
import { User } from '../models/user';
import { ApiResponse } from '../models/ApiResponse';
import { ShoppingItem } from '../models/shoppingItem';
import { ShoppingCart } from '../models/shoppingCart';
import * as _ from 'lodash';
import { EditProductComponent } from '../popup-collection/edit-product/edit-product.component';
import { NgbDropdownConfig } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
  providers: [NgbDropdownConfig]
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
  shoppingItems: ShoppingItem[];

  displayedColumns: string[];

  categoryList: Category[] = [];

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
    private adminService: AdminService,
    public dialog: MatDialog
  ) {}

  ngOnInit() {

    this.categoryList = this.productService.categoryList;
    this.activatedRoute.queryParamMap.subscribe(param => {
      this.category = param.get('category');

      this.title = 'Products in All Categories';
      if (this.shareService.readCookie("role") && this.shareService.readCookie("role") == "admin") {
        this.displayedColumns = ['name', 'category', 'originalPrice', 'agentPrice', 'resellerPrice', 'status', 'button']
      }
      else if (this.shareService.readCookie("role") && this.shareService.readCookie("role") == "agent") {
        this.displayedColumns = ['name', 'category', 'originalPrice', 'agentPrice', 'qty', 'status', 'button']
      }
      else if (this.shareService.readCookie("role") && this.shareService.readCookie("role") == "reseller") {
        this.displayedColumns = ['name', 'category', 'originalPrice', 'resellerPrice', 'qty', 'status', 'button']
      } else {
        this.displayedColumns = ['name', 'category', 'originalPrice', 'qty', 'status', 'button']
      }

      this.dataSource = new MatTableDataSource();

      this.getAllProducts();

    });

    let currentURL = this.router.url;
    if (!currentURL.includes('/home')) {
      this.isHomeComponentDestroyed = true;
    }
    this.role = this.shareService.readCookie("role");
    this.nav.show();
    this.getAllProducts();
    this.adminService.GetUserByAccount(_.toNumber(this.shareService.readCookie("userId"))).subscribe((res: User) => {
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
      id: 0,
      amount: product.quantity,
      product: product,
      subTotal: 0
    }
    this.shoppingCartService.UpdateShoppingCart(this.user.id, this.shoppingItem).subscribe((res: ApiResponse) => {
      if (res.status == "success") {
        this.adminService.GetUserByAccount(_.toNumber(this.shareService.readCookie("userId"))).subscribe((user: User) => {
          this.shoppingCartService.GetShoppingCart(user.id).subscribe((res: ShoppingCart) => {
            this.shoppingItems = res.shoppingItems;
            this.shoppingCartService.MonitorShoppingItemLength(this.shoppingItems.length);
          },
            (error: any) => {
              console.info(error);
            });
        },
          (error: any) => {
            console.info(error);
          });
        this.shareService.showSuccess("#" + product.id, res.message, "right");
      } else {
        this.shareService.showError("#" + product.id, res.message, "right");
      }
    },
      (error: any) => {
        console.info(error);
      });
  }

  addNewProduct(): void {
    const dialogRef = this.dialog.open(EditProductComponent, {
      width: '700px',
      data: { categorys: this.productService.categoryList }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.productService.AddProduct(result).subscribe((res: ApiResponse) => {
          if (res.status == "success") {
            this.shareService.showSuccess(".addnewproduct", res.message, "right");
            setTimeout(() => {
              this.getAllProducts();
            }, 2000);
          } else {
            this.shareService.showError(".addnewproduct", res.message, "right");
          }
        },
          (error: any) => {
            console.info(error);
          });
      }
    });
  }

  editProduct(product: Product): void {
    const dialogRef = this.dialog.open(EditProductComponent, {
      width: '700px',
      data: { product: product, categorys: this.productService.categoryList }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        product.name = result.name;
        product.category = result.category;
        product.agentPrice = result.agentPrice;
        product.originalPrice = result.originalPrice;
        product.resellerPrice = result.resellerPrice;
        this.productService.UpdateProduct(product).subscribe((res: ApiResponse) => {
          if (res.status == "success") {
            this.shareService.showSuccess("#" + product.id, res.message, "right");
            setTimeout(() => {
              this.getAllProducts();
            }, 2000);
          } else {
            this.shareService.showError("#" + product.id, res.message, "right");
          }
        },
          (error: any) => {
            console.info(error);
          });
      }
    });
  }

  changeQuantity(element: Product): void{
    if(element.quantity<1){
      element.quantity = 1;
      this.shareService.showError("#product"+element.id, "Minimum qty is 1", "right");
    }
  }

  manageRedirect(category: string) {
    this.dataSource.filter = category;
  }

  switchLabel(id: string): void{
    if(id == "allProducts"){
      this.dataSource.filter ="";
    }
  }

  ngOnDestroy() {
    console.log("destroy home page");
  }

}
