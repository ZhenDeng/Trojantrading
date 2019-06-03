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
import { Subject } from 'rxjs';
import 'rxjs/add/operator/takeUntil';
import { DeleteConfirmComponent } from '../popup-collection/delete-confirm/delete-confirm.component';

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
  loadContent: boolean = false;
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
      path: '/productsview/outofstock',
      label: 'Out of Stock',
      id: 'outofstock'
    },
  ];


  isHomeComponentDestroyed: boolean = false;

   //unsubscribe
  ngUnsubscribe: Subject<void> = new Subject<void>();

  constructor(
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private shareService: ShareService,
    public nav: NavbarService,
    private productService: ProductService,
    private shoppingCartService: ShoppingCartService,
    private adminService: AdminService,
    public dialog: MatDialog
  ) { }

  ngOnInit() {
    this.loadContent = false;
    if (!this.shareService.readCookie("userToken")) {
      this.loadContent = true;
      this.router.navigateByUrl('/login');
    } else {
      this.categoryList = this.productService.categoryList;
      this.activatedRoute.queryParamMap.subscribe(param => {
        this.category = param.get('category');

        this.title = 'Products in All Categories';
        if (this.shareService.readCookie("role") && this.shareService.readCookie("role") == "admin") {
          this.displayedColumns = ['itemcode','name', 'category', 'originalPrice', 'agentPrice', 'wholesalerPrice', 'prepaymentDiscount', 'status', 'button', 'deletebutton']
        }
        else if (this.shareService.readCookie("role") && this.shareService.readCookie("role") == "agent") {
          this.displayedColumns = ['itemcode','name', 'category', 'originalPrice', 'agentPrice', 'qty', 'status', 'button']
        }
        else if (this.shareService.readCookie("role") && this.shareService.readCookie("role") == "wholesaler") {
          this.displayedColumns = ['itemcode','name', 'category', 'originalPrice', 'wholesalerPrice', 'prepaymentDiscount', 'qty', 'status', 'button']
        } else {
          this.displayedColumns = ['itemcode','name', 'category', 'originalPrice', 'qty', 'status', 'button']
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
      this.adminService.GetUserByAccount(_.toNumber(this.shareService.readCookie("userId"))).takeUntil(this.ngUnsubscribe)
      .subscribe((res: User) => {
        this.loadContent = true;
        if (res) {
          this.user = res;
          this.shoppingCartService.AddShoppingCart(res.id).subscribe((res: ApiResponse) => {
            console.info(res);
          },
            (error: any) => {
              this.loadContent = true;
              console.info(error);
            });
        }
      },
        (error: any) => {
          this.loadContent = true;
          console.info(error);
        });
    }

  }


  getAllProducts() {
    this.loadContent = false;
    this.productService.getAllProducts().takeUntil(this.ngUnsubscribe)
    .subscribe((value: Product[]) => {
      this.allProducts = value;
      this.allProducts.forEach(product => product.quantity = 0);
      this.loadContent = true;
      if (value.length && this.category != null && this.category != '') {
        this.filterProductsByCategory();
      } else {
        this.dataSource = new MatTableDataSource(_.orderBy(this.allProducts, 'name'));
      }
    },
      (error: any) => {
        this.loadContent = true;
        console.info(error);
      });
  }

  onLoading(currentLoadingStatus: boolean) {
    this.loadContent = !currentLoadingStatus;
  }

  filterProductsByCategory() {
    this.title = `Products ${this.category}`;
    this.filteredProducts = this.allProducts.filter(x => x.category.toLowerCase().includes(this.category.toLowerCase()));
    this.dataSource = new MatTableDataSource(_.orderBy(this.filteredProducts, 'name'));
  }

  applyFilter(value: string) {
    value = value.trim();
    value = value.toLowerCase();
    this.dataSource.filter = value;
  }

  isNumberKey(event: any) {
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
    this.loadContent = false;
    this.shoppingCartService.UpdateShoppingCart(this.user.id, this.shoppingItem).subscribe((res: ApiResponse) => {
      if (res.status == "success") {
        this.adminService.GetUserByAccount(_.toNumber(this.shareService.readCookie("userId"))).subscribe((user: User) => {
          this.shoppingCartService.GetShoppingCart(user.id).subscribe((res: ShoppingCart) => {
            this.loadContent = true;
            this.shoppingItems = res.shoppingItems;
            this.shoppingCartService.MonitorShoppingItemLength(this.shoppingItems.length);
          },
            (error: any) => {
              this.loadContent = true;
              console.info(error);
            });
        },
          (error: any) => {
            this.loadContent = true;
            console.info(error);
          });
        this.shareService.showSuccess("#" + product.id, res.message, "right");
      } else {
        this.loadContent = true;
        this.shareService.showError("#" + product.id, res.message, "right");
      }
    },
      (error: any) => {
        this.loadContent = true;
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
        this.loadContent = false;
        this.productService.AddProduct(result).subscribe((res: ApiResponse) => {
          if (res.status == "success") {
            this.shareService.showSuccess(".addnewproduct", res.message, "right");
            setTimeout(() => {
              this.getAllProducts();
            }, 2000);
          } else {
            this.loadContent = true;
            this.shareService.showError(".addnewproduct", res.message, "right");
          }
        },
          (error: any) => {
            this.loadContent = true;
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
        this.loadContent = false;
        product.name = result.name;
        product.itemCode = result.itemCode;
        product.category = result.category;
        product.agentPrice = result.agentPrice;
        product.originalPrice = result.originalPrice;
        product.wholesalerPrice = result.wholesalerPrice;
        product.status = result.status;
        product.prepaymentDiscount = result.prepaymentDiscount;
        this.productService.UpdateProduct(product).subscribe((res: ApiResponse) => {
          if (res.status == "success") {
            this.shareService.showSuccess("#" + product.id, res.message, "right");
            setTimeout(() => {
              this.getAllProducts();
            }, 2000);
          } else {
            this.loadContent = true;
            this.shareService.showError("#" + product.id, res.message, "right");
          }
        },
          (error: any) => {
            this.loadContent = true;
            console.info(error);
          });
      }
    });
  }

  deleteProduct(element: Product): void{
    const dialogRef = this.dialog.open(DeleteConfirmComponent, {
      width: '500px',
    });

    dialogRef.afterClosed().subscribe(result => {
      if(result){
        this.productService.DeleteProduct(element).subscribe((res: ApiResponse) => {
          if (res.status == "success") {
            this.shareService.showSuccess(".delete" + element.id, res.message, "right");
            setTimeout(() => {
              this.getAllProducts();
            }, 2000);
          } else {
            this.loadContent = true;
            this.shareService.showError(".delete" + element.id, res.message, "right");
          }
        },
          (error: any) => {
            this.loadContent = true;
            console.info(error);
          });
      }
    });
  }

  changeQuantity(element: Product): void {
    if(element.quantity){
      if (element.quantity < 0) {
        element.quantity = 0;
        this.shareService.showError("#product" + element.id, "Minimum qty is 0", "right");
      }
    }else{
      element.quantity = 0;
      this.shareService.showError("#product" + element.id, "Minimum qty is 0", "right");
    }
  }

  manageRedirect(category: string) {
    this.dataSource.filter = category;
  }

  switchLabel(id: string): void {
    if (id == "allProducts") {
      this.dataSource.filter = "";
    }
  }

  ngOnDestroy() {
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }
}
