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
import { ApiResponse } from '../models/ApiResponse';
import { ShoppingItem } from '../models/shoppingItem';
import { ShoppingCart } from '../models/shoppingCart';
import * as _ from 'lodash';
import { EditProductComponent } from '../popup-collection/edit-product/edit-product.component';
import { NgbDropdownConfig } from '@ng-bootstrap/ng-bootstrap';
import { Subject } from 'rxjs';
import 'rxjs/add/operator/takeUntil';
import { DeleteConfirmComponent } from '../popup-collection/delete-confirm/delete-confirm.component';
import { UploadProductsComponent } from '../popup-collection/upload-products/upload-products.component';

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

        this.getAllProducts();
      });

      let currentURL = this.router.url;
      if (!currentURL.includes('/home')) {
        this.isHomeComponentDestroyed = true;
      }
      this.role = this.shareService.readCookie("role");
      this.nav.show();
      if(this.shareService.readCookie("userId")){
        this.shoppingCartService.AddShoppingCart(_.toNumber(this.shareService.readCookie("userId"))).subscribe((res: ApiResponse) => {
          this.loadContent = true;
        },
          (error: any) => {
            this.loadContent = true;
            console.info(error);
          });
      }
    }

  }


  getAllProducts() {
    this.loadContent = false;
    this.productService.getAllProducts().takeUntil(this.ngUnsubscribe)
    .subscribe((value: Product[]) => {
      this.allProducts = value;
      this.allProducts.forEach(product => product.quantity = 0);
      this.allProducts.forEach(product => product.packaging="");
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
    if(product.packagingLists.length && !product.packaging){
      this.shareService.openSnackBar("Please select packaging", "error");
    }else{
      this.shoppingItem = {
        id: 0,
        amount: product.quantity,
        product: product,
        subTotal: 0,
        packaging: product.packaging
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
      data: { categorys: this.productService.categoryList }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.loadContent = false;
        this.productService.AddProduct(result).subscribe((res: ApiResponse) => {
          if (res.status == "success") {
            this.shareService.openSnackBar(res.message, "success");
            setTimeout(() => {
              this.getAllProducts();
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
      data: { product: product, categorys: this.productService.categoryList }
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
            this.productService.UpdatePackagingList(product.id, product.packagingLists).subscribe((result: ApiResponse) => {
              if(result.status == "success"){
                this.loadContent = true;
                this.getAllProducts();
              }else{
                this.loadContent = true;
                this.shareService.openSnackBar(result.message, "error");
              }
            },
              (error: any) => {
                this.loadContent = true;
                console.info(error);
              });
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
              this.getAllProducts();
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

  changeQuantity(element: Product): void {
    if(element.quantity){
      if (element.quantity < 0) {
        element.quantity = 0;
        this.shareService.openSnackBar("Minimum qty is 0", "error");
      }
    }else{
      element.quantity = 0;
      this.shareService.openSnackBar("Minimum qty is 0", "error");
    }
  }

  uploadProduct(): void{
    const dialogRef = this.dialog.open(UploadProductsComponent, {
      width: '700px',
    });

    dialogRef.afterClosed().subscribe(result => {
      this.getAllProducts();
    });
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
