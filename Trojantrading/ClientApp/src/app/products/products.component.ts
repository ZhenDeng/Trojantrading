import { ProductService } from './../services/product.service';
import { Product, Category } from './../models/Product';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { MatTableDataSource, MatDialog } from '@angular/material';
import { Menu } from '../models/menu';
import { ShareService } from '../services/share.service';
import { NavbarService } from '../services/navbar.service';
import { ApiResponse } from '../models/ApiResponse';
import { EditProductComponent } from '../popup-collection/edit-product/edit-product.component';
import { Subject } from 'rxjs';
import 'rxjs/add/operator/takeUntil';
import { DeleteConfirmComponent } from '../popup-collection/delete-confirm/delete-confirm.component';
import * as _ from 'lodash';
import { User } from '../models/user';
import { ShoppingItem } from '../models/shoppingItem';
import { ShoppingCart } from '../models/shoppingCart';
import { ShoppingCartService } from '../services/shopping-cart.service';

@Component({
  selector: 'app-products',
  templateUrl: './products.component.html',
  styleUrls: ['./products.component.css']
})
export class ProductsComponent implements OnInit, OnDestroy {

  products: Product[] = [];

  filteredProducts: Product[] = [];

  dataSource = new MatTableDataSource();

  viewType: string;

  title: string = '';

  displayedColumns: string[];

  role: string;

  categoryList: Category[] = [];

  shoppingItem: ShoppingItem;

  shoppingItems: ShoppingItem[];

  loadContent: boolean = false;

  navLinks: Menu[] = [
    {
      path: '/home',
      label: 'All Products',
      id: 'allProducts'
    },
    {
      path: '/productsview/new',
      label: 'New',
      id: 'newProduct'
    },
    {
      path: '/productsview/hot',
      label: 'Hot',
      id: 'hots'
    },
    {
      path: '/productsview/limited',
      label: 'Limited',
      id: 'limited'
    },
    {
      path: '/productsview/outofstock',
      label: 'Out of Stock',
      id: 'outofstock'
    },
  ];

   //unsubscribe
   ngUnsubscribe: Subject<void> = new Subject<void>();

  constructor(
    private router: Router,
    public nav: NavbarService,
    private shoppingCartService: ShoppingCartService,
    private activatedRouter: ActivatedRoute,
    private productService: ProductService,
    private shareService: ShareService,
    public dialog: MatDialog
  ) { }

  ngOnInit() {
    this.nav.show();
    this.loadContent = false;
    this.categoryList = this.productService.categoryList;
    this.role = this.shareService.readCookie("role");
    if (this.shareService.readCookie("role") && this.shareService.readCookie("role") == "admin") {
      this.displayedColumns = ['itemcode','name', 'category', 'packaging', 'originalPrice', 'agentPrice', 'wholesalerPrice', 'prepaymentDiscount', 'status', 'button', 'deletebutton']
    }
    else if (this.shareService.readCookie("role") && this.shareService.readCookie("role") == "agent") {
      this.displayedColumns = ['itemcode','name', 'category', 'packaging', 'originalPrice', 'agentPrice', 'qty', 'status', 'button']
    }
    else if (this.shareService.readCookie("role") && this.shareService.readCookie("role") == "wholesaler") {
      this.displayedColumns = ['itemcode','name', 'category', 'packaging', 'originalPrice', 'wholesalerPrice', 'prepaymentDiscount', 'qty', 'status', 'button']
    } else {
      this.displayedColumns = ['itemcode','name', 'category', 'packaging', 'originalPrice', 'qty', 'status', 'button']
    }
    this.activatedRouter.paramMap.subscribe(param => {
      this.viewType = param.get('type');
      const type = this.viewType.toLowerCase();
      if (type === 'new') {
        this.title = 'New';
      } else if (type.includes('hot')) {
        this.title = 'Hot';
      } else if (type.includes('limited')) {
        this.title = 'Limited';
      } else {
        this.title = 'Out of Stock';
      }
      if (!this.products.length) {
        this.getProducts(type);
      } else {
        this.filterProducts(type, this.products);
      }
      this.loadContent = true;
    },
      (error: any) => {
        this.loadContent = true;
        console.info(error);
      });
  }

  getProducts(type: string) {
    this.loadContent = false;
    this.productService.getAllProducts().takeUntil(this.ngUnsubscribe)
    .subscribe((value: Product[]) => {
      this.loadContent = true;
      this.products = value;
      this.filterProducts(type, value);
    });
  }

  filterProducts(type: string, value: Product[]) {
    this.filteredProducts = value.filter(x => x.status && x.status.toLowerCase().trim().includes(type.toLowerCase().trim()));
    this.filteredProducts.forEach(product => product.quantity = 0);
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
    if(product.packagingLists.length && !product.packaging){
      this.shareService.openSnackBar("Please select packaging", "error");
    }else{
      this.shoppingItem = {
        id: 0,
        amount: product.quantity,
        product: product,
        subTotal: 0
      }
      this.loadContent = false;
      this.shoppingCartService.UpdateShoppingCart(_.toNumber(this.shareService.readCookie("userId")), this.shoppingItem).subscribe((res: ApiResponse) => {
        if (res.status == "success") {
          this.shoppingCartService.GetShoppingCart(_.toNumber(this.shareService.readCookie("userId"))).subscribe((res: ShoppingCart) => {
            this.loadContent = true;
            this.shoppingItems = res.shoppingItems;
            this.shoppingCartService.MonitorShoppingItemLength(this.shoppingItems.length);
          },
            (error: any) => {
              this.loadContent = true;
              console.info(error);
            });
            this.shareService.openSnackBar(res.message, "success");
        } else {
          this.loadContent = true;
          this.shareService.openSnackBar(res.message, "error");
        }
      },
        (error: any) => {
          this.loadContent = true;
          console.info(error);
        });
    }
  }

  addNewProduct(): void {
    const dialogRef = this.dialog.open(EditProductComponent, {
      width: '700px',
      data: { categorys: this.productService.categoryList, type: "Add" }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.loadContent = false;
        this.productService.AddProduct(result).subscribe((res: ApiResponse) => {
          if (res.status == "success") {
            this.shareService.openSnackBar(res.message, "success");
            setTimeout(() => {
              this.activatedRouter.paramMap.subscribe(param => {
                this.viewType = param.get('type');
                const type = this.viewType.toLowerCase();
                if (type === 'new') {
                  this.title = 'New';
                } else if (type.includes('hot')) {
                  this.title = 'Hot';
                } else if (type.includes('limited')) {
                  this.title = 'Limited';
                } else {
                  this.title = 'Out of Stock';
                }
                if (!this.products.length) {
                  this.getProducts(type);
                } else {
                  this.filterProducts(type, this.products);
                }
              });
            }, 2000);
          } else {
            this.loadContent = true;
            this.shareService.openSnackBar(res.message, "error");
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
      data: { product: product, categorys: this.productService.categoryList, type: "Update" }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.loadContent = false;
        product.packagingLists = result.packaging;
        product.name = result.product.name;
        product.itemCode = result.product.itemCode;
        product.category = result.product.category;
        product.agentPrice = result.product.agentPrice;
        product.originalPrice = result.product.originalPrice;
        product.wholesalerPrice = result.product.wholesalerPrice;
        product.status = result.product.status;
        product.prepaymentDiscount = result.product.prepaymentDiscount;
        this.productService.UpdateProduct(product).subscribe((res: ApiResponse) => {
          if (res.status == "success") {
            this.shareService.openSnackBar(res.message, "success");
            setTimeout(() => {
              this.activatedRouter.paramMap.subscribe(param => {
                this.viewType = param.get('type');
                const type = this.viewType.toLowerCase();
                if (type === 'new') {
                  this.title = 'New';
                } else if (type.includes('hot')) {
                  this.title = 'Hot';
                } else if (type.includes('limited')) {
                  this.title = 'Limited';
                } else {
                  this.title = 'Out of Stock';
                }
                if (!this.products.length) {
                  this.getProducts(type);
                } else {
                  this.filterProducts(type, this.products);
                }
              });
            }, 1500);
          } else {
            this.loadContent = true;
            this.shareService.openSnackBar(res.message, "error");
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
            this.shareService.openSnackBar(res.message, "success");
            setTimeout(() => {
              this.activatedRouter.paramMap.subscribe(param => {
                this.viewType = param.get('type');
                const type = this.viewType.toLowerCase();
                if (type === 'new') {
                  this.title = 'New';
                } else if (type.includes('hot')) {
                  this.title = 'Hot';
                } else if (type.includes('limited')) {
                  this.title = 'Limited';
                } else {
                  this.title = 'Out of Stock';
                }
                this.getProducts(type);
              });
            }, 1500);
          } else {
            this.loadContent = true;
            this.shareService.openSnackBar(res.message, "error");
          }
        },
          (error: any) => {
            this.loadContent = true;
            console.info(error);
          });
      }
    });
  }

  manageRedirect(category: string) {
    this.dataSource.filter = category;
  }

  changeQuantity(element: Product): void {
    if (element.quantity < 0) {
      element.quantity = 0;
      this.shareService.openSnackBar("Minimum qty is 0", "error");
    }
  }

  switchLabel(id: string): void{
    if(id == "allProducts"){
      this.dataSource.filter ="";
    }else{
      this.activatedRouter.paramMap.subscribe(param => {
        this.viewType = param.get('type');
        const type = this.viewType.toLowerCase();
        this.filterProducts(type, this.products);
      });
    }
  }

  onLoading(currentLoadingStatus: boolean) {
    this.loadContent = !currentLoadingStatus;
  }

  ngOnDestroy() {
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }
}
